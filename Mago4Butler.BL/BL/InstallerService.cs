using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class InstallerService
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

        MsiZapper msiZapper;
        RegistryService registryService;
        MsiService msiService;
        IisService iisService;
        FileSystemService fileSystemService;

        string rootPath;
        Queue<Request> requests = new Queue<Request>();

        bool isRunning;

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public InstallerService(string rootPath, MsiService msiService)
        {
            this.rootPath = rootPath;
            this.msiService = msiService;
            this.msiZapper = new MsiZapper(this.msiService);
            this.registryService = new RegistryService(this.msiService);
            this.iisService = new IisService();
            this.fileSystemService = new FileSystemService(rootPath);
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
                RootPath = this.rootPath
            });
            StartService();
        }

        public void Uninstall(ICollection<Instance> instances)
        {
            if (instances.Count == 0)
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
                RootPath = this.rootPath
            });
            StartService();
        }

        public void Update(string msiFullFilePath, ICollection<Instance> instances)
        {
            if (instances.Count == 0)
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
                RootPath = this.rootPath
            });
            StartService();
        }

        private void StartService()
        {
            lock (this.lockTicket)
            {
                if (this.isRunning)
                {
                    return;
                }

                this.OnStarting();
                ThreadPool.QueueUserWorkItem((state) => Worker());
            }
        }

        private void Worker()
        {
            lock (this.lockTicket)
            {
                if (this.IsRunning)
                {
                    return;
                }
                this.isRunning = true;
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
                this.isRunning = false;
            }
            this.OnStopped();
        }

        private void Remove(Request currentRequest)
        {
            //Rimuovere prima le virtual folder e le application, poi gli application pool.
            //Un application pool a cui sono collegate ancora applicazioni non puo` essere eliminato
            this.iisService.RemoveVirtualFoldersAndApplications(currentRequest.Instance);

            this.iisService.RemoveApplicationPools(currentRequest.Instance);

            this.fileSystemService.RemoveAllFiles(currentRequest.Instance);
        }

        private void Update(Request currentRequest)
        {
            var rootDirInfo = new DirectoryInfo(currentRequest.RootPath);
            if (!rootDirInfo.Exists)
            {
                rootDirInfo.Create();
            }

            string msiFolderPath = Path.GetDirectoryName(currentRequest.MsiPath);
            string installLogFilePath = Path.Combine(msiFolderPath, "Mago4_" + currentRequest.Instance.Name + "_UpdateLog.txt");
            this.LaunchProcess(
                msiexecPath,
                String.Format("/i {0} /qb /norestart /l*vx {1} UICULTURE=\"it-IT\" INSTALLLOCATION=\"{2}\" INSTANCENAME=\"{3}\" DEFAULTWEBSITENAME=\"{4}\" DEFAULTWEBSITEID={5} DEFAULTWEBSITEPORT={6} SKIPCLICKONCEDEPLOYER=\"1\" REGISTERWCF=\"1\" NOSHORTCUTS=\"1\" NOSHARES=\"1\"", currentRequest.MsiPath, msiFolderPath, currentRequest.RootPath, currentRequest.Instance.Name, currentRequest.Instance.WebSiteInfo.SiteName, currentRequest.Instance.WebSiteInfo.SiteID, currentRequest.Instance.WebSiteInfo.SitePort),
                3600000
                );
            this.msiZapper.ZapMsi(currentRequest.MsiPath);
            this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
            this.registryService.RemoveInstallerFoldersKeys(currentRequest.RootPath, currentRequest.Instance);
        }

        private void Install(Request currentRequest)
        {
            var rootDirInfo = new DirectoryInfo(currentRequest.RootPath);
            if (!rootDirInfo.Exists)
            {
                rootDirInfo.Create();
            }

            string msiFolderPath = Path.GetDirectoryName(currentRequest.MsiPath);
            string installLogFilePath = Path.Combine(msiFolderPath, "Mago4_" + currentRequest.Instance.Name + "_InstallLog.txt");
            this.LaunchProcess(
                msiexecPath,
                String.Format("/i \"{0}\" /qb /norestart /l*vx \"{1}\" UICULTURE=\"it-IT\" INSTALLLOCATION=\"{2}\" INSTANCENAME=\"{3}\" DEFAULTWEBSITENAME=\"{4}\" DEFAULTWEBSITEID={5} DEFAULTWEBSITEPORT={6} SKIPCLICKONCEDEPLOYER=\"1\" REGISTERWCF=\"1\" NOSHORTCUTS=\"1\" NOHARES=\"1\"", currentRequest.MsiPath, installLogFilePath, currentRequest.RootPath, currentRequest.Instance.Name, currentRequest.Instance.WebSiteInfo.SiteName, currentRequest.Instance.WebSiteInfo.SiteID, currentRequest.Instance.WebSiteInfo.SitePort),
                3600000
                );
            this.msiZapper.ZapMsi(currentRequest.MsiPath);
            this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
            this.registryService.RemoveInstallerFoldersKeys(currentRequest.RootPath, currentRequest.Instance);
        }
    }
}
