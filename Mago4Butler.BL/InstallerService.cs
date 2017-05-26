using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using Microarea.TaskBuilderNet.Core.NameSolver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    internal class InstallerService : ILogger
    {
        readonly object lockTicket = new object();

        public event EventHandler<EventArgs> Starting;
        public event EventHandler<EventArgs> Started;
        public event EventHandler<EventArgs> Stopping;
        public event EventHandler<EventArgs> Stopped;
        public event EventHandler<InstallInstanceEventArgs> Installing;
        public event EventHandler<InstallInstanceEventArgs> Installed;
        public event EventHandler<UpdateInstanceEventArgs> Updating;
        public event EventHandler<UpdateInstanceEventArgs> Updated;
        public event EventHandler<RemoveInstanceEventArgs> Removing;
        public event EventHandler<RemoveInstanceEventArgs> Removed;
        public event EventHandler<InstallerServiceErrorEventArgs> Error;

        public event EventHandler<NotificationEventArgs> Notification;

        WcfService wcfService;
        MsiService msiService;
        FileSystemService fileSystemService;
        ICompanyDBUpdateService companyDBUpdateService;
        ISalesModulesConfiguratorService salesModulesConfiguratorService;
        IisService iisService;

        ISettings settings;
        Queue<Request> requests = new Queue<Request>();

        Thread workingThread;

        public bool IsRunning { get { return this.workingThread != null; } }

        public void Join()
        {
            if (this.workingThread != null)
            {
                this.workingThread.Join();
            }
        }

        public InstallerService(
            ISettings settings,
            MsiService msiService,
            ICompanyDBUpdateService companyDBUpdateService,
            FileSystemService fileSystemService,
            WcfService wcfService,
            ISalesModulesConfiguratorService salesModulesConfiguratorService,
            IisService iisService
            )
        {
            this.settings = settings;
            this.msiService = msiService;
            this.companyDBUpdateService = companyDBUpdateService;
            this.fileSystemService = fileSystemService;
            this.wcfService = wcfService;
            this.salesModulesConfiguratorService = salesModulesConfiguratorService;
            this.iisService = iisService;
        }

        protected virtual void OnStarting()
        {
            var handler = Starting;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStarted()
        {
            var handler = Started;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStopping()
        {
            var handler = Stopping;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStopped()
        {
            var handler = Stopped;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnInstalling(InstallInstanceEventArgs e)
        {
            var handler = Installing;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnInstalled(InstallInstanceEventArgs e)
        {
            var handler = Installed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnUpdating(UpdateInstanceEventArgs e)
        {
            var handler = Updating;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnUpdated(UpdateInstanceEventArgs e)
        {
            var handler = Updated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRemoving(RemoveInstanceEventArgs e)
        {
            var handler = Removing;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRemoved(RemoveInstanceEventArgs e)
        {
            var handler = Removed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnNotification(NotificationEventArgs e)
        {
            var handler = Notification;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnError(InstallerServiceErrorEventArgs e)
        {
            var handler = Error;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        void Enqueue(Request request)
        {
            lock (this.lockTicket)
            {
                this.requests.Enqueue(request);
            }
        }
        Request Dequeue()
        {
            lock (this.lockTicket)
            {
                if (this.requests.Count > 0)
                {
                    return this.requests.Dequeue();
                }

                return null;
            }
        }

        public void Install(string msiFullFilePath, Instance instance)
        {
            this.Enqueue(new Request()
            {
                Instance = instance,
                MsiPath = msiFullFilePath,
                RequestType = RequestType.Install,
                RootFolder = this.settings.RootFolder
            });
            StartService();
        }

        public void Uninstall(IEnumerable<Instance> instances)
        {
            if (instances.Count() == 0)
            {
                return;
            }
            foreach (var instance in instances)
            {
                this.Uninstall(instance);
            }
        }

        public void Uninstall(Instance instance)
        {
            this.Enqueue(new Request()
            {
                Instance = instance,
                MsiPath = String.Empty,
                RequestType = RequestType.Remove,
                RootFolder = this.settings.RootFolder
            });
            StartService();
        }

        public void Update(string msiFullFilePath, IEnumerable<Instance> instances)
        {
            if (instances.Count() == 0)
            {
                return;
            }
            foreach (var instance in instances)
            {
                this.Update(msiFullFilePath, instance);
            }
        }

        public void Update(string msiFullFilePath, Instance instance)
        {
            this.Enqueue(new Request()
            {
                Instance = instance,
                MsiPath = msiFullFilePath,
                RequestType = RequestType.Update,
                RootFolder = this.settings.RootFolder
            });
            StartService();
        }

        private void StartService()
        {
            lock (this.lockTicket)
            {
                if (this.workingThread != null)
                {
                    return;
                }

                this.OnStarting();
                ThreadPool.QueueUserWorkItem(Worker);
            }
        }
        private void Worker(object state)
        {
            lock (this.lockTicket)
            {
                if (this.workingThread != null)
                {
                    return;
                }
                this.workingThread = Thread.CurrentThread;
            }
            this.OnStarted();

            var currentRequest = this.Dequeue();

            while (currentRequest != null)
            {
                switch (currentRequest.RequestType)
                {
                    case RequestType.Install:
                        {
                            var cmdLineInfo = new CmdLineInfo()
                            {
                                SkipClickOnceDeployer = true,
                                NoShortcuts = true,
                                NoShares = true,
                                NoEnvVar = true,
                                NoEveryone = true,
                                ClassicApplicationPoolPipeline = true,
                                ProxySettingsSet = this.settings.UseProxy,
                                ProxyUrl = this.settings.ProxyServerUrl,
                                ProxyPort = this.settings.ProxyServerPort,
                                ProxyUserSet = this.settings.UseCredentials,
                                ProxyDomain = this.settings.DomainName,
                                ProxyUsername = this.settings.Username,
                                ProxyPassword = this.settings.Password,
                                Features = this.msiService.GetFeatureNames(currentRequest.MsiPath)
                            };
                            var args = new InstallInstanceEventArgs() { Instance = currentRequest.Instance, CmdLineInfo = cmdLineInfo };
                            try
                            {
                                this.OnInstalling(args);
                            }
                            catch (Exception exc)
                            {
                                this.LogError("Command line paremeters request failed, skipping plugin", exc);
                            }
                            this.Install(currentRequest, args.CmdLineInfo);

                            this.OnInstalled(new InstallInstanceEventArgs() { Instance = currentRequest.Instance });

                            break;
                        }
                    case RequestType.Update:
                        {
                            var cmdLineInfo = new CmdLineInfo()
                            {
                                SkipClickOnceDeployer = true,
                                NoShortcuts = true,
                                NoShares = true,
                                NoEnvVar = true,
                                NoEveryone = true,
                                ClassicApplicationPoolPipeline = true,
                                ProxySettingsSet = this.settings.UseProxy,
                                ProxyUrl = this.settings.ProxyServerUrl,
                                ProxyPort = this.settings.ProxyServerPort,
                                ProxyUserSet = this.settings.UseCredentials,
                                ProxyDomain = this.settings.DomainName,
                                ProxyUsername = this.settings.Username,
                                ProxyPassword = this.settings.Password,
                                Features = this.msiService.GetFeatureNames(currentRequest.MsiPath)
                            };
                            var args = new UpdateInstanceEventArgs() { Instances = new Instance[] { currentRequest.Instance }, CmdLineInfo = cmdLineInfo };
                            try
                            {
                                this.OnUpdating(args);

                            }
                            catch (Exception exc)
                            {
                                this.LogError("Command line paremeters request failed, skipping plugin", exc);
                            }
                            this.Update(currentRequest, args.CmdLineInfo);

                            this.OnUpdated(new UpdateInstanceEventArgs() { Instances = new Instance[] { currentRequest.Instance } });

                            break;
                        }
                    case RequestType.Remove:
                        {
                            this.OnRemoving(new RemoveInstanceEventArgs() { Instances = new Instance[] { currentRequest.Instance } });
                            this.Remove(currentRequest);

                            this.OnRemoved(new RemoveInstanceEventArgs() { Instances = new Instance[] { currentRequest.Instance } });
                            break;
                        }
                    default:
                        break;
                }

                currentRequest = this.Dequeue();
            }

            this.OnStopping();
            lock (this.lockTicket)
            {
                this.workingThread = null;
            }
            this.OnStopped();
        }

        private void Remove(Request currentRequest)
        {
            OnNotification(new NotificationEventArgs() { Message = "Unregistering wcf namespaces..." });
            this.wcfService.UnregisterWcf(currentRequest.Instance.WcfStartPort, currentRequest.Instance.Name);
            OnNotification(new NotificationEventArgs() { Message = "Wcf namespaces unregistered" });

            OnNotification(new NotificationEventArgs() { Message = "Removing virtual folders..." });
            //Rimuovere prima le virtual folder e le application, poi gli application pool.
            //Un application pool a cui sono collegate ancora applicazioni non puo` essere eliminato
            this.iisService.RemoveVirtualFoldersAndApplications(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Virtual folders removed" });

            OnNotification(new NotificationEventArgs() { Message = "Removing application pools..." });
            this.iisService.RemoveApplicationPools(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Application pools removed" });

            OnNotification(new NotificationEventArgs() { Message = "Removing all files..." });
            try
            {
                this.fileSystemService.RemoveAllFiles(currentRequest.Instance);
                OnNotification(new NotificationEventArgs() { Message = "All files removed" });
            }
            catch (Exception exc)
            {
                this.LogError("Error removing " + currentRequest.Instance.Name, exc);
                OnNotification(new NotificationEventArgs() { Message = "Error removing files: " + exc.Message });
                OnError(new InstallerServiceErrorEventArgs()
                {
                    Message = "Error removing files for " + currentRequest.Instance.Name,
                    Error = exc
                });
            }
        }

        private void Update(Request currentRequest, CmdLineInfo cmdLineInfo)
        {
            try
            {
                OnNotification(new NotificationEventArgs() { Message = "Removing files before updating..." });
                this.Remove(currentRequest);
                OnNotification(new NotificationEventArgs() { Message = "Files successfully removed." });
            }
            catch (Exception exc)
            {
                this.LogError("Error removing before updating " + currentRequest.Instance.Name, exc);
                OnNotification(new NotificationEventArgs() { Message = "Error removing before updating: " + exc.Message });
            }
            try
            {
                OnNotification(new NotificationEventArgs() { Message = "Launching msi..." });
                this.msiService.UpdateMsi(currentRequest, cmdLineInfo);
                OnNotification(new NotificationEventArgs() { Message = "Msi execution successfully terminated." });
            }
            catch (Exception exc)
            {
                OnError(new InstallerServiceErrorEventArgs()
                {
                    Message = "Error updating " + currentRequest.Instance.Name,
                    Error = exc
                });
                return;
            }

            OnNotification(new NotificationEventArgs() { Message = "Configuring the application..." });
            this.salesModulesConfiguratorService.ConfigureSalesModules(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Application configured" });

            OnNotification(new NotificationEventArgs() { Message = "Restarting Login Manager..." });
            this.iisService.RestartLoginManager(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Login Manager restarted" });

            OnNotification(new NotificationEventArgs() { Message = "Updating company database..." });
            this.companyDBUpdateService.UpdateCompanyDB(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Company database successfully updated" });
        }

        private void Install(Request currentRequest, CmdLineInfo cmdLineInfo)
        {
            try
            {
                OnNotification(new NotificationEventArgs() { Message = "Launching msi..." });
                this.msiService.InstallMsi(currentRequest, cmdLineInfo);
                OnNotification(new NotificationEventArgs() { Message = "Msi execution successfully terminated..." });
            }
            catch (Exception exc)
            {
                OnError(new InstallerServiceErrorEventArgs()
                {
                    Message = "Error installing " + currentRequest.Instance.Name,
                    Error = exc
                });
                return;
            }

            OnNotification(new NotificationEventArgs() { Message = "Configuring the application..." });
            this.SaveServerConnectionConfig(currentRequest);
            this.salesModulesConfiguratorService.ConfigureSalesModules(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Application configured" });

            OnNotification(new NotificationEventArgs() { Message = "Restarting Login Manager..." });
            this.iisService.RestartLoginManager(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Login Manager restarted" });

            OnNotification(new NotificationEventArgs() { Message = "Creating settings.config file with wcf starting port..." });
            this.wcfService.CreateSettingsConfigFile(currentRequest.Instance.Name, currentRequest.Instance.WcfStartPort);
            OnNotification(new NotificationEventArgs() { Message = "settings.config file created" });

            OnNotification(new NotificationEventArgs() { Message = "Registering wcf namespaces..." });
            this.wcfService.RegisterWcf(currentRequest.Instance.WcfStartPort, currentRequest.Instance.Name);
            OnNotification(new NotificationEventArgs() { Message = "Wcf namespaces registered" });
        }

        private void SaveServerConnectionConfig(Request currentRequest)
        {
            var customFolderPath = Path.Combine(currentRequest.RootFolder, currentRequest.Instance.Name, "Custom");
            var serverConnectionConfigFilePath = Path.Combine(customFolderPath, "ServerConnection.config");

            var customDirInfo = new DirectoryInfo(customFolderPath);
            if (!customDirInfo.Exists)
            {
                customDirInfo.Create();
            }

            var serverConnectionConfigFileInfo = new FileInfo(serverConnectionConfigFilePath);
            bool serverConnectionConfigUpdated = false;
            if (serverConnectionConfigFileInfo.Exists)
            {
                try
                {
                    var serverConnectionConfig = new ServerConnectionInfo();
                    serverConnectionConfig.Parse(serverConnectionConfigFilePath);
                    serverConnectionConfig.PreferredLanguage = "it-IT";
                    serverConnectionConfig.ApplicationLanguage = "it-IT";
                    serverConnectionConfig.WebServicesPort = currentRequest.Instance.WebSiteInfo.SitePort;
                    serverConnectionConfig.UnParse(serverConnectionConfigFilePath);
                    serverConnectionConfigUpdated = true;
                }
                catch (Exception exc)
                {
                    this.LogError("Error loading exsting serverconnection.config file, I'll create a brand new file...", exc);
                    serverConnectionConfigFileInfo.Delete();
                }
            }
            if (!serverConnectionConfigUpdated)
            {
                string serverConnectionConfigTemplateContent;
                using (var sr = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Microarea.Mago4Butler.BL.res.ServerConnectionConfig.template")))
                {
                    serverConnectionConfigTemplateContent = sr.ReadToEnd();
                }

                var data = new
                {
                    PreferredLanguage = "it-IT",
                    ApplicationLanguage = "it-IT",
                    WebServicesPort = currentRequest.Instance.WebSiteInfo.SitePort
                };

                Nustache.Core.Render.StringToFile(serverConnectionConfigTemplateContent, data, serverConnectionConfigFilePath);
            }
        }
    }
}
