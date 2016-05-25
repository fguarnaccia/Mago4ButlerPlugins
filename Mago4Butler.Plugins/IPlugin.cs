using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Plugins
{
    public interface IPlugin
    {
        IEnumerable<ContextMenuItem> GetContextMenuItems();
        DoubleClickHandler GetDoubleClickHandler();
        void OnUpdating(CmdLineInfo cmdLineInfo);
        void OnInstalling(CmdLineInfo cmdLineInfo);

        void OnApplicationStarted();

        System.Version Version
        {
            get;
        }
        string Name
        {
            get;
        }

        void OnInstallerServiceStopped();
        void OnInstallerServiceStarted();
    }
}
