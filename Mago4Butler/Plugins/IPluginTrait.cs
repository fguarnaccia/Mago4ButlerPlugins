using Microarea.Mago4Butler.Plugins;

namespace Microarea.Mago4Butler
{
    public static class IPluginTrait
    {
        public static string GetName(this IPlugin @this)
        {
            return @this.GetType().FullName;
        }
    }
}
