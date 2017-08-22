﻿using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Model;
using Microarea.Mago4Butler.Plugins;
using Microarea.Tools.ProvisioningConfigurator.ProvisioningConfigurator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WinApp = System.Windows.Forms.Application;

namespace Microarea.Mago4Butler
{
    internal partial class MainForm : Form, IMainUI
    {
        SynchronizationContext syncCtx;

        UIEmpty uiEmpty;
        UIWaiting uiWaiting;
        UIWaitingMinimized uiWaitingMinimized;
        UIWaitingMinimizedFactory uiWaitingMinimizedFactory;
        UIError uiError;
        UINormalUse uiNormalUse;

        IUIMediator uiMediator;
        PluginService pluginService;
        ISettings settings;

        public MainForm(
            IUIMediator uiMediator,
            ISettings settings,
            PluginService pluginService,
            UIEmpty uiEmpty,
            UIWaiting uiWaiting,
            UIWaitingMinimizedFactory uiWaitingMinimizedFactory,
            UIError uiError,
            UINormalUse uiNormalUse
            )
        {
            this.syncCtx = SynchronizationContext.Current;
            if (this.syncCtx == null)
            {
                this.syncCtx = new WindowsFormsSynchronizationContext();
            }

            this.uiMediator = uiMediator;

            this.settings = settings;

            this.pluginService = pluginService;

            InitializeComponent();

            this.uiNormalUse = uiNormalUse;
            this.uiEmpty = uiEmpty;
            this.uiWaiting = uiWaiting;
            this.uiWaitingMinimizedFactory = uiWaitingMinimizedFactory;
            this.uiError = uiError;

            WinApp.Idle += Application_Idle;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (!this.uiMediator.CanClose)
            {
                e.Cancel = true;
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            WinApp.Idle -= Application_Idle;

            this.Text = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} v. {1}", this.Text, this.GetType().Assembly.GetName().Version.ToString());

            this.uiMediator.JobNotification += UiMediator_Notification;
            this.uiMediator.ParametersForInstallNeeded += UiMediator_AskForParametersForInstall;
            this.uiMediator.ProvisioningNeeded += UiMediator_ProvisioningNeeded;

            this.uiEmpty.SelectMsiToInstall += (s, args) => this.uiMediator.InstallInstance();
            this.uiError.Back += UiError_Back;
            this.uiNormalUse.InstallNewInstance += (s, args) => this.uiMediator.InstallInstance();
            this.uiNormalUse.UpdateInstance += UiNormalUse_UpdateInstance;
            this.uiNormalUse.RemoveInstance += UiNormalUse_RemoveInstance;
            this.uiWaiting.Back += UiWaiting_Back;

            InitToolstripMenuItems();

            Thread.Sleep(1000);
            UpdateUI();
        }

        private void InitToolstripMenuItems()
        {
            foreach (var plugin in this.pluginService.Plugins)
            {
                if (plugin != null)
                {
                    var toolstripMenuItems = plugin.GetToolstripMenuItems();
                    if (toolstripMenuItems != null && toolstripMenuItems.Count() > 0)
                    {
                        foreach (var toolstripMenuItem in toolstripMenuItems)
                        {
                            AddToolstripItem(toolstripMenuItem, this.toolStrip.Items);
                        }
                    }
                }
            }
        }

        private void AddToolstripItem(ToolstripMenuItem toolstripMenuItem, ToolStripItemCollection items)
        {
            var menuItem = new ToolStripButton();
            menuItem.Font = this.toolStrip.Font;
            menuItem.Name = toolstripMenuItem.Name;
            menuItem.Text = toolstripMenuItem.Text;

            items.Insert(this.toolStrip.Items.Count - 1, menuItem);

            if (toolstripMenuItem.Command != null)
            {
                var handler = new ToolstripMenuItemClickHandler() { ToolstripMenuItem = toolstripMenuItem };
                menuItem.Click += handler.MenuItem_Click;
                menuItem.Tag = handler;
            }
        }

        private void CreateUIWaitingMinimized()
        {
            if (this.uiWaitingMinimized != null)
            {
                DestroyUIWaitingMinimized();
            }
            this.uiWaitingMinimized = this.uiWaitingMinimizedFactory.CreateWaitingWindow();
            this.uiWaitingMinimized.WindowClose += UiWaitingMinimized_WindowClose;
            this.uiWaitingMinimized.AttachToMainUI(this);
            this.uiWaitingMinimized.AttachToMainUiWaiting(this.uiWaiting);
            this.uiWaitingMinimized.Show(this);
        }
        private void DestroyUIWaitingMinimized()
        {
            if (this.uiWaitingMinimized != null)
            {
                this.uiWaitingMinimized.WindowClose -= UiWaitingMinimized_WindowClose;
                this.uiWaitingMinimized.DetachToMainUI();
                this.uiWaitingMinimized.DetachToMainUiWaiting();

                if (!this.uiWaitingMinimized.IsDisposed)
                {
                    this.uiWaitingMinimized.Dispose();
                }
                this.uiWaitingMinimized = null;
            }
        }

