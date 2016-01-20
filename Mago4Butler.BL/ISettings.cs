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
        string SiteName { get; set; }

        bool IsFirstRun { get; set; }
    }
}
