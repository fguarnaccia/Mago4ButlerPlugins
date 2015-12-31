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

        MsiZapper msiZapper = new MsiZapper();
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

        public InstallerService(string rootPath)
        {
            this.rootPath = rootPath;
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

        public void Install(string msiFullFilePath, string instanceName)
        {
            this.Enqueue(new Request()
            {
                InstanceName = instanceName,
                MsiPath = msiFullFilePath,
                RequestType = RequestType.Install,
                RootPath = this.rootPath
            });
            StartService();
        }

        public void Uninstall(ICollection<string> instanceNames)
        {
            if (instanceNames.Count == 0)
            {
                return;
            }
            foreach (var instanceName in instanceNames)
            {
                this.Enqueue(new Request()
                {
                    InstanceName = instanceName,
                    MsiPath = String.Empty,
                    RequestType = RequestType.Remove,
                    RootPath = this.rootPath
                });
            }
            StartService();
        }

        public void Update(string msiFullFilePath, ICollection<string> instanceNames)
        {
            if (instanceNames.Count == 0)
            {
                return;
            }
            foreach (var instanceName in instanceNames)
            {
                this.Enqueue(new Request()
                {
                    InstanceName = instanceName,
                    MsiPath = msiFullFilePath,
                    RequestType = RequestType.Update,
                    RootPath = this.rootPath
                });
            }
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
                            this.OnInstalling(new InstallInstanceEventArgs() { InstanceName = currentRequest.InstanceName });
                            this.Install(currentRequest);

                            this.OnInstalled(new InstallInstanceEventArgs() { InstanceName = currentRequest.InstanceName });

                            break;
                        }
                    case RequestType.Update:
                        {
                            this.OnUpdating(new UpdateInstanceEventArgs() { InstanceNames = new List<string>() { currentRequest.InstanceName } });
                            this.Update(currentRequest);

                            this.OnUpdated(new UpdateInstanceEventArgs() { InstanceNames = new List<string>() { currentRequest.InstanceName } });

                            break;
                        }
                    case RequestType.Remove:
                        {
                            this.OnRemoving(new RemoveInstanceEventArgs() { InstanceNames = new List<string>() { currentRequest.InstanceName } });
                            this.Remove(currentRequest);

                            this.OnRemoved(new RemoveInstanceEventArgs() { InstanceNames = new List<string>() { currentRequest.InstanceName } });
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
#warning Remove application pools
#warning Remove virtual dirs and applications
#warning Remove network shares (se li creo in cloud)
#warning Remove Desktop shortcuts
#warning Remove Program Menu shortcuts
#warning Remove Files
        }

        private void Update(Request currentRequest)
        {
            var rootDirInfo = new DirectoryInfo(currentRequest.RootPath);
            if (!rootDirInfo.Exists)
            {
                rootDirInfo.Create();
            }

            string msiFolderPath = Path.GetDirectoryName(currentRequest.MsiPath);
            string installLogFilePath = Path.Combine(msiFolderPath, "Mago4_" + currentRequest.InstanceName + "_UpdateLog.txt");
            this.LaunchProcess(
                msiexecPath,
                String.Format("/i {0} /qb /norestart /l*vx {1} UICULTURE=it-IT INSTALLLOCATION=\"{2}\" INSTANCENAME={3}", currentRequest.MsiPath, msiFolderPath, currentRequest.RootPath, currentRequest.InstanceName),
                3600000
                );
            msiZapper.ZapMsi(currentRequest.MsiPath);
        }

        private void Install(Request currentRequest)
        {
            var rootDirInfo = new DirectoryInfo(currentRequest.RootPath);
            if (!rootDirInfo.Exists)
            {
                rootDirInfo.Create();
            }
#warning Attenzione che qui passo il nome dell'istanza, ma viene sovrascritto con il valore trovato nel registry.
            string msiFolderPath = Path.GetDirectoryName(currentRequest.MsiPath);
            string installLogFilePath = Path.Combine(msiFolderPath, "Mago4_" + currentRequest.InstanceName + "_InstallLog.txt");
            this.LaunchProcess(
                msiexecPath,
                String.Format("/i \"{0}\" /qb /norestart /l*vx \"{1}\" UICULTURE=it-IT INSTALLLOCATION=\"{2}\" INSTANCENAME=\"{3}\"", currentRequest.MsiPath, installLogFilePath, currentRequest.RootPath, currentRequest.InstanceName),
                3600000
                );
            msiZapper.ZapMsi(currentRequest.MsiPath);
        }
    }
}
