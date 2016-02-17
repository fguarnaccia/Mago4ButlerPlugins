using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microarea.Mago4Butler
{
    class PluginService
    {
        public IEnumerable<IPlugin> LoadAndInitPlugins()
        {
            var pluginsPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            foreach (var dllFileInfo in new DirectoryInfo(pluginsPath).GetFiles("*.dll"))
            {
                yield return LoadPlugin(dllFileInfo);
            }
        }

        public IPlugin LoadPlugin(FileInfo pluginFileInfo)
        {
            byte[] rawAssembly = null;
            using (var inputStream = pluginFileInfo.OpenRead())
            using (var br = new BinaryReader(inputStream))
            {
                rawAssembly = new byte[inputStream.Length];
                br.Read(rawAssembly, 0, rawAssembly.Length);
            }

            var pluginAssembly = AppDomain.CurrentDomain.Load(rawAssembly);
            var pluginType = pluginAssembly.ExportedTypes.Where(t => t.GetInterface("IPlugin") != null).FirstOrDefault();
            IPlugin pluginInstance = null;
            if (pluginType != null)
            {
                pluginInstance = Activator.CreateInstance(pluginType) as IPlugin;
            }

            return pluginInstance;
        }
    }
}
