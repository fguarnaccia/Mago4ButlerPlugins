using AutoMapper;
using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinApp = System.Windows.Forms.Application;

namespace Microarea.Mago4Butler
{
    public class UIMediator : IUIMediator
    {
        public event EventHandler<JobEventArgs> JobNotification;
        public event EventHandler<JobEventArgs> ParametersForInstallNeeded;
        public event EventHandler<ProvisioningEventArgs> ProvisioningNeeded;

        protected virtual void OnJobNotification(JobEventArgs e)
        {
            var handler = JobNotification;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnAskForParametersForInstall(JobEventArgs e)
        {
            var handler = ParametersForInstallNeeded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnProvisioningNeeded(ProvisioningEventArgs e)
        {
            var handler = ProvisioningNeeded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

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

        ISettings settings;

        Model.Model model;
        MsiService msiService;
        InstallerService installerService;
        LoggerService loggerService;
        PluginService pluginService;

        string msiFullFilePath;

        public bool CanClose { get { return !this.installerService.IsRunning; } }
        public bool ShouldUserWait { get { return this.installerService.IsRunning; } }
        public bool ShouldShowEmptyUI { get { return this.model.Instances.Count() == 0; } }

        public UIMediator(
            Model.Model model,
            MsiService msiService,
            InstallerService installerService,
            LoggerService loggerService,
            PluginService pluginService,
            ISettings settings
            )
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            WinApp.ThreadException += Application_ThreadException;

            this.settings = settings;
            this.model = model;

            this.loggerService = loggerService;
            this.pluginService = pluginService;
            this.msiService = msiService;
            this.installerService = installerService;

            WinApp.Idle += Application_Idle;
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
            this.OnJobNotification(new JobEventArgs() { Error = e });
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            WinApp.Idle -= Application_Idle;

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

        private void InstallerService_Error(object sender, InstallerServiceErrorEventArgs e)
        {
            ManageException(e.Error);
            OnJobNotification(new Mago4Butler.JobEventArgs() { Error = e.Error, NotificationType = NotificationTypes.Error });
        }

        private void InstallerService_Notification(object sender, NotificationEventArgs e)
        {
            this.loggerService.LogInfo(e.Message);
            OnJobNotification(new Mago4Butler.JobEventArgs() { Notification = e.Message, NotificationType = NotificationTypes.Notification });
        }

        private void InstallerService_Updated(object sender, UpdateInstanceEventArgs e)
        {
            string message = e.Instances[0].Name + " successfully updated";
            this.loggerService.LogInfo(message);
            OnJobNotification(new Mago4Butler.JobEventArgs() { Progress = message, NotificationType = NotificationTypes.Progress });
        }

        private void InstallerService_Updating(object sender, UpdateInstanceEventArgs e)
        {
            string message = "Updating " + e.Instances[0].Name + " ...";
            this.loggerService.LogInfo(message);
            OnJobNotification(new Mago4Butler.JobEventArgs() { Progress = message, NotificationType = NotificationTypes.Progress });

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
            string message = e.Instances[0].Name + " successfully removed";
            this.loggerService.LogInfo(message);
            OnJobNotification(new Mago4Butler.JobEventArgs() { Progress = message, NotificationType = NotificationTypes.Progress });
        }

        private void InstallerService_Removing(object sender, RemoveInstanceEventArgs e)
        {
            string message = "Removing " + e.Instances[0].Name + " ...";
            this.loggerService.LogInfo(message);
            OnJobNotification(new Mago4Butler.JobEventArgs() { Progress = message, NotificationType = NotificationTypes.Progress });

            var instances = new List<Plugins.Instance>(e.Instances.Length);
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
            string message = "Database configuration...";
            this.loggerService.LogInfo(message);
            OnJobNotification(new Mago4Butler.JobEventArgs() { Progress = message, Notification = message, NotificationType = NotificationTypes.Progress | NotificationTypes.Notification });

            try
            {
                var productName = msiService.GetProductName(this.msiFullFilePath).ToLowerInvariant();
                var provisioningService = IoCContainer.Instance.GetProvisioningService(productName);

                if (provisioningService.ShouldStartProvisioning)
                {
                    provisioningService.StartProvisioning(e.Instance);
                    
                    this.loggerService.LogInfo(message);
                    OnJobNotification(new Mago4Butler.JobEventArgs() { Notification = message, NotificationType = NotificationTypes.Notification });
                }
                else
                {
                    message = String.Format(System.Globalization.CultureInfo.InvariantCulture, "No database provisioning for {0}, database configuration skipped", e.Instance.Name);
                    this.loggerService.LogInfo(message);
                    OnJobNotification(new Mago4Butler.JobEventArgs() { Notification = message, NotificationType = NotificationTypes.Notification });
                }
            }
            catch (Exception exc)
            {
                message = "Database configurator returned an error";
                this.loggerService.LogError(message, exc);
                OnJobNotification(new Mago4Butler.JobEventArgs() { Notification = message, NotificationType = NotificationTypes.Notification });
            }
        }

        private void InstallerService_Installing(object sender, InstallInstanceEventArgs e)
        {
            string message = "Installing " + e.Instance.Name + " ...";
            this.loggerService.LogInfo(message);
            OnJobNotification(new Mago4Butler.JobEventArgs() { Progress = message, NotificationType = NotificationTypes.Progress });

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
            this.loggerService.LogInfo("Installer service stopped");
            this.loggerService.LogInfo("--------------------------------------------------------------------------------");

            this.OnJobNotification(new JobEventArgs() { NotificationType = NotificationTypes.JobEnded });

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
        }

        private void InstallerService_Starting(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("Starting installer service");
        }

        private void InstallerService_Started(object sender, EventArgs e)
        {
            this.loggerService.LogInfo("--------------------------------------------------------------------------------");
            this.loggerService.LogInfo("Installer service started");

            this.OnJobNotification(new JobEventArgs() { NotificationType = NotificationTypes.JobStarted });

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

        private void OnAskForInstallParametersToPlugins(AskForParametersBag bag)
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

        private void OnAskForUpdateParametersToPlugins(AskForParametersBag bag)
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

        public void Init()
        {
            this.model.Init();
        }

        public void RemoveInstances(Instance[] instances)
        {
            this.model.RemoveInstances(instances);
        }

        public void UpdateInstances(Instance[] instances)
        {
            this.msiFullFilePath = null;
            var bag = new AskForParametersBag() { MsiFullFilePath = this.msiFullFilePath };
            OnAskForUpdateParametersToPlugins(bag);

            if (!string.IsNullOrWhiteSpace(bag.MsiFullFilePath))
            {
                this.msiFullFilePath = bag.MsiFullFilePath;
            }
            else
            {
                this.msiFullFilePath = this.msiService.CalculateMsiFullFilePath();
            }

            var version = this.msiService.GetVersion(this.msiFullFilePath);
            foreach (var instance in instances)
            {
                if (instance.Version >= version)
                {
                    this.loggerService.LogError(instance.Name + " version is " + instance.Version + ", instance not to be updated");
                }
            }
            this.model.UpdateInstances(instances, version);
        }

        public void InstallInstance()
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
            OnAskForInstallParametersToPlugins(bag);

            if (bag.Cancel)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(bag.InstanceName) || string.IsNullOrWhiteSpace(bag.MsiFullFilePath))
            {
                this.msiFullFilePath = this.msiService.CalculateMsiFullFilePath();

                var args = new Mago4Butler.JobEventArgs() { Bag = bag };
                this.OnAskForParametersForInstall(args);
                if (args.Bag.Cancel)
                {
                    return;
                }
                instanceName = args.Bag.InstanceName;

                var productName = msiService.GetProductName(this.msiFullFilePath).ToLowerInvariant();
                IProvisioningService provisioningService = IoCContainer.Instance.GetProvisioningService(productName);
                if (provisioningService.ShouldStartProvisioning)
                {
                    var eventArgs = new ProvisioningEventArgs() { InstanceName = instanceName };
                    this.OnProvisioningNeeded(eventArgs);

                    if (eventArgs.Cancel)
                    {
                        return;
                    }
                    provisioningCommandLine = eventArgs.ProvisioningCommandLine;
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
    }
}
