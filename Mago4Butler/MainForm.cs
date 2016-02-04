using Microarea.Mago4Butler.BL;
using Microarea.Tools.ProvisioningConfigurator.ProvisioningConfigurator;
using System;
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
        InstallerService instanceService;
        ProvisioningService provisioningService;

        string msiFullFilePath;

        public MainForm(
            Model model,
            MsiService msiService,
            InstallerService instanceService,
            ProvisioningService provisioningService,
            ISettings settings,
            UIEmpty uiEmpty,
            UIWaiting uiWaiting,
            UIError uiError,
            UINormalUse uiNormalUse
            )
        {
            this.syncCtx = SynchronizationContext.Current;
            if (this.syncCtx == null)
            {
                this.syncCtx = new WindowsFormsSynchronizationContext();
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;

            this.model = model;

            this.msiService = msiService;
            this.instanceService = instanceService;
            this.provisioningService = provisioningService;

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

            if (this.instanceService.IsRunning)
            {
                e.Cancel = true;
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= Application_Idle;

            this.Text = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} v. {1}", this.Text, this.GetType().Assembly.GetName().Version.ToString());

            this.model.InstanceAdded += (s, iea) => this.instanceService.Install(this.msiFullFilePath, iea.Instance);
            this.model.InstanceUpdated += (s, iea) => this.instanceService.Update(this.msiFullFilePath, iea.Instance);
            this.model.InstanceRemoved += (s, iea) => this.instanceService.Uninstall(iea.Instance);

            this.uiEmpty.SelectMsiToInstall += Install;
            this.uiError.Back += UiError_Back;
            this.uiNormalUse.InstallNewInstance += Install;
            this.uiNormalUse.UpdateInstance += UiNormalUse_UpdateInstance;
            this.uiNormalUse.RemoveInstance += UiNormalUse_RemoveInstance;

            this.instanceService.Started += InstanceService_Started;
            this.instanceService.Starting += InstanceService_Starting;
            this.instanceService.Stopped += InstanceService_Stopped;
            this.instanceService.Stopping += InstanceService_Stopping;

            this.instanceService.Installing += InstanceService_Installing;
            this.instanceService.Installed += InstanceService_Installed;
            this.instanceService.Removing += InstanceService_Removing;
            this.instanceService.Removed += InstanceService_Removed;
            this.instanceService.Updating += InstanceService_Updating;
            this.instanceService.Updated += InstanceService_Updated;

            this.instanceService.Notification += InstanceService_Notification;

            Thread.Sleep(1000);
            UpdateUI();
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

        private void InstanceService_Notification(object sender, NotificationEventArgs e)
        {
            this.uiWaiting.AddDetailsText(e.Message);
        }

        private void InstanceService_Updated(object sender, UpdateInstanceEventArgs e)
        {
            this.uiWaiting.SetProgressText(e.Instances[0].Name + " successfully updated");
        }

        private void InstanceService_Updating(object sender, UpdateInstanceEventArgs e)
        {
            this.uiWaiting.SetProgressText("Updating " + e.Instances[0].Name + " ...");
        }

        private void InstanceService_Removed(object sender, RemoveInstanceEventArgs e)
        {
            this.uiWaiting.SetProgressText(e.Instances[0].Name + " successfully removed");
        }

        private void InstanceService_Removing(object sender, RemoveInstanceEventArgs e)
        {
            this.uiWaiting.SetProgressText("Removing " + e.Instances[0].Name + " ...");
        }

        private void InstanceService_Installed(object sender, InstallInstanceEventArgs e)
        {
            this.uiWaiting.SetProgressText("Database configuration...");

            this.uiWaiting.AddDetailsText("Database configuration...");
            try
            {
                this.provisioningService.StartProvisioning(e.Instance);
                this.uiWaiting.AddDetailsText("Database configuration ended");
            }
            catch (Exception exc)
            {
                this.uiWaiting.AddDetailsText("Database configurator returned the followinf error: " + exc.Message);
            }
        }

        private void InstanceService_Installing(object sender, InstallInstanceEventArgs e)
        {
            this.uiWaiting.SetProgressText("Installing " + e.Instance.Name + " ...");
        }

        private void InstanceService_Stopping(object sender, EventArgs e)
        {

        }

        private void InstanceService_Stopped(object sender, EventArgs e)
        {
            var ui = (this.model.Instances.Count() == 0) ? this.uiEmpty as UserControl : this.uiNormalUse as UserControl;
            ShowUI(ui);
            EnableDisableToolStripItem(this.tsbSettings, true);
        }

        private void InstanceService_Starting(object sender, EventArgs e)
        {
            
        }

        private void InstanceService_Started(object sender, EventArgs e)
        {
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
            this.uiError.SetErrorMessage(e.Message);
            ShowUI(this.uiError);
        }

        private void UiError_Back(object sender, EventArgs e)
        {
            if (this.instanceService.IsRunning)
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
            using (var askForParametersDialog = new AskForParametersForm(this.model))
            {
                var diagRes = askForParametersDialog.ShowDialog();

                if (diagRes != DialogResult.OK)
                {
                    return;
                }

                using (var provisioningForm = new ProvisioningForm(instanceName: askForParametersDialog.InstanceName, preconfigurationMode: true, loadDataFromFile: false))
                {
                    diagRes = provisioningForm.ShowDialog();

                    if (diagRes == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.msiFullFilePath = askForParametersDialog.MsiFullPath;

                    this.model.AddInstance(new Instance()
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
            var logsPath = Path.Combine(this.settings.RootFolder, "Logs");
            if (!Directory.Exists(logsPath))
            {
                Directory.CreateDirectory(logsPath);
            }
            Process.Start(logsPath);
        }
    }
}
