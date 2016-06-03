using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    public static class IPluginTrait
    {
        public static System.Version GetVersion(this IPlugin @this)
        {
            return @this.GetType().Assembly.GetName().Version;
        }

        public static string GetName(this IPlugin @this)
        {
            return @this.GetType().FullName;
        }
    }
}
