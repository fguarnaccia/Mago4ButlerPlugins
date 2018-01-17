using System.Collections.Generic;

namespace Microarea.Mago4Butler.Plugins
{
    public interface IPlugin
    {
        IEnumerable<ToolstripMenuItem> GetToolstripMenuItems();
        IEnumerable<ContextMenuItem> GetContextMenuItems();
        DoubleClickHandler GetDoubleClickHandler();

        void OnUpdating(CmdLineInfo cmdLineInfo);
        void OnUpdated(Instance[] instances);

        void OnInstalling(CmdLineInfo cmdLineInfo);
        void OnInstalled(Instance instance);

        void OnRemoving(Instance[] instances);
        void OnRemoved(Instance[] instances);

        void OnApplicationStarted();

        void OnInstallerServiceStopped();
        void OnInstallerServiceStarted();
        bool ShouldUseProvisioning();
        void OnAskForParametersForInstall(AskForParametersBag bag);
        void OnAskForParametersForUpdate(AskForParametersBag bag);
    }
}
