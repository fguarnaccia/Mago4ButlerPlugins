using log4net;
using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Plugins;
using Microarea.Tools.ProvisioningConfigurator.ProvisioningConfigurator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class MainForm : Form
    {
        SynchronizationContext syncCtx;

        UIEmpty uiEmpty;
        UIWaiting uiWaiting;
        UIError uiError;
        UINormalUse uiNormalUse;

        ISettings settings;

        Model model;
        MsiService msiService;
        InstallerService installerService;
        LoggerService loggerService;

        string msiFullFilePath;

        public MainForm(
            Model model,
            MsiService msiService,
            InstallerService installerService,
            LoggerService loggerService,
            ISettings settings,
            UIEmpty uiEmpty,
            UIWaiting uiWaiting,
            UIError uiError,
            UINormalUse uiNormalUse
            )
        {
            this.loggerService = loggerService;
            this.syncCtx = SynchronizationContext.Current;
            if (this.syncCtx == null)
            {
                this.syncCtx = new WindowsFormsSynchronizationContext();
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;

            this.model = model;

            this.msiService = msiService;
            this.installerService = installerService;

            this.settings = settings;

            InitializeComponent();

            this.uiNormalUse = uiNormalUse;
            this.uiEmpty = uiEmpty;
            this.uiWaiting = uiWaiting;
            this.uiError = uiError;

            Application.Idle += Application_Idle;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (this.installerService.IsRunning)
            {
                e.Cancel = true;
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= Application_Idle;

            this.Text = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} v. {1}", this.Text, this.GetType().Assembly.GetName().Version.ToString());

            this.model.InstanceAdded += (s, iea) => this.installerService.Install(this.msiFullFilePath, iea.Instance);
            this.model.InstanceUpdated += (s, iea) => this.installerService.Update(this.msiFullFilePath, iea.Instance);
            this.model.InstanceRemoved += (s, iea) => this.installerService.Uninstall(iea.Instance);

            this.uiEmpty.SelectMsiToInstall += Install;
            this.uiError.Back += UiError_Back;
            this.uiNormalUse.InstallNewInstance += Install;
            this.uiNormalUse.UpdateInstance += UiNormalUse_UpdateInstance;
            this.uiNormalUse.RemoveInstance += UiNormalUse_RemoveInstance;

            this.installerService.Started += InstallerService_Started;
            this.installerService.Starting += InstallerService_Starting;
            this.installerService.Stopped += InstallerService_Stopped;
            this.installerService.Stopping += InstallerService_Stopping;

            this.installerService.Installing += InstallerService_Installing;
            this.installerService.Installed += InstallerService_Installed;
            this.installerService.Removing += InstallerService_Removing;
            this.installerService.Removed += InstallerService_Removed;
            this.installerService.Updating += InstallerService_Updating;
            this.installerService.Updated += InstallerService_Updated;
            this.installerService.Error += InstallerService_Error;

            this.installerService.Notification += InstallerService_Notification;

            Thread.Sleep(1000);
            UpdateUI();
        }

        private void InstallerService_Error(object sender, InstallerServiceErrorEventArgs e)
        {
            ManageException(e.Error);
        }

        void UpdateUI()
        {
            if (this.model.Instances.Count() > 0)
            {
                ShowUI(this.uiNormalUse);
            }
            else
            {
                ShowUI(this.uiEmpty);
            }
        }

        private void InstallerService_Notification(object sender, NotificationEventArgs e)
        {
            this.loggerService.LogInfo(e.Message);
            this.uiWaiting.AddDetailsText(e.Message);
        }

        private void InstallerService_Updated(object sender, UpdateInstanceEventArgs e)
        {
            this.loggerService.LogInfo(e.Instances[0].Name + " successfully updated");
            this.uiWaiting.SetProgressText(e.Instances[0].Name + " successfully updated");
        }

        private void InstallerService_Updating(object sender, UpdateInstanceEventArgs e)
        {
            this.loggerService.LogInfo("Updating " + e.Instances[0].Name + " ...");
            this.uiWaiting.SetProgressText("Updating " + e.Instances[0].Name + " ...");
        }

        private void InstallerService_Removed(object sender, RemoveInstanceEventArgs e)
        {
            this.loggerService.LogInfo(e.Instances[0].Name + " successfully removed");
            this.uiWaiting.SetProgressText(e.Instances[0].Name + " successfully removed");
        }

        private void InstallerService_Removing(object sender, RemoveInstanceEventArgs e)
        {
            this.loggerService.LogInfo("Removing " + e.Instances[0].Name + " ...");
            this.uiWaiting.SetProgressText("Removing " + e.Instances[0].Name + " ...");
        }

        private void InstallerService_Installed(object sender, InstallInstanceEventArgs e)
        {
            this.loggerService.LogInfo("Database configuration...");
            this.uiWaiting.SetProgressText("Database configuration...");

            this.uiWaiting.AddDetailsText("Database configuration...");
            try
            {
                var productName = msiService.GetProductName(this.msiFullFilePath).ToLowerInvariant(); ;
                var provisioningService = IoCContainer.Instance.Get<IProvisioningService>(productName);

                if (provisioningService.ShouldStartProvisioning)
                {
                    provisioningService.StartProvisioning(e.Instance);
                    this.loggerService.LogInfo("Database configuration ended");
                    this.uiWaiting.AddDetailsText("Database configuration ended");
                }
                else
                {
                    this.loggerService.LogInfo(String.Format(System.Globalization.CultureInfo.InvariantCulture, "No database provisioning for {0}, database configuration skipped", e.Instance.Name));
                    this.uiWaiting.AddDetailsText(String.Format(System.Globalization.CultureInfo.InvariantCulture, "No database provisioning for {0}, database configuration skipped", e.Instance.Name));
                }
            }
            catch (Exception exc)
            {
                this.loggerService.LogError("Database configurator returned an error", exc);
                this.uiWaiting.AddDetailsText("Database configurator returned the following error: " + exc.Message);
            }
        }

        private void InstallerService_Installing(object sender, InstallInstanceEventArgs e)
        {
            this.loggerService.LogInfo("Installing " + e.Instance.Name + " ...");
            this.uiWaiting.SetProgressText("Installing " + e.Instance.Name + " ...");
        }

        private void InstallerService_Stopping(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("Stopping installer service");
        }

        private void InstallerService_Stopped(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("Installer service stopped");
            var ui = (this.model.Instances.Count() == 0) ? this.uiEmpty as UserControl : this.uiNormalUse as UserControl;
            ShowUI(ui);
            EnableDisableToolStripItem(this.tsbSettings, true);
        }

        private void InstallerService_Starting(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("Starting installer service");
        }

        private void InstallerService_Started(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("Installer service started");
            ShowUI(uiWaiting);
            EnableDisableToolStripItem(this.tsbSettings, false);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exc = e.ExceptionObject as Exception;
            Debug.Assert(exc != null);

            ManageException(exc);
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ManageException(e.Exception);
        }

        private void ManageException(Exception e)
        {
            this.loggerService.LogError("Application error", e);
            this.uiError.SetErrorMessage(e.Message);
            ShowUI(this.uiError);
        }

        private void UiError_Back(object sender, EventArgs e)
        {
            if (this.installerService.IsRunning)
            {
                ShowUI(this.uiWaiting);
                return;
            }
            var ui = (this.model.Instances.Count() == 0) ? this.uiEmpty as UserControl : this.uiNormalUse as UserControl;
            ShowUI(ui);
        }

        private void ShowUI(UserControl ui)
        {
            this.syncCtx.Post(new SendOrPostCallback((obj) =>
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
            })
            , null);
        }

        private void EnableDisableToolStripItem(ToolStripItem item, bool enabled)
        {
            this.syncCtx.Post(new SendOrPostCallback((obj) =>
            {
                item.Enabled = enabled;
            })
            , null);
        }

        private void bntAbout_Click(object sender, EventArgs e)
        {
            var version = GetType().Assembly.GetName().Version.ToString();
            MessageBox.Show(
                this,
                "The logo and the icon are built upon Jenkins logo (https://wiki.jenkins-ci.org/display/JENKINS/Logo) and are distributed under the 'Attribution - ShareAlike 3.0 Unported(CC BY - SA 3.0)' license (http://creativecommons.org/licenses/by-sa/3.0/)",
                String.Format("Mago4 Butler v. {0}", version),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1
                );
        }

        private void Install(object sender, InstallInstanceEventArgs e)
        {
            if (this.settings.ShowRootFolderChoice)
            {
                using (var chooseRootFolderDialog = new ChooseRootFolderForm(this.settings))
                {
                    chooseRootFolderDialog.ShowDialog();
                }
                this.settings.ShowRootFolderChoice = false;
                this.settings.Save();
            }
            using (var askForParametersDialog = new AskForParametersForm(this.model, this.settings))
            {
                var diagRes = askForParametersDialog.ShowDialog();

                if (diagRes != DialogResult.OK)
                {
                    return;
                }
                this.msiFullFilePath = askForParametersDialog.MsiFullPath;

                using (var provisioningForm = new ProvisioningForm(instanceName: askForParametersDialog.InstanceName, preconfigurationMode: true, loadDataFromFile: false))
                {
                    var productName = msiService.GetProductName(this.msiFullFilePath).ToLowerInvariant();
                    var provisioningService = IoCContainer.Instance.Get<IProvisioningService>(productName);

                    if (provisioningService.ShouldStartProvisioning)
                    {
                        diagRes = provisioningForm.ShowDialog();

                        if (diagRes == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    this.model.AddInstance(new BL.Instance()
                    {
                        Name = askForParametersDialog.InstanceName,
                        Version = msiService.GetVersion(this.msiFullFilePath),
                        WebSiteInfo = WebSiteInfo.DefaultWebSite,
                        ProvisioningCommandLine = provisioningForm.PreconfigurationCommandLine
                    });
                }
            }
        }

        private void UiNormalUse_RemoveInstance(object sender, RemoveInstanceEventArgs e)
        {
            this.model.RemoveInstances(e.Instances);
        }

        private void UiNormalUse_UpdateInstance(object sender, UpdateInstanceEventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                ofd.Multiselect = false;
                ofd.Title = "Select Mago4 msi file";
                var res = ofd.ShowDialog(this);

                if (res != DialogResult.OK)
                {
                    return;
                }

                this.msiFullFilePath = ofd.FileName;
            }

            this.model.UpdateInstances(e.Instances);
        }

        private void tsbSettings_Click(object sender, EventArgs e)
        {
            var oldRootFolder = this.settings.RootFolder;
            using (var settingsForm = new SettingsForm(this.settings, new IisService()))
            {
                settingsForm.ShowDialog(this);
            }
            if (String.Compare(oldRootFolder, this.settings.RootFolder, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                this.model.Init();
                UpdateUI();
            }
        }

        private void tsbViewLogs_Click(object sender, EventArgs e)
        {
            var logsPath = this.settings.LogsFolder;
            if (!Directory.Exists(logsPath))
            {
                Directory.CreateDirectory(logsPath);
            }
            Process.Start(logsPath);
        }
    }
}