        private void UiMediator_ProvisioningNeeded(object sender, ProvisioningEventArgs e)
        {
            using (var provisioningForm = new ProvisioningFormLITE(instanceName: e.InstanceName, preconfigurationMode: true, loadDataFromFile: false))
            {
                var provisioningSuggestions = new ProvisioningData()
                {
                    CompanyDbName = string.Format("{0}_DB", e.InstanceName),
                    DMSDbName = string.Format("{0}_DBDMS", e.InstanceName),
                    SystemDbName = string.Format("{0}_SystemDB", e.InstanceName),
                    AdminLoginName = string.Format("{0}_admin", e.InstanceName),
                    UserLoginName = string.Format("{0}_user", e.InstanceName)
                };

                provisioningForm.ProvisioningData = provisioningSuggestions;

                var diagRes = provisioningForm.ShowDialog(this);

                if (diagRes == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                e.ProvisioningCommandLine = provisioningForm.PreconfigurationCommandLine;
            }
        }

        private void UiMediator_AskForParametersForInstall(object sender, JobEventArgs e)
        {
            using (var askForParametersDialog = IoCContainer.Instance.Get<AskForParametersForm>())
            {
                var diagRes = askForParametersDialog.ShowDialog(this);

                if (diagRes != DialogResult.OK)
                {
                    e.Bag.Cancel = true;
                    return;
                }
                e.Bag.InstanceName = askForParametersDialog.InstanceName;
            }
        }

        private void UiMediator_Notification(object sender, JobEventArgs e)
        {
            if ((e.NotificationType & NotificationTypes.JobStarted) == NotificationTypes.JobStarted)
            {
                this.syncCtx.Post((_) =>
                {
                    this.syncCtx.Post((obj) => uiWaiting.ClearDetails(), null);

                    ShowUI(uiWaiting);
                    EnableDisableToolStripItem(this.tsbSettings, false);
                }, null);
            }
            if ((e.NotificationType & NotificationTypes.JobEnded) == NotificationTypes.JobEnded)
            {
                this.syncCtx.Post((_) =>
                {
                    DestroyUIWaitingMinimized();
                    UpdateUI();

                    EnableDisableToolStripItem(this.tsbSettings, true);
                }, null);
            }
            if ((e.NotificationType & NotificationTypes.Progress) == NotificationTypes.Progress)
            {
                this.uiWaiting.SetProgressText(e.Progress);
            }
            if ((e.NotificationType & NotificationTypes.Notification) == NotificationTypes.Notification)
            {
                this.uiWaiting.AddDetailsText(e.Notification);
            }
            if ((e.NotificationType & NotificationTypes.Error) == NotificationTypes.Error)
            {
                this.uiError.SetErrorMessage(e.Error.Message);
                ShowUI(this.uiError);
            }
        }

        private void UiWaitingMinimized_WindowClose(object sender, EventArgs e)
        {
            DestroyUIWaitingMinimized();
            ShowUI(this.uiWaiting);
        }

        private void UiWaiting_Back(object sender, EventArgs e)
        {
            CreateUIWaitingMinimized();

            UpdateUI();
        }

        void UpdateUI()
        {
            if (this.uiMediator.ShouldShowEmptyUI)
            {
                ShowUI(this.uiEmpty);
            }
            else
            {
                ShowUI(this.uiNormalUse);
            }
        }

        private void UiError_Back(object sender, EventArgs e)
        {
            if (this.uiMediator.ShouldUserWait)
            {
                ShowUI(this.uiWaiting);
                return;
            }
            UpdateUI();
        }

        private void ShowUI(UserControl ui)
        {
            this.syncCtx.Post((obj) =>
            {
                this.pnlContent.SuspendLayout();
                if (this.pnlContent.Controls.Count > 0)
                {
                    this.pnlContent.Controls[0].Visible = false;
                    this.pnlContent.Controls.Clear();
                }
                this.pnlContent.Controls.Add(ui);
                this.pnlContent.Controls[0].Visible = true;
                ui.Dock = DockStyle.Fill;
                this.pnlContent.ResumeLayout();
            }
            , null);
        }

        private void EnableDisableToolStripItem(ToolStripItem item, bool enabled)
        {
            this.syncCtx.Post((obj) => item.Enabled = enabled, null);
        }

        private void bntAbout_Click(object sender, EventArgs e)
        {
            using (var aboutForm = IoCContainer.Instance.Get<AboutForm>())
            {
                aboutForm.ShowDialog(this);
            }
        }

        private void UiNormalUse_RemoveInstance(object sender, RemoveInstanceEventArgs e)
        {
            this.uiMediator.RemoveInstances(e.Instances);
        }

        private void UiNormalUse_UpdateInstance(object sender, UpdateInstanceEventArgs e)
        {
            this.uiMediator.UpdateInstances(e.Instances);
        }

        private void tsbSettings_Click(object sender, EventArgs e)
        {
            var oldRootFolder = this.settings.RootFolder;

            using (var settingsForm = IoCContainer.Instance.Get<SettingsForm>())
            {
                settingsForm.ShowDialog(this);
            }
            if (String.Compare(oldRootFolder, this.settings.RootFolder, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                this.uiMediator.Init();
                UpdateUI();
            }
        }

        private void tsbViewLogs_Click(object sender, EventArgs e)
        {
            var logFileFullPath = Path.Combine(this.settings.LogsFolder, Program.LogFileName);
            if (!File.Exists(logFileFullPath))
            {
                MessageBox.Show(this, "No log file found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Process.Start(logFileFullPath);
        }
    }
}
