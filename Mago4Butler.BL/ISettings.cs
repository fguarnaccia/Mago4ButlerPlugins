using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public interface ISettings
    {
        string RootFolder { get; set; }
        string LogsFolder { get; set; }
        string MsiFolder { get; set; }
        string SiteName { get; set; }
        bool ShowRootFolderChoice { get; set; }
        bool AlsoDeleteCustom { get; set; }
        bool MsiLog { get; set; }
        bool UseProxy { get; set; }

        string ProxyServerUrl { get; set; }
        int ProxyServerPort { get; set; }
        bool UseCredentials { get; set; }
        string DomainName { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string LastFolderOpenedBrowsingForMsi { get; set; }

        void Save();
    }
}
