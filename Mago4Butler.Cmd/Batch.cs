using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microarea.Mago4Butler.Cmd
{
    class Batch
    {
        MsiService msiService = new MsiService();
        InstallerService instanceService;
        TextWriter outputWriter;
        Model model;
        bool isRunning;

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public Batch(Model model, TextWriter outputWriter)
        {
            this.model = model;
            this.outputWriter = outputWriter;
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
            this.outputWriter.WriteLine(e.Instances[0].Name + " successfully updated");
            this.PrintCurrentStatus();
        }

        private void InstanceService_Updating(object sender, UpdateInstanceEventArgs e)
        {
            this.outputWriter.WriteLine("Updating " + e.Instances[0].Name + " ...");
        }

        private void InstanceService_Removed(object sender, RemoveInstanceEventArgs e)
        {
            this.outputWriter.WriteLine(e.Instances[0].Name + " successfully removed");
            this.model.RemoveInstances(e.Instances);
            this.PrintCurrentStatus();
        }

        private void InstanceService_Removing(object sender, RemoveInstanceEventArgs e)
        {
            this.outputWriter.WriteLine("Removing " + e.Instances[0].Name + " ...");
        }

        private void InstanceService_Installed(object sender, InstallInstanceEventArgs e)
        {
            this.outputWriter.WriteLine("Installation of " + e.Instance.Name + " completed");
            this.model.AddInstance(e.Instance);
            this.PrintCurrentStatus();
        }

        private void PrintCurrentStatus()
        {
            if (this.model.Instances.Count == 0)
            {
                this.outputWriter.WriteLine("No instances installed");
            }
            else
            {
                this.outputWriter.WriteLine("Current instances are:");
                foreach (var instance in this.model.Instances)
                {
                    this.outputWriter.WriteLine("\t" + instance);
                }
            }
        }

        private void InstanceService_Installing(object sender, InstallInstanceEventArgs e)
        {
            this.outputWriter.WriteLine("Installing " + e.Instance.Name + " ...");
        }

        private void InstanceService_Stopping(object sender, EventArgs e)
        {
            this.outputWriter.WriteLine("Stopping install service ...");
        }

        private void InstanceService_Stopped(object sender, EventArgs e)
        {
            this.outputWriter.WriteLine("Install service stopped");
            this.isRunning = false;
        }

        private void InstanceService_Starting(object sender, EventArgs e)
        {
            this.outputWriter.WriteLine("Starting install service ...");
        }

        private void InstanceService_Started(object sender, EventArgs e)
        {
            this.outputWriter.WriteLine("Install service started");
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
                    this.outputWriter.WriteLine(instance.Name + " already exists, I cannot install it");
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
                    this.outputWriter.WriteLine(instance.Name + " does not exist, I cannot update it");
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
                    this.outputWriter.WriteLine(instance.Name + " does not exist, I cannot uninstall it");
                    continue;
                }
                workingInstances.Add(instance);
            }

            this.instanceService.Uninstall(workingInstances);
        }
    }
}
