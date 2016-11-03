using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Model
{
    public class Instance
    {
        public string Name { get; set; }
        public Version Version { get; set; }
        public WebSiteInfo WebSiteInfo { get; set; }
        public bool AllowBatchDeletesUpdates { get; set; } = true;
        [YamlDotNet.Serialization.YamlIgnore]
        public string ProvisioningCommandLine { get; set; }
        public DateTime InstalledOn { get; set; } = DateTime.Now;
        public int WcfStartPort { get; set; }
        [YamlDotNet.Serialization.YamlIgnore]
        public Edition Edition { get; set; }

        public static Instance FromStandardDirectoryInfo(DirectoryInfo standardDirInfo)
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
            }

            Debug.Assert(instance != null);
            return instance;
        }

        public override string ToString()
        {
            return String.Format(
                CultureInfo.InvariantCulture,
                "{0}, v.{1} (installed on {2})",
                this.Name,
                this.Version.ToString(),
                this.InstalledOn.ToString("d MMM yyyy HH:mm")
                );
        }

        public override bool Equals(object obj)
        {
            var other = obj as Instance;
            if (other == null)
            {
                return false;
            }
            return String.Compare(this.Name, other.Name, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
