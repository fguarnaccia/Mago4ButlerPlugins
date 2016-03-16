using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Plugins
{
    public class CmdLineInfo
    {
        public bool SkipClickOnceDeployer { get; set; }
        public bool NoShortcuts { get; set; }
        public bool NoShares { get; set; }
        public bool NoEnvVar { get; set; }
        public bool NoEveryone { get; set; }
        public bool ClassicApplicationPoolPipeline { get; set; }
        public bool ProxySettingsSet { get; set; }
        public string ProxyUrl { get; set; }
        public int ProxyPort { get; set; }
        public bool ProxyUserSet { get; set; }
        public string ProxyDomain { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }
    }
}
