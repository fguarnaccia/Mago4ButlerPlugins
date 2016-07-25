﻿using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microarea.Mago4Butler
{
    public class PluginService : ILogger
    {
        internal static readonly string PluginsPath = Path.GetDirectoryName(typeof(PluginService).Assembly.Location);
        List<IPlugin> plugins;
        string ipluginTypeName;

        public event EventHandler<PluginErrorEventArgs> ErrorLoadingPlugins;
        protected virtual void OnErrorLoadingPlugins(PluginErrorEventArgs e)
        {
            var handler = ErrorLoadingPlugins;
            if (handler != null)
            {
                handler(this, e);
            }
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
