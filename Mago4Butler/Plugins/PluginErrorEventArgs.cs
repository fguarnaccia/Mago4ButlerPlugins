using System.Collections.Generic;

namespace Microarea.Mago4Butler
{
    public class PluginErrorEventArgs: System.EventArgs
    {
        public IEnumerable<string> PluginsFailedToLoad { get; set; }
    }
}