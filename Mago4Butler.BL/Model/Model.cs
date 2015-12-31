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
        List<string> instances = new List<string>();

        public ICollection<string> Instances
        {
            get
            {
                return instances;
            }
        }
        public void RemoveInstances(ICollection<string> instanceNames)
        {
            if (instanceNames != null)
            {
                foreach (var instanceName in instanceNames)
                {
                    this.RemoveInstance(instanceName);
                }
            }
        }
        public void RemoveInstance(string instanceName)
        {
            instances.Remove(instanceName);
        }
        public void AddInstance(string instanceName)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(instanceName));

            this.instances.Add(instanceName);
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
                        this.instances.Add(instanceDirInfo.Name);
                        break;
                    }
                }
            }
        }
    }
}
