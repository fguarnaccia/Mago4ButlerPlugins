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
        public ProductType ProductType { get; set; }

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
