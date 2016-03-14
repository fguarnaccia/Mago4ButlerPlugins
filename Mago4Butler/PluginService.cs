using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microarea.Mago4Butler
{
    public class PluginService
    {
        List<IPlugin> plugins;
        string pluginsPath;
        string ipluginTypeName;

        public PluginService()
        {
            this.pluginsPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            this.ipluginTypeName = typeof(IPlugin).FullName;
        }

        public IEnumerable<IPlugin> Plugins
        {
            get
            {
                if (this.plugins == null)
                {
                    this.LoadPlugins();
                }
                return this.plugins;
            }
        }


        void LoadPlugins()
        {
            var plugins = new List<IPlugin>();
            foreach (var dllFileInfo in new DirectoryInfo(pluginsPath).GetFiles("*.dll"))
            {
                var plugin = LoadPlugin(dllFileInfo);
                if (plugin != null)
                {
                    plugins.Add(plugin);
                }
            }


            this.plugins = new List<IPlugin>(plugins);
        }

        IPlugin LoadPlugin(FileInfo pluginFileInfo)
        {
            byte[] rawAssembly = null;
            using (var inputStream = pluginFileInfo.OpenRead())
            using (var br = new BinaryReader(inputStream))
            {
                rawAssembly = new byte[inputStream.Length];
                br.Read(rawAssembly, 0, rawAssembly.Length);
            }

            var pluginAssembly = AppDomain.CurrentDomain.Load(rawAssembly);
            var pluginType = pluginAssembly.ExportedTypes.Where(t => t.GetInterface(ipluginTypeName) != null).FirstOrDefault();
            IPlugin pluginInstance = null;
            if (pluginType != null)
            {
                pluginInstance = Activator.CreateInstance(pluginType) as IPlugin;
            }

            return pluginInstance;
        }
    }
}
