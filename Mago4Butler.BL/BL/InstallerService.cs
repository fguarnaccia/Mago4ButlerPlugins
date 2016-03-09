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
    public class InstallerService : ILogger
    {
        readonly object lockTicket = new object();
        const string msiexecPath = @"C:\Windows\System32\msiexec.exe";

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

        MsiZapper msiZapper;
        RegistryService registryService;
        MsiService msiService;
        IisService iisService;
        FileSystemService fileSystemService;
        CompanyDBUpdateService companyDBUpdateService;

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
            CompanyDBUpdateService companyDBUpdateService,
            MsiZapper msiZapper,
            RegistryService registryService,
            IisService iisService,
            FileSystemService fileSystemService
            )
        {
            this.settings = settings;
            this.msiService = msiService;
            this.companyDBUpdateService = companyDBUpdateService;
            this.msiZapper = msiZapper;// new MsiZapper(this.msiService);
            this.registryService = registryService;// new RegistryService(this.msiService);
            this.iisService = iisService;// new IisService();
            this.fileSystemService = fileSystemService;// new FileSystemService(settings);
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
                            this.OnInstalling(new InstallInstanceEventArgs() { Instance = currentRequest.Instance });
                            this.Install(currentRequest);

                            this.OnInstalled(new InstallInstanceEventArgs() { Instance = currentRequest.Instance });

                            break;
                        }
                    case RequestType.Update:
                        {
                            this.OnUpdating(new UpdateInstanceEventArgs() { Instances = new List<Instance>() { currentRequest.Instance } });
                            this.Update(currentRequest);

                            this.OnUpdated(new UpdateInstanceEventArgs() { Instances = new List<Instance>() { currentRequest.Instance } });

                            break;
                        }
                    case RequestType.Remove:
                        {
                            this.OnRemoving(new RemoveInstanceEventArgs() { Instances = new List<Instance>() { currentRequest.Instance } });
                            this.Remove(currentRequest);

                            this.OnRemoved(new RemoveInstanceEventArgs() { Instances = new List<Instance>() { currentRequest.Instance } });
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

        private void Update(Request currentRequest)
        {
            OnNotification(new NotificationEventArgs() { Message = "Removing installation info..." });
            //Rimuovo le informazioni di installazione dal registry se presenti in
            //modo che la mia installazione non le trovi e tenga i parametri che passo io da riga di comando.
            this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
            OnNotification(new NotificationEventArgs() { Message = "Installation info removed" });

            //Rimuovo la parte di installazione su IIS per evitare che, se tra un setup e il successivo
            //alcuni componenti cambiano noe, mi rimangano dei cadaveri.
            //Rimuovere prima le virtual folder e le application, poi gli application pool.
            //Un application pool a cui sono collegate ancora applicazioni non puo` essere eliminato
            OnNotification(new NotificationEventArgs() { Message = "Removing virtual folders..." });
            this.iisService.RemoveVirtualFoldersAndApplications(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Virtual folders removed" });
            OnNotification(new NotificationEventArgs() { Message = "Removing application pools..." });
            this.iisService.RemoveApplicationPools(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Application pools removed" });

            string logFilesFolderPath = CreateApplicationFolders(currentRequest);            

            OnNotification(new NotificationEventArgs() { Message = "Launching msi..." });
            string installLogFilePath = Path.Combine(logFilesFolderPath, "Mago4_" + currentRequest.Instance.Name + "_UpdateLog_" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture) + ".log");
            string proxyCmdLine = GerProxyCmdLine();
            try
            {
                this.LaunchProcess(
                        msiexecPath,
                        String.Format("/i \"{0}\" /qn /norestart {1} UICULTURE=\"it-IT\" INSTALLLOCATION=\"{2}\" INSTANCENAME=\"{3}\" DEFAULTWEBSITENAME=\"{4}\" DEFAULTWEBSITEID={5} DEFAULTWEBSITEPORT={6} SKIPCLICKONCEDEPLOYER=\"1\" REGISTERWCF=\"1\" NOSHORTCUTS=\"1\" NOSHARES=\"1\" NOENVVAR=\"1\" NOEVERYONE=\"1\" {7}", currentRequest.MsiPath, this.settings.MsiLog ? String.Format("/l*vx \"{0}\"", installLogFilePath) : string.Empty, currentRequest.RootFolder, currentRequest.Instance.Name, currentRequest.Instance.WebSiteInfo.SiteName, currentRequest.Instance.WebSiteInfo.SiteID, currentRequest.Instance.WebSiteInfo.SitePort, proxyCmdLine),
                        3600000
                        );
                OnNotification(new NotificationEventArgs() { Message = "Msi execution successfully terminated..." });
            }
            catch (Exception exc)
            {
                this.LogError("Msi execution terminated with errors...", exc);
                OnNotification(new NotificationEventArgs() { Message = "Msi execution terminated with errors..." });
                OnError(new InstallerServiceErrorEventArgs()
                {
                    Message = "Error updating " + currentRequest.Instance.Name,
                    Error = exc
                });
            }

            OnNotification(new NotificationEventArgs() { Message = "Cleaning registry..." });
            this.msiZapper.ZapMsi(currentRequest.MsiPath);
            this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
            this.registryService.RemoveInstallerFoldersKeys(currentRequest.RootFolder, currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Now the registry is clean" });

            OnNotification(new NotificationEventArgs() { Message = "Updating company database..." });
            this.companyDBUpdateService.UpdateCompanyDB(currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Company databsae updated successfully" });
        }

        private void Install(Request currentRequest)
        {
            OnNotification(new NotificationEventArgs() { Message = "Removing installation info..." });
            //Rimuovo le informazioni di installazione dal registry se presenti in
            //modo che la mia installazione non le trovi e tenga i parametri che passo io da riga di comando.
            this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
            OnNotification(new NotificationEventArgs() { Message = "Installation info removed" });
            string logFilesFolderPath = CreateApplicationFolders(currentRequest);

            OnNotification(new NotificationEventArgs() { Message = "Launching msi..." });
            string installLogFilePath = Path.Combine(logFilesFolderPath, "Mago4_" + currentRequest.Instance.Name + "_InstallLog_" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture) + ".log");
            string proxyCmdLine = GerProxyCmdLine();
            try
            {
                this.LaunchProcess(
                        msiexecPath,
                        String.Format("/i \"{0}\" /qn /norestart {1} UICULTURE=\"it-IT\" INSTALLLOCATION=\"{2}\" INSTANCENAME=\"{3}\" DEFAULTWEBSITENAME=\"{4}\" DEFAULTWEBSITEID={5} DEFAULTWEBSITEPORT={6} SKIPCLICKONCEDEPLOYER=\"1\" REGISTERWCF=\"1\" NOSHORTCUTS=\"1\" NOSHARES=\"1\" NOENVVAR=\"1\" NOEVERYONE=\"1\" {7}", currentRequest.MsiPath, this.settings.MsiLog ? String.Format("/l*vx \"{0}\"", installLogFilePath) : string.Empty, currentRequest.RootFolder, currentRequest.Instance.Name, currentRequest.Instance.WebSiteInfo.SiteName, currentRequest.Instance.WebSiteInfo.SiteID, currentRequest.Instance.WebSiteInfo.SitePort, proxyCmdLine),
                        3600000
                        );
                OnNotification(new NotificationEventArgs() { Message = "Msi execution successfully terminated..." });
            }
            catch (Exception exc)
            {
                this.LogError("Msi execution terminated with errors...", exc);
                OnNotification(new NotificationEventArgs() { Message = "Msi execution terminated with errors..." });
                OnError(new InstallerServiceErrorEventArgs()
                {
                    Message = "Error installing " + currentRequest.Instance.Name,
                    Error = exc
                });
            }

            OnNotification(new NotificationEventArgs() { Message = "Cleaning registry..." });
            this.msiZapper.ZapMsi(currentRequest.MsiPath);
            this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
            this.registryService.RemoveInstallerFoldersKeys(currentRequest.RootFolder, currentRequest.Instance);
            OnNotification(new NotificationEventArgs() { Message = "Now the registry is clean" });

            OnNotification(new NotificationEventArgs() { Message = "Configuring the application..." });
            this.SaveServerConnectionConfig(currentRequest);
            OnNotification(new NotificationEventArgs() { Message = "Application configured" });
        }

        private string GerProxyCmdLine()
        {
            if (!this.settings.UseProxy)
            {
                return string.Empty;
            }
            var cmdLineBld = new StringBuilder();
            cmdLineBld
                .Append("PROXYSETTINGSSET=\"1\" PROXYURL=\"")
                .Append(this.settings.ProxyServerUrl).Append("\" PROXYPORT=\"")
                .Append(this.settings.ProxyServerPort).Append("\"")
                ;

            if (!this.settings.UseCredentials)
            {
                return cmdLineBld.ToString();
            }

            cmdLineBld
                .Append(" PROXYUSERSET=\"1\" PROXYDOMAIN=\"")
                .Append(this.settings.DomainName).Append("\" PROXYUSERNAME=\"")
                .Append(this.settings.Username).Append("\" PROXYPASSWORD=\"")
                .Append(this.settings.Password).Append("\"")
                ;

            return cmdLineBld.ToString();
        }

        private string CreateApplicationFolders(Request currentRequest)
        {
            string logFilesFolderPath = null;
            try
            {
                var rootDirInfo = new DirectoryInfo(currentRequest.RootFolder);
                if (!rootDirInfo.Exists)
                {
                    OnNotification(new NotificationEventArgs() { Message = String.Format(CultureInfo.InvariantCulture, "Creating root folder {0}...", currentRequest.RootFolder) });
                    rootDirInfo.Create();
                    OnNotification(new NotificationEventArgs() { Message = String.Format(CultureInfo.InvariantCulture, "Root folder {0} created", currentRequest.RootFolder) });
                }

                logFilesFolderPath = Path.Combine(currentRequest.RootFolder, "Logs");
                if (!Directory.Exists(logFilesFolderPath))
                {
                    OnNotification(new NotificationEventArgs() { Message = String.Format(CultureInfo.InvariantCulture, "Creating logs folder {0}...", currentRequest.RootFolder) });
                    Directory.CreateDirectory(logFilesFolderPath);
                    OnNotification(new NotificationEventArgs() { Message = String.Format(CultureInfo.InvariantCulture, "Logs folder {0} created", currentRequest.RootFolder) });
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error creating root folder or log folder", exc);
            }

            return logFilesFolderPath;
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

            string serverConnectionConfigTemplateContent;
            using (var sr = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Microarea.Mago4Butler.BL.res.ServerConnectionConfig.template")))
            {
                serverConnectionConfigTemplateContent = sr.ReadToEnd();
            }

            var data = new {
                PreferredLanguage = "it-IT",
                ApplicationLanguage = "it-IT",
                WebServicesPort = currentRequest.Instance.WebSiteInfo.SitePort
            };

            var serverConnectionConfigFileInfo = new FileInfo(serverConnectionConfigFilePath);
            if (serverConnectionConfigFileInfo.Exists)
            {
                serverConnectionConfigFileInfo.Delete();
            }

            Nustache.Core.Render.StringToFile(serverConnectionConfigTemplateContent, data, serverConnectionConfigFilePath);
        }
    }
}
