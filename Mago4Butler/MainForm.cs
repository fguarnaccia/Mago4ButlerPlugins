using AutoMapper;
using log4net;
using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
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
    public partial class MainForm : Form
    {
        MapperConfiguration cmdLineMapperConfig;
        MapperConfiguration cmdLineReverseMapperConfig;
        MapperConfiguration instanceMapperConfig;
        MapperConfiguration parametersBagMapperConfig;
        MapperConfiguration parametersBagReverseMapperConfig;
        IMapper cmdLineMapper;
        IMapper cmdLineReverseMapper;
        IMapper instanceMapper;
        IMapper parametersBagMapper;
        IMapper parametersBagReverseMapper;

        SynchronizationContext syncCtx;

        UIEmpty uiEmpty;
        UIWaiting uiWaiting;
        UIWaitingMinimized uiWaitingMinimized;
        UIError uiError;
        UINormalUse uiNormalUse;

        ISettings settings;

        Model.Model model;
        MsiService msiService;
        InstallerService installerService;
        LoggerService loggerService;
        PluginService pluginService;

        string msiFullFilePath;

        public MainForm(
            Model.Model model,
            MsiService msiService,
            InstallerService installerService,
            LoggerService loggerService,
            PluginService pluginService,
            ISettings settings,
            UIEmpty uiEmpty,
            UIWaiting uiWaiting,
            UIWaitingMinimized uiWaitingMinimized,
            UIError uiError,
            UINormalUse uiNormalUse
            )
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            WinApp.ThreadException += Application_ThreadException;

            this.syncCtx = SynchronizationContext.Current;
            if (this.syncCtx == null)
            {
                this.syncCtx = new WindowsFormsSynchronizationContext();
            }

            this.settings = settings;
            this.model = model;

            this.loggerService = loggerService;
            this.pluginService = pluginService;
            this.msiService = msiService;
            this.installerService = installerService;

            InitializeComponent();

            this.uiNormalUse = uiNormalUse;
            this.uiEmpty = uiEmpty;
            this.uiWaiting = uiWaiting;
            this.uiWaitingMinimized = uiWaitingMinimized;
            this.uiError = uiError;

            WinApp.Idle += Application_Idle;
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
            WinApp.Idle -= Application_Idle;

            this.Text = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} v. {1}", this.Text, this.GetType().Assembly.GetName().Version.ToString());

            this.cmdLineMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Model.CmdLineInfo, Plugins.CmdLineInfo>();
                cfg.CreateMap<Model.Feature, Plugins.Feature>();
            });

            this.cmdLineMapper = cmdLineMapperConfig.CreateMapper();

            this.cmdLineReverseMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Plugins.CmdLineInfo, Model.CmdLineInfo>();
                cfg.CreateMap<Plugins.Feature, Model.Feature>();
            });

            this.cmdLineReverseMapper = cmdLineReverseMapperConfig.CreateMapper();

            this.instanceMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Model.Instance, Plugins.Instance>());
            this.instanceMapper = instanceMapperConfig.CreateMapper();

            this.parametersBagMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<AskForParametersBag, Plugins.AskForParametersBag>());
            this.parametersBagMapper = parametersBagMapperConfig.CreateMapper();

            this.parametersBagReverseMapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Plugins.AskForParametersBag, AskForParametersBag>());
            this.parametersBagReverseMapper = parametersBagReverseMapperConfig.CreateMapper();

            this.model.InstanceAdded += (s, iea) => this.installerService.Install(this.msiFullFilePath, iea.Instance);
            this.model.InstanceUpdated += (s, iea) => this.installerService.Update(this.msiFullFilePath, iea.Instance);
            this.model.InstanceRemoved += (s, iea) => this.installerService.Uninstall(iea.Instance);

            this.uiEmpty.SelectMsiToInstall += Install;
            this.uiError.Back += UiError_Back;
            this.uiNormalUse.InstallNewInstance += Install;
            this.uiNormalUse.UpdateInstance += UiNormalUse_UpdateInstance;
            this.uiNormalUse.RemoveInstance += UiNormalUse_RemoveInstance;
            this.uiWaiting.Back += UiWaiting_Back;
            this.uiWaitingMinimized.WindowClose += UiWaitingMinimized_WindowClose;
            this.uiWaitingMinimized.AttachToMainUI(this);
            this.uiWaitingMinimized.AttachToMainUiWaiting(this.uiWaiting);


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

            OnApplicationStarted();
        }

        private void OnApplicationStarted()
        {
            foreach (var plugin in this.pluginService.Plugins)
            {
                if (plugin != null)
                {
                    try
                    {
                        plugin.OnApplicationStarted();
                    }
                    catch (Exception exc)
                    {
                        this.loggerService.LogError("Error notifying plugins about the application started event.", exc);
                    }
                }
            }
        }

        private void UiWaitingMinimized_WindowClose(object sender, EventArgs e)
        {
            this.uiWaitingMinimized.Visible = false;
            ShowUI(this.uiWaiting);
        }

        private void UiWaiting_Back(object sender, EventArgs e)
        {
            if (!this.uiWaitingMinimized.Visible)
            {
                if (!this.uiWaitingMinimized.IsHandleCreated)
                {
                    this.uiWaitingMinimized.Show(this);
                }
                else
                {
                    this.uiWaitingMinimized.Visible = true;
                }
            }

            var ui = (this.model.Instances.Count() == 0) ? this.uiEmpty as UserControl : this.uiNormalUse as UserControl;
            ShowUI(ui);
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

            foreach (var plugin in this.pluginService.Plugins)
            {
                var pluginCmdLineInfo = cmdLineMapper.Map<Plugins.CmdLineInfo>(e.CmdLineInfo);
                try
                {
                    plugin.OnUpdating(pluginCmdLineInfo);
                    e.CmdLineInfo = cmdLineReverseMapper.Map(pluginCmdLineInfo, typeof(Plugins.CmdLineInfo), typeof(Model.CmdLineInfo)) as Model.CmdLineInfo;
                }
                catch (Exception exc)
                {
                    this.loggerService.LogError("Command line paremeters request failed, skipping plugin " + plugin.GetName(), exc);
                }
            }
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

            var instances = new List<Plugins.Instance>(e.Instances.Count);
            foreach (var instance in e.Instances)
            {
                instances.Add(instanceMapper.Map<Plugins.Instance>(instance));
            }
            foreach (var plugin in this.pluginService.Plugins)
            {
                try
                {
                    plugin.OnRemoving(instances.ToArray());
                }
                catch (Exception exc)
                {
                    this.loggerService.LogError("'OnRemoving' event failed, skipping plugin " + plugin.GetName(), exc);
                }
            }
        }

        private void InstallerService_Installed(object sender, InstallInstanceEventArgs e)
        {
            this.loggerService.LogInfo("Database configuration...");
            this.uiWaiting.SetProgressText("Database configuration...");

            this.uiWaiting.AddDetailsText("Database configuration...");
            try
            {
                var productName = msiService.GetProductName(this.msiFullFilePath).ToLowerInvariant();
                var provisioningService = IoCContainer.Instance.GetProvisioningService(productName);

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

            foreach (var plugin in this.pluginService.Plugins)
            {
                var pluginCmdLineInfo = cmdLineMapper.Map<Plugins.CmdLineInfo>(e.CmdLineInfo);
                try
                {
                    plugin.OnInstalling(pluginCmdLineInfo);
                    e.CmdLineInfo = cmdLineReverseMapper.Map(pluginCmdLineInfo, typeof(Plugins.CmdLineInfo), typeof(Model.CmdLineInfo)) as Model.CmdLineInfo;
                }
                catch (Exception exc)
                {
                    this.loggerService.LogError("Command line paremeters request failed, skipping plugin " + plugin.GetName(), exc);
                }
            }
        }

        private void InstallerService_Stopping(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("Stopping installer service");
        }

        private void InstallerService_Stopped(object sender, EventArgs e)
        {
            this.syncCtx.Post((_) =>
            {
                this.loggerService.LogInfo("Installer service stopped");
                this.loggerService.LogInfo("--------------------------------------------------------------------------------");

                if (this.uiWaitingMinimized.Visible)
                {
                    this.loggerService.LogInfo("uiWaitingMinimized is visible, I'm going to hide it...");
                    this.uiWaitingMinimized.Visible = false;
                    this.loggerService.LogInfo("uiWaitingMinimized now should be no more visible.");
                }
                else
                {
                    this.loggerService.LogInfo("uiWaitingMinimized was not visible, nothing to do.");
                    var ui = (this.model.Instances.Count() == 0) ? this.uiEmpty as UserControl : this.uiNormalUse as UserControl;
                    ShowUI(ui);
                }

                EnableDisableToolStripItem(this.tsbSettings, true);

                foreach (var plugin in this.pluginService.Plugins)
                {
                    if (plugin != null)
                    {
                        try
                        {
                            plugin.OnInstallerServiceStopped();
                        }
                        catch (Exception exc)
                        {
                            this.loggerService.LogError("Error notifying plugins about the installed service stopped event.", exc);
                        }
                    }
                }
            }, null);
        }

        private void InstallerService_Starting(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("Starting installer service");
        }

        private void InstallerService_Started(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("--------------------------------------------------------------------------------");
            this.loggerService.LogInfo("Installer service started");

            this.syncCtx.Post((obj) => uiWaiting.ClearDetails(), null);

            ShowUI(uiWaiting);
            EnableDisableToolStripItem(this.tsbSettings, false);

            foreach (var plugin in this.pluginService.Plugins)
            {
                if (plugin != null)
                {
                    try
                    {
                        plugin.OnInstallerServiceStarted();
                    }
                    catch (Exception exc)
                    {
                        this.loggerService.LogError("Error notifing plugins about the installed service started event.", exc);
                    }
                }
            }
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
            using (var aboutForm = new AboutForm(pluginService))
            {
                aboutForm.ShowDialog(this);
            }
        }

        private void Install(object sender, InstallInstanceEventArgs e)
        {
            var rootFolder = new DirectoryInfo(this.settings.RootFolder);
            if (!rootFolder.Exists)
            {
                rootFolder.Create();
            }

            var instanceName = string.Empty;
            var provisioningCommandLine = string.Empty;
            this.msiFullFilePath = null;

            var bag = new AskForParametersBag() { InstanceName = instanceName, MsiFullFilePath = this.msiFullFilePath };
            OnAskForParametersForInstall(bag);

            if (bag.Cancel)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(bag.InstanceName) || string.IsNullOrWhiteSpace(bag.MsiFullFilePath))
            {
                this.msiFullFilePath = this.msiService.CalculateMsiFullFilePath();
                using (var askForParametersDialog = new AskForParametersForm(this.model, this.settings))
                {
                    var diagRes = askForParametersDialog.ShowDialog(this);

                    if (diagRes != DialogResult.OK)
                    {
                        return;
                    }
                    instanceName = askForParametersDialog.InstanceName;

                    var productName = msiService.GetProductName(this.msiFullFilePath).ToLowerInvariant();
                    IProvisioningService provisioningService = IoCContainer.Instance.GetProvisioningService(productName);

                    if (provisioningService.ShouldStartProvisioning)
                    {
                        using (var provisioningForm = new ProvisioningFormLITE(instanceName: instanceName, preconfigurationMode: true, loadDataFromFile: false))
                        {
                            var provisioningSuggestions = new ProvisioningData()
                            {
                                CompanyDbName = string.Format("{0}_DB", instanceName),
                                DMSDbName = string.Format("{0}_DBDMS", instanceName),
                                SystemDbName = string.Format("{0}_SystemDB", instanceName),
                                AdminLoginName = string.Format("{0}_admin", instanceName),
                                UserLoginName = string.Format("{0}_user", instanceName)
                            };

                            provisioningForm.ProvisioningData = provisioningSuggestions;

                            diagRes = provisioningForm.ShowDialog(this);

                            if (diagRes == DialogResult.Cancel)
                            {
                                return;
                            }
                            provisioningCommandLine = provisioningForm.PreconfigurationCommandLine;
                        }
                    }
                }
            }
            else
            {
                this.msiFullFilePath = bag.MsiFullFilePath;
                instanceName = bag.InstanceName;
            }

            this.model.AddInstance(new Model.Instance()
            {
                Name = instanceName,
                Version = msiService.GetVersion(this.msiFullFilePath),
                WebSiteInfo = WebSiteInfo.DefaultWebSite,
                ProvisioningCommandLine = provisioningCommandLine
            });
        }

        private void OnAskForParametersForInstall(AskForParametersBag bag)
        {
            foreach (var plugin in this.pluginService.Plugins)
            {
                var mappedBag = parametersBagMapper.Map<Plugins.AskForParametersBag>(bag);
                if (plugin != null)
                {
                    try
                    {
                        plugin.OnAskForParametersForInstall(mappedBag);
                        bag = parametersBagReverseMapper.Map(mappedBag, bag, typeof(Plugins.AskForParametersBag), typeof(AskForParametersBag)) as AskForParametersBag;
                    }
                    catch (Exception exc)
                    {
                        this.loggerService.LogError("Error asking the plugin " + plugin.GetName() + " for parameters on installation.", exc);
                    }
                }
            }
        }

        private void OnAskForParametersForUpdate(AskForParametersBag bag)
        {
            foreach (var plugin in this.pluginService.Plugins)
            {
                var mappedBag = parametersBagMapper.Map<Plugins.AskForParametersBag>(bag);
                if (plugin != null)
                {
                    try
                    {
                        plugin.OnAskForParametersForUpdate(mappedBag);
                        bag = parametersBagReverseMapper.Map(mappedBag, bag, typeof(Plugins.AskForParametersBag), typeof(AskForParametersBag)) as AskForParametersBag;
                    }
                    catch (Exception exc)
                    {
                        this.loggerService.LogError("Error asking the plugin " + plugin.GetName() + " for parameters on update.", exc);
                    }
                }
            }
        }

        private void UiNormalUse_RemoveInstance(object sender, RemoveInstanceEventArgs e)
        {
            this.model.RemoveInstances(e.Instances);
        }

        private void UiNormalUse_UpdateInstance(object sender, UpdateInstanceEventArgs e)
        {
            this.msiFullFilePath = null;
            var bag = new AskForParametersBag() { MsiFullFilePath = this.msiFullFilePath };
            OnAskForParametersForUpdate(bag);

            if (!string.IsNullOrWhiteSpace(bag.MsiFullFilePath))
            {
                this.msiFullFilePath = bag.MsiFullFilePath;
            }
            else
            {
                this.msiFullFilePath = this.msiService.CalculateMsiFullFilePath();
            }

            var version = this.msiService.GetVersion(this.msiFullFilePath);
            foreach (var instance in e.Instances)
            {
                if (instance.Version >= version)
                {
                    this.loggerService.LogError(instance.Name + " version is " + instance.Version + ", instance not to be updated");
                }
            }
            this.model.UpdateInstances(e.Instances, version);
        }

        private void tsbSettings_Click(object sender, EventArgs e)
        {
            var oldRootFolder = this.settings.RootFolder;

            var iisService = IoCContainer.Instance.Get<IisService>();
            using (var settingsForm = new SettingsForm(this.settings, iisService))
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
