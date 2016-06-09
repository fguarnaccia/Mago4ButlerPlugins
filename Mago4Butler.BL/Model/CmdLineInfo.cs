using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
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
        public IList<Feature> Features { get; set; }

        public override string ToString()
        {
            var cmdLineBld = new StringBuilder();

            cmdLineBld.Append("SKIPCLICKONCEDEPLOYER=\"")
                .Append(SkipClickOnceDeployer ? "1" : "0")
                .Append("\"");

            cmdLineBld.Append(" NOSHORTCUTS=\"")
                .Append(NoShortcuts ? "1" : "0")
                .Append("\"");

            cmdLineBld.Append(" NOSHARES=\"")
                .Append(NoShares ? "1" : "0")
                .Append("\"");

            cmdLineBld.Append(" NOENVVAR=\"")
                .Append(NoEnvVar ? "1" : "0")
                .Append("\"");

            cmdLineBld.Append(" NOEVERYONE=\"")
                .Append(NoEveryone ? "1" : "0")
                .Append("\"");

            cmdLineBld.Append(" CLASSICPIPELINE=\"")
                .Append(ClassicApplicationPoolPipeline ? "1" : "0")
                .Append("\"");

            if (this.ProxySettingsSet)
            {
                cmdLineBld
                    .Append(" PROXYSETTINGSSET=\"1\" PROXYURL=\"")
                    .Append(this.ProxyUrl).Append("\" PROXYPORT=\"")
                    .Append(this.ProxyPort).Append("\"");

                if (this.ProxyUserSet)
                {
                    cmdLineBld
                        .Append(" PROXYUSERSET=\"1\" PROXYDOMAIN=\"")
                        .Append(this.ProxyDomain).Append("\" PROXYUSERNAME=\"")
                        .Append(this.ProxyUsername).Append("\" PROXYPASSWORD=\"")
                        .Append(this.ProxyPassword).Append("\"");
                }
            }

            cmdLineBld.Append(" ADDLOCAL=\"")
                .Append(string.Join(",", Features.Select(f => f.Name)))
                .Append("\"");

            return cmdLineBld.ToString();
        }
    }
}
