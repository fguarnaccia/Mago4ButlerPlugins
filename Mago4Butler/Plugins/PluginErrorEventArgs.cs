using System.Collections.Generic;

namespace Microarea.Mago4Butler
{
    public class PluginErrorEventArgs
    {
        public IEnumerable<string> PluginsFailedToLoad { get; set; }
    }
}