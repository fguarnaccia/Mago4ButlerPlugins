using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microarea.Mago4Butler
{
    public class PluginService : ILogger
    {
        internal static readonly string PluginsPath = Path.GetDirectoryName(typeof(PluginService).Assembly.Location);
        List<IPlugin> plugins;
        string ipluginTypeName;

        readonly Dictionary<string, string> pluginsVerison = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public event EventHandler<PluginErrorEventArgs> ErrorLoadingPlugins;
        protected virtual void OnErrorLoadingPlugins(PluginErrorEventArgs e)
        {
            ErrorLoadingPlugins?.Invoke(this, e);
        }
        public event EventHandler<EventArgs> PluginsLoaded;
        protected virtual void OnPluginsLoaded()
        {
            var handler = PluginsLoaded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }


        public PluginService()
        {
            this.ipluginTypeName = typeof(IPlugin).FullName;
        }

        public IEnumerable<IPlugin> Plugins
        {
            get
            {
                if (this.plugins == null)
                {
                    this.LoadPlugins();
                    if (this.plugins.Count > 0)
                    {
                        OnPluginsLoaded();
                    }
                }
                return this.plugins;
            }
        }


        void LoadPlugins()
        {
            var plugins = new List<IPlugin>();
            IPlugin plugin = null;
            List<string> pluginsFailedToLoad = new List<string>();
            foreach (var dllFileInfo in new DirectoryInfo(PluginsPath).GetFiles("*.dll"))
            {
#warning TODO CEF
                if (
                    dllFileInfo.Name.StartsWith("cef", StringComparison.InvariantCultureIgnoreCase) ||
                    dllFileInfo.Name.StartsWith("d3", StringComparison.InvariantCultureIgnoreCase) ||
                    dllFileInfo.Name.StartsWith("lib", StringComparison.InvariantCultureIgnoreCase) ||
                    dllFileInfo.Name.StartsWith("widev", StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    continue;
                }
                try
                {
                    plugin = LoadPlugin(dllFileInfo);
                }
                catch (Exception exc)
                {
                    pluginsFailedToLoad.Add(dllFileInfo.Name);
                    this.LogError("Error loading plugin from " + dllFileInfo.FullName, exc);
                    File.Move(dllFileInfo.FullName, Path.ChangeExtension(dllFileInfo.FullName, "disabled"));
                }
                if (plugin != null)
                {
                    plugins.Add(plugin);
                    pluginsVerison.Add(plugin.GetName(), dllFileInfo.LastWriteTimeUtc.Ticks.ToString());
                    plugin = null;
                }
            }

            if (pluginsFailedToLoad.Count > 0)
            {
                this.OnErrorLoadingPlugins(new PluginErrorEventArgs() { PluginsFailedToLoad = pluginsFailedToLoad });
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

            var pluginTypesQuery = from pt in pluginAssembly.ExportedTypes
                                   //Cerco i tipi che implementino l'interfaccia IPlugin e che abbiano un costruttore senza parametri
                    where pt.GetInterface(ipluginTypeName) != null && pt.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.Any, new Type[] { }, null) != null
                    select pt;
            var pluginType = pluginTypesQuery.FirstOrDefault();

            IPlugin pluginInstance = null;
            if (pluginType != null)
            {
                pluginInstance = Activator.CreateInstance(pluginType) as IPlugin;
            }

            return pluginInstance;
        }

        public string GetPluginVersion(string pluginName)
        {
            var pluginVersion = string.Empty;
            pluginsVerison.TryGetValue(pluginName, out pluginVersion);

            return pluginVersion;
        }
    }
}
