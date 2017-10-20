using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Microarea.Mago4Butler.Model
{
    public class Model
    {
        const string configutationFileName = "Mago4Butler.yml";

        ISettings settings;

        List<Instance> instances = new List<Instance>();

        public event EventHandler<InstanceEventArgs> InstanceAdded;
        public event EventHandler<InstanceEventArgs> InstanceRemoved;
        public event EventHandler<InstanceEventArgs> InstanceUpdated;
        public event EventHandler<EventArgs> ModelInitialized;

        protected virtual void OnInstanceAdded(InstanceEventArgs e)
        {
            InstanceAdded?.Invoke(this, e);
        }
        protected virtual void OnInstanceRemoved(InstanceEventArgs e)
        {
            InstanceRemoved?.Invoke(this, e);
        }

        protected virtual void OnInstanceUpdated(InstanceEventArgs e)
        {
            InstanceUpdated?.Invoke(this, e);
        }
        protected virtual void OnModelInitialized()
        {
            ModelInitialized?.Invoke(this, EventArgs.Empty);
        }

        public Model()
        {
            //needed for serialization/deserialization
        }

        public Model(ISettings settings)
        {
            this.settings = settings;
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

            var rootFolderDirInfo = new DirectoryInfo(this.settings.RootFolder);
            if (!rootFolderDirInfo.Exists)
            {
                rootFolderDirInfo.Create();
            }

            LoadFromConfigurationFile();

            var rootDirInfo = new DirectoryInfo(this.settings.RootFolder);
            var instancesOnDisk = new List<Instance>();
            var instanceDirInfos = rootDirInfo.GetDirectories();
            foreach (var instanceDirInfo in instanceDirInfos)
            {
                var subDirInfos = instanceDirInfo.GetDirectories();
                foreach (var subDirInfo in subDirInfos)
                {
                    if (String.Compare("Standard", subDirInfo.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        var instance = FromStandardDirectoryInfo(subDirInfo);
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
                    this.instances[i].ProductType = instanceOnDisk.ProductType;
                }
            }

            OnModelInitialized();
        }

        public Instance FromStandardDirectoryInfo(DirectoryInfo standardDirInfo)
        {
            var parentDirInfo = standardDirInfo.Parent;
            var installationVerFileInfo = new FileInfo(Path.Combine(standardDirInfo.FullName, "Installation.ver"));

            Instance instance = null;
            if (installationVerFileInfo.Exists)
            {
                string content = null;
                using (var sr = installationVerFileInfo.OpenText())
                {
                    content = sr.ReadToEnd();
                }
                var versionRegex = new Regex("<Version>(?<version>.*)</Version>", RegexOptions.IgnoreCase);
                var match = versionRegex.Match(content);
                if (match.Success)
                {
                    var group = match.Groups["version"];
                    if (group != null)
                    {
                        instance = new Instance() { Name = parentDirInfo.Name, Version = Version.Parse(group.Value), WebSiteInfo = WebSiteInfo.DefaultWebSite };
                    }
                }
            }

            if (instance != null)
            {
                var appDataDirInfo = new DirectoryInfo(Path.Combine(standardDirInfo.FullName, "TaskBuilder", "WebFramework", "LoginManager", "App_Data"));
                if (appDataDirInfo.Exists)
                {
                    var magoLicensedFileInfos = appDataDirInfo.GetFiles("Mago*.Licensed.config");
                    if (magoLicensedFileInfos.Length == 1)
                    {
                        var fileNameWOExt = Path.GetFileNameWithoutExtension(magoLicensedFileInfos[0].FullName);
                        var tokens = fileNameWOExt.Split('-');
                        if (tokens.Length == 2)
                        {
                            if (tokens[1].StartsWith("ent", StringComparison.InvariantCultureIgnoreCase))
                            {
                                instance.Edition = Edition.Enterprise;
                            }
                            else if (tokens[1].StartsWith("pro", StringComparison.InvariantCultureIgnoreCase))
                            {
                                instance.Edition = Edition.Professional;
                            }
                            else if (tokens[1].StartsWith("std", StringComparison.InvariantCultureIgnoreCase))
                            {
                                instance.Edition = Edition.Standard;
                            }
                        }
                    }
                }
                if (IsMagoNet(instance))
                {
                    instance.ProductType = ProductType.Magonet;
                }
                else if (IsMago4(instance))
                {
                    instance.ProductType = ProductType.Mago4;
                }
            }

            Debug.Assert(instance != null);
            return instance;
        }

        private bool IsMagoNet(Instance instance)
        {
            var installationVerPath = GetInstallationVer(instance.Name);

            if (!installationVerPath.Exists)
            {
                return false;
            }

            var content = ReadFileContent(installationVerPath);

            return content.IndexOf("<ProductName>Magonet</ProductName>", StringComparison.OrdinalIgnoreCase) != -1;
        }

        private bool IsMago4(Instance instance)
        {
            var installationVerPath = GetInstallationVer(instance.Name);

            if (!installationVerPath.Exists)
            {
                return false;
            }

            var content = ReadFileContent(installationVerPath);

            return content.IndexOf("<ProductName>Mago4</ProductName>", StringComparison.OrdinalIgnoreCase) != -1;
        }

        private static string ReadFileContent(FileInfo installationVerPath)
        {
            using (var inputStream = File.OpenRead(installationVerPath.FullName))
            using (var sr = new StreamReader(inputStream))
            {
                return sr.ReadToEnd();
            }
        }

        private FileInfo GetInstallationVer(string instanceName)
        {
            return new FileInfo(Path.Combine(settings.RootFolder, instanceName, "Standard", "Installation.ver"));
        }

        private void LoadFromConfigurationFile()
        {
            var confFileInfo = new FileInfo(Path.Combine(this.settings.RootFolder, configutationFileName));
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
            var confFileInfo = new FileInfo(Path.Combine(this.settings.RootFolder, configutationFileName));

            var serializer = new YamlDotNet.Serialization.Serializer(SerializationOptions.DisableAliases | SerializationOptions.EmitDefaults);
            using (var outputStream = File.Create(confFileInfo.FullName))
            using (var streamWriter = new StreamWriter(outputStream))
            {
                serializer.Serialize(streamWriter, this);
            }
        }
    }
}
