using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class Model
    {
        List<Instance> instances = new List<Instance>();

        public event EventHandler<InstanceEventArgs> InstanceAdded;
        public event EventHandler<InstanceEventArgs> InstanceRemoved;
        public event EventHandler<InstanceEventArgs> InstanceUpdated;

        protected virtual void OnInstanceAdded(InstanceEventArgs e)
        {
            var handler = InstanceAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnInstanceRemoved(InstanceEventArgs e)
        {
            var handler = InstanceRemoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnInstanceUpdated(InstanceEventArgs e)
        {
            var handler = InstanceUpdated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public static bool IsInstanceNameValid(string instanceName)
        {
            if (String.IsNullOrWhiteSpace(instanceName))
            {
                return false;
            }
            foreach (var c in instanceName)
            {
                if (!Char.IsLetterOrDigit(c) && c != '-')
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<Instance> Instances
        {
            get
            {
                return instances;
            }
        }
        public void RemoveInstances(ICollection<Instance> instances)
        {
            if (instances != null)
            {
                foreach (var instance in instances)
                {
                    this.RemoveInstance(instance);
                }
            }
        }
        public void RemoveInstance(Instance instance)
        {
            this.instances.Remove(instance);
            this.OnInstanceRemoved(new InstanceEventArgs() { Instance = instance });
        }
        public void AddInstance(Instance instance)
        {
            Debug.Assert(instance != null);
            Debug.Assert(!String.IsNullOrWhiteSpace(instance.Name));
            Debug.Assert(instance.Version != null);

            this.instances.Add(instance);
            this.OnInstanceAdded(new InstanceEventArgs() { Instance = instance });
        }

        public void UpdateInstances(ICollection<Instance> instances)
        {
            if (instances != null)
            {
                foreach (var instance in instances)
                {
                    this.UpdateInstance(instance);
                }
            }
        }

        public void UpdateInstance(Instance instance)
        {
            Debug.Assert(instance != null);
            Debug.Assert(!String.IsNullOrWhiteSpace(instance.Name));

            var oldInstance = this.instances.Find((i) => String.Compare(i.Name, instance.Name, StringComparison.InvariantCultureIgnoreCase) == 0);

            Debug.Assert(oldInstance != null);

            oldInstance.Version = instance.Version;

            this.OnInstanceUpdated(new InstanceEventArgs() { Instance = oldInstance });
        }

        public bool ContainsInstance(string instanceName)
        {
            foreach (var instance in this.instances)
            {
                if (String.Compare(instance.Name, instanceName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsInstance(Instance toSearchFor)
        {
            foreach (var instance in this.instances)
            {
                if (String.Compare(instance.Name, toSearchFor.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void Init(string rootFolder)
        {
            this.instances.Clear();

            var rootDirInfo = new DirectoryInfo(rootFolder);
            if (!rootDirInfo.Exists)
            {
                return;
            }

            var instanceDirInfos = rootDirInfo.GetDirectories();
            foreach (var instanceDirInfo in instanceDirInfos)
            {
                var subDirInfos = instanceDirInfo.GetDirectories();
                foreach (var subDirInfo in subDirInfos)
                {
                    if (String.Compare("Standard", subDirInfo.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        //per non far scattare gli eventi
                        this.instances.Add(Instance.FromStandardDirectoryInfo(subDirInfo));
                        break;
                    }
                }
            }
        }
    }
}
