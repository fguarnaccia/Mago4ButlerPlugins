using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Console = Colorful.Console;

namespace Microarea.Mago4Butler
{
    class Batch : ILogger
    {
        CompanyDBUpdateService companyDBUpdateService;
        InstallerService instanceService;
        Model model;

        public string Now
        {
            get
            {
                return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }

        public Batch(Model model, ISettings settings)
        {
            this.model = model;
            companyDBUpdateService = new CompanyDBUpdateService(settings);
            var msiService = new MsiService(settings);
            this.instanceService = new InstallerService(
                settings,
                msiService,
                this.companyDBUpdateService,
                new MsiZapper(msiService),
                new RegistryService(msiService),
                new IisService(),
                new FileSystemService(settings)
                );

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
        }

        private void InstanceService_Updated(object sender, UpdateInstanceEventArgs e)
        {
            this.LogInfo(e.Instances[0].Name + " successfully updated");
            Console.WriteLine("[" + Now + "]: " + e.Instances[0].Name + " successfully updated", Color.Green);
            this.PrintCurrentStatus();
        }

        private void InstanceService_Updating(object sender, UpdateInstanceEventArgs e)
        {
            this.LogInfo("Updating " + e.Instances[0].Name + "...");
            Console.WriteLine("[" + Now + "]: Updating " + e.Instances[0].Name + " ...");
        }

        private void InstanceService_Removed(object sender, RemoveInstanceEventArgs e)
        {
            this.LogInfo(e.Instances[0].Name + " successfully removed");
            Console.WriteLine("[" + Now + "]: " + e.Instances[0].Name + " successfully removed", Color.Green);
            this.model.RemoveInstances(e.Instances);
            this.PrintCurrentStatus();
        }

        private void InstanceService_Removing(object sender, RemoveInstanceEventArgs e)
        {
            this.LogInfo("Removing " + e.Instances[0].Name + " ...");
            Console.WriteLine("[" + Now + "]: Removing " + e.Instances[0].Name + " ...");
        }

        private void InstanceService_Installed(object sender, InstallInstanceEventArgs e)
        {
            this.LogInfo("Installation of " + e.Instance.Name + " completed");
            Console.WriteLine("[" + Now + "]: Installation of " + e.Instance.Name + " completed", Color.Green);
            this.model.AddInstance(e.Instance);
            this.PrintCurrentStatus();
        }

        public void PrintCurrentStatus()
        {
            if (this.model.Instances.Count() == 0)
            {
                this.LogInfo("No instances installed");
                Console.WriteLine("[" + Now + "]: No instances installed");
            }
            else
            {
                this.LogInfo("Current instances are:");
                Console.WriteLine("[" + Now + "]: Current instances are:");
                foreach (var instance in this.model.Instances)
                {
                    this.LogInfo("\t" + instance);
                    Console.WriteLine("\t" + instance);
                }
            }
        }

        private void InstanceService_Installing(object sender, InstallInstanceEventArgs e)
        {
            this.LogInfo("Installing " + e.Instance.Name + " ...");
            Console.WriteLine("[" + Now + "]: Installing " + e.Instance.Name + " ...");
        }

        private void InstanceService_Stopping(object sender, EventArgs e)
        {
            this.LogInfo("Stopping install service ...");
            Console.WriteLine("[" + Now + "]: Stopping install service ...");
        }

        private void InstanceService_Stopped(object sender, EventArgs e)
        {
            this.LogInfo("Install service stopped");
            this.LogInfo("--------------------------------------------------------------------------------");
            Console.WriteLine("[" + Now + "]: Install service stopped", Color.Green);
        }

        private void InstanceService_Starting(object sender, EventArgs e)
        {
            this.LogInfo("Starting install service ...");
            Console.WriteLine("[" + Now + "]: Starting install service ...");
        }

        private void InstanceService_Started(object sender, EventArgs e)
        {
            this.LogInfo("--------------------------------------------------------------------------------");
            this.LogInfo("Install service started");
            Console.WriteLine("[" + Now + "]: Install service started", Color.Green);
        }

        public void Install(string msiFullfilePath, params Instance[] instances)
        {
            if (instances.Length == 0)
            {
                return;
            }
            var workingInstances = new List<Instance>();
            foreach (var instance in instances)
            {
                if (this.model.ContainsInstance(instance))
                {
                    this.LogError(instance.Name + " already exists, I cannot install it");
                    Console.WriteLine("[" + Now + "]: " + instance.Name + " already exists, I cannot install it", Color.Red);
                    continue;
                }
                if (!Model.IsInstanceNameValid(instance.Name))
                {
                    this.LogError(String.Format("'{0}' is not a valid name for an instance: only letters, digits and '-' are allowed", instance.Name));
                    Console.WriteLine("'{0}' is not a valid name for an instance: only letters, digits and '-' are allowed", instance.Name, Color.Red);
                    continue;
                }
                workingInstances.Add(instance);
            }

            foreach (var instanceName in workingInstances)
            {
                this.instanceService.Install(msiFullfilePath, instanceName);
            }
        }

        public void Update(string msiFullfilePath, params Instance[] instances)
        {
            if (instances.Length == 0)
            {
                return;
            }
            var workingInstances = new List<Instance>();
            foreach (var instance in instances)
            {
                if (!this.model.ContainsInstance(instance))
                {
                    this.LogError(instance.Name + " does not exist, I cannot update it");
                    Console.WriteLine("[" + Now + "]: " + instance.Name + " does not exist, I cannot update it", Color.Red);
                    continue;
                }
                if (!instance.AllowBatchDeletesUpdates)
                {
                    this.LogError(instance.Name + " is not updatable via batch, I cannot update it");
                    Console.WriteLine("[" + Now + "]: " + instance.Name + " is not updatable via batch, I cannot update it", Color.Orange);
                    continue;
                }
                workingInstances.Add(instance);
            }
            
            this.instanceService.Update(msiFullfilePath, workingInstances);
        }

        public void UpdateAll(string msiFullfilePath)
        {
            this.Update(msiFullfilePath, this.model.Instances.ToArray());
        }

        public void Uninstall(params Instance[] instances)
        {
            if (instances.Length == 0)
            {
                return;
            }
            var workingInstances = new List<Instance>();
            foreach (var instance in instances)
            {
                if (!this.model.ContainsInstance(instance))
                {
                    this.LogError(instance.Name + " does not exist, I cannot uninstall it");
                    Console.WriteLine("[" + Now + "]: " + instance.Name + " does not exist, I cannot uninstall it", Color.Red);
                    continue;
                }
                if (!instance.AllowBatchDeletesUpdates)
                {
                    this.LogError(instance.Name + " is not deletable via batch, I cannot delete it");
                    Console.WriteLine("[" + Now + "]: " + instance.Name + " is not deletable via batch, I cannot delete it", Color.Orange);
                    continue;
                }
                workingInstances.Add(instance);
            }

            this.instanceService.Uninstall(workingInstances);
        }

        public void UninstallAll(string msiFullFilePath)
        {
            this.Uninstall(this.model.Instances.ToArray());
        }

        public void WaitingForGodot()
        {
            this.instanceService.Join();
        }
    }
}
