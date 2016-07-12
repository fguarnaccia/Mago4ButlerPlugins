using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
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

        public static Instance FromStandardDirectoryInfo(DirectoryInfo standardDirInfo)
        {
            var parentDirInfo = standardDirInfo.Parent;
            var installationVerFileInfo = new FileInfo(Path.Combine(standardDirInfo.FullName, "Installation.ver"));

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
                        return new Instance() { Name = parentDirInfo.Name, Version = Version.Parse(group.Value), WebSiteInfo = WebSiteInfo.DefaultWebSite };
                    }
                }
            }

            Debug.Assert(false);
            return null;
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
