using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Plugins
{
    public class Mago4ButlerPlugin : IPlugin
    {
        public virtual IEnumerable<ContextMenuItem> GetContextMenuItems()
        {
            return new List<ContextMenuItem>();
        }

        public virtual DoubleClickHandler GetDoubleClickHandler()
        {
            return null;
        }

        public virtual void OnApplicationStarted()
        {
            
        }

        public virtual void OnAskForParametersForInstall(AskForParametersBag bag)
        {

        }

        public virtual void OnAskForParametersForUpdate(AskForParametersBag bag)
        {
            
        }

        public virtual void OnInstallerServiceStarted()
        {
            
        }

        public virtual void OnInstallerServiceStopped()
        {
            
        }

        public virtual void OnInstalling(CmdLineInfo cmdLineInfo)
        {
            
        }

        public virtual void OnRemoving(Instance[] instances)
        {
            
        }

        public virtual void OnUpdating(CmdLineInfo cmdLineInfo)
        {
            
        }

        public virtual bool ShouldUseProvisioning()
        {
            return true;
        }
    }
}
