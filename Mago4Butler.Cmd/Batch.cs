using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;

namespace Microarea.Mago4Butler.Cmd
{
    class Batch
    {
        MsiService msiService = new MsiService();
        InstallerService instanceService;
        Model model;
        bool isRunning;

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public Batch(Model model)
        {
            this.model = model;
            this.instanceService = new InstallerService(Settings.Default.RootFolder, this.msiService);

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
            Console.WriteLine(e.Instances[0].Name + " successfully updated", Color.Green);
            this.PrintCurrentStatus();
        }

        private void InstanceService_Updating(object sender, UpdateInstanceEventArgs e)
        {
            Console.WriteLine("Updating " + e.Instances[0].Name + " ...");
        }

        private void InstanceService_Removed(object sender, RemoveInstanceEventArgs e)
        {
            Console.WriteLine(e.Instances[0].Name + " successfully removed", Color.Green);
            this.model.RemoveInstances(e.Instances);
            this.PrintCurrentStatus();
        }

        private void InstanceService_Removing(object sender, RemoveInstanceEventArgs e)
        {
            Console.WriteLine("Removing " + e.Instances[0].Name + " ...");
        }

        private void InstanceService_Installed(object sender, InstallInstanceEventArgs e)
        {
            Console.WriteLine("Installation of " + e.Instance.Name + " completed", Color.Green);
            this.model.AddInstance(e.Instance);
            this.PrintCurrentStatus();
        }

        private void PrintCurrentStatus()
        {
            if (this.model.Instances.Count() == 0)
            {
                Console.WriteLine("No instances installed");
            }
            else
            {
                Console.WriteLine("Current instances are:");
                foreach (var instance in this.model.Instances)
                {
                    Console.WriteLine("\t" + instance);
                }
            }
        }

        private void InstanceService_Installing(object sender, InstallInstanceEventArgs e)
        {
            Console.WriteLine("Installing " + e.Instance.Name + " ...");
        }

        private void InstanceService_Stopping(object sender, EventArgs e)
        {
            Console.WriteLine("Stopping install service ...");
        }

        private void InstanceService_Stopped(object sender, EventArgs e)
        {
            Console.WriteLine("Install service stopped", Color.Green);
            this.isRunning = false;
        }

        private void InstanceService_Starting(object sender, EventArgs e)
        {
            Console.WriteLine("Starting install service ...");
        }

        private void InstanceService_Started(object sender, EventArgs e)
        {
            Console.WriteLine("Install service started", Color.Green);
            this.isRunning = true;
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
                    Console.WriteLine(instance.Name + " already exists, I cannot install it", Color.Red);
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
                    Console.WriteLine(instance.Name + " does not exist, I cannot update it", Color.Red);
                    continue;
                }
                workingInstances.Add(instance);
            }
            
            this.instanceService.Update(msiFullfilePath, workingInstances);
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
                    Console.WriteLine(instance.Name + " does not exist, I cannot uninstall it", Color.Red);
                    continue;
                }
                workingInstances.Add(instance);
            }

            this.instanceService.Uninstall(workingInstances);
        }
    }
}
