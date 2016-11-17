using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Microarea.Mago4Butler.Model
{
    public class Model
    {
        const string configutationFileName = "Mago4Butler.yml";

        string rootFolder;

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

        public Model()
        {
            //needed for serialization/deserialization
        }

        public Model(ISettings settings)
        {
            this.rootFolder = settings.RootFolder;
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

        public Instance[] Instances
        {
            get
            {
                return instances.ToArray();
            }
            internal set
            {
                this.instances.Clear();
                this.instances.AddRange(value);
            }
        }
        public void RemoveInstances(Instance[] instances)
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

            SaveToConfigurationFile();
        }
        public void AddInstance(Instance instance)
        {
            Debug.Assert(instance != null);
            Debug.Assert(!String.IsNullOrWhiteSpace(instance.Name));
            Debug.Assert(instance.Version != null);

            instance.WcfStartPort = GetAvailableWcfStartPort();

            this.instances.Add(instance);
            this.OnInstanceAdded(new InstanceEventArgs() { Instance = instance });

            SaveToConfigurationFile();
        }

        private int GetAvailableWcfStartPort()
        {
            int maxWcfStartPortFound = 9990;

            foreach (var instance in this.instances)
            {
                if (maxWcfStartPortFound <= instance.WcfStartPort )
                {
                    maxWcfStartPortFound = instance.WcfStartPort;
                }
            }

            return maxWcfStartPortFound + 10;
        }

        public void UpdateInstances(Instance[] instances, Version version)
        {
            if (instances != null)
            {
                foreach (var instance in instances)
                {
                    this.UpdateInstance(instance, version);
                }
            }
        }

        public void UpdateInstance(Instance instance, Version version)
        {
            Debug.Assert(instance != null);
            Debug.Assert(!String.IsNullOrWhiteSpace(instance.Name));

            var oldInstance = this.instances.Find((i) => String.Compare(i.Name, instance.Name, StringComparison.InvariantCultureIgnoreCase) == 0);

            Debug.Assert(oldInstance != null);

            if (oldInstance.Version < version)
            {
                oldInstance.Version = version;
                oldInstance.InstalledOn = DateTime.Now;

                this.OnInstanceUpdated(new InstanceEventArgs() { Instance = oldInstance });

                SaveToConfigurationFile();
            }
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
            return this.ContainsInstance(toSearchFor.Name);
        }

        public Instance GetInstance(int instanceIdx)
        {
            if (instanceIdx < 0 || instanceIdx > this.instances.Count)
            {
                throw new ArgumentException("Invalid instance index");
            }
            return this.instances[instanceIdx];
        }

        public Instance GetInstance(string instanceName)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
            {
                return null;
            }
            return
                this.instances
                .Where(i => string.Compare(instanceName, i.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                .FirstOrDefault();
        }

        public void Init()
        {
            this.instances.Clear();

            var rootFolderDirInfo = new DirectoryInfo(this.rootFolder);
            if (!rootFolderDirInfo.Exists)
            {
                rootFolderDirInfo.Create();
            }

            LoadFromConfigurationFile();

            var rootDirInfo = new DirectoryInfo(this.rootFolder);
            var instancesOnDisk = new List<Instance>();
            var instanceDirInfos = rootDirInfo.GetDirectories();
            foreach (var instanceDirInfo in instanceDirInfos)
            {
                var subDirInfos = instanceDirInfo.GetDirectories();
                foreach (var subDirInfo in subDirInfos)
                {
                    if (String.Compare("Standard", subDirInfo.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        var instance = Instance.FromStandardDirectoryInfo(subDirInfo);
                        if (instance != null)
                        {
                            //per non far scattare gli eventi
                            instancesOnDisk.Add(instance);
                        }
                        break;
                    }
                }
            }

            for (int i = this.instances.Count - 1; i >= 0; i--)
            {
                var instanceOnDisk = instancesOnDisk
                    .Where((instance) => instance.Equals(this.instances[i]))
                    .FirstOrDefault();
                if (instanceOnDisk == null)
                {
                    this.instances.RemoveAt(i);
                }
                else
                {
                    this.instances[i].Version = instanceOnDisk.Version;
                    this.instances[i].Edition = instanceOnDisk.Edition;
                }
            }
        }

        private void LoadFromConfigurationFile()
        {
            var confFileInfo = new FileInfo(Path.Combine(this.rootFolder, configutationFileName));
            if (!confFileInfo.Exists)
            {
                return;
            }

            var deserializer = new YamlDotNet.Serialization.Deserializer(null, new PascalCaseNamingConvention(), ignoreUnmatched: true);
            using (var inputStream = confFileInfo.OpenRead())
            using (var streamReader = new StreamReader(inputStream))
            {
                var model = deserializer.Deserialize<Model>(streamReader);
                if (model != null && model.instances.Count > 0)
                {
                    this.instances.AddRange(model.instances);
                }
            }
        }

        private void SaveToConfigurationFile()
        {
            var confFileInfo = new FileInfo(Path.Combine(this.rootFolder, configutationFileName));

            var serializer = new YamlDotNet.Serialization.Serializer(SerializationOptions.DisableAliases | SerializationOptions.EmitDefaults);
            using (var outputStream = File.Create(confFileInfo.FullName))
            using (var streamWriter = new StreamWriter(outputStream))
            {
                serializer.Serialize(streamWriter, this);
            }
        }
    }
}
