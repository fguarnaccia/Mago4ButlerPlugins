using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.IO.Pipes;
using System.IO;
using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Plugins;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Automation;
using WinApp = System.Windows.Forms.Application;

namespace Microarea.Mago4Butler
{
    internal class UIRunner : IForrest, ILogger
    {
        AppAutomationServer appAutomationServer = new AppAutomationServer();

        public int Run()
        {
            WinApp.EnableVisualStyles();
            WinApp.SetCompatibleTextRenderingDefault(false);

            try
            {
                var pluginService = IoCContainer.Instance.Get<PluginService>();
                pluginService.PluginsLoaded += PluginService_PluginsLoaded;

                var mainForm = IoCContainer.Instance.Get<Form>();

                WinApp.Run(mainForm);
            }
            catch (Exception exc)
            {
                this.LogError("Error running the application.", exc);
                return 1;
            }
            finally
            {
                App.Instance.Dispose();
                appAutomationServer.Dispose();
            }

            return 0;
        }

        private void PluginService_PluginsLoaded(object sender, EventArgs e)
        {
            appAutomationServer.CommandReceived += AppAutomationServer_CommandReceived;
            var workingThread = new Thread(() => appAutomationServer.Start());
            workingThread.IsBackground = true;
            workingThread.Start();

            App.Instance.Init();
        }

        private void AppAutomationServer_CommandReceived(object sender, CommandEventArgs e)
        {
            Command command;
            Enum.TryParse(e.Command, out command);
            switch (command)
            {
                case Command.ShutdownApplication:
                    Environment.Exit(0);
                    break;
                case Command.GetVersion:
                    if (e.Args == Path.GetFileNameWithoutExtension(this.GetType().Assembly.Location))
                    {
                        e.Response = this.GetType().Assembly.GetName().Version.ToString();
                    }
                    else
                    {
                        bool found = false;
                        foreach (var plugin in IoCContainer.Instance.Get<PluginService>().Plugins)
                        {
                            if (plugin.GetName() == e.Args)
                            {
                                found = true;
                                e.Response = plugin.GetVersion().ToString();
                            }
                        }
                        if (!found)
                        {
                            e.Response = string.Empty;
                        }
                    }
                    break;
                case Command.GetPluginFolderPath:
                    e.Response = PluginService.PluginsPath;
                    break;
                case Command.GetInstances:
                    var model = IoCContainer.Instance.Get<Model.Model>();
                    e.Response = string.Join(",", model.Instances.Select(i => i.Name).ToArray());
                    break;
                case Command.GetPluginsData:
                    {
                        var pluginService = IoCContainer.Instance.Get<PluginService>();
                        var responseBld = new StringBuilder();
                        foreach (var plugin in pluginService.Plugins)
                        {
                            responseBld.Append(plugin.GetName()).Append("-").Append(plugin.GetVersion()).Append(",");
                        }
                        responseBld.Remove(responseBld.Length - 1, 1);
                        e.Response = responseBld.ToString();
                        break;
                    }
                default:
                    break;
            }
        }

        private void PluginService_ErrorLoadingPlugins(object sender, PluginErrorEventArgs e)
        {
            var messageBld = new StringBuilder();
            messageBld.Append("Error loading following plugins, they will not be available")
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append(String.Join(Environment.NewLine, e.PluginsFailedToLoad))
                ;
            MessageBox.Show(
                messageBld.ToString(),
                "Mago4Butler",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
        }
    }
}