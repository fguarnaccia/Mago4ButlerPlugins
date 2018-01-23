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
using System.Diagnostics;

namespace Microarea.Mago4Butler
{
    internal class UIRunner : IForrest, ILogger
    {
        static readonly Mutex mutex = new Mutex(true, "{5EDCA319-7AED-4E2C-A80A-3F545BB7D572}");

        AppAutomationServer appAutomationServer;
        PluginService pluginService;
        MainUIFactory mainUIFactory;

        public UIRunner(PluginService pluginService, MainUIFactory mainUIFactory, AppAutomationServer appAutomationServer)
        {
            this.pluginService = pluginService;
            this.mainUIFactory = mainUIFactory;
            this.appAutomationServer = appAutomationServer;
        }

        public int Run()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                WinApp.EnableVisualStyles();
                WinApp.SetCompatibleTextRenderingDefault(false);

                pluginService.PluginsLoaded += PluginService_PluginsLoaded;

                try
                {
                    var mainUI = mainUIFactory.CreateMainUI();

                    WinApp.Run(mainUI as Form);
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
            }
            else
            {
                var currentProcess = Process.GetCurrentProcess();
                var process = Process.GetProcessesByName(currentProcess.ProcessName)
                    .Where((p => p.Id != currentProcess.Id))
                    .FirstOrDefault();

                if (process != null)
                {
                    SafeNativeMethods.ShowWindow(process.MainWindowHandle, ShowWindowCommands.Restore);
                    SafeNativeMethods.SetForegroundWindow(process.MainWindowHandle);
                }
            }

            return 0;
        }

        private void PluginService_PluginsLoaded(object sender, EventArgs e)
        {
            appAutomationServer.CommandReceived += AppAutomationServer_CommandReceived;
            var workingThread = new Thread(() => appAutomationServer.Start())
            {
                IsBackground = true
            };
            workingThread.Start();

            App.Instance.Init(IoCContainer.Instance.Get<IisService>());

            this.LogInfo("================================================================================");

            var version = GetType().Assembly.GetName().Version.ToString();
            this.LogInfo(String.Format("Mago4 Butler v. {0}", version));

            var plugins = pluginService.Plugins;
            if (plugins.Any())
            {
                this.LogInfo("Loaded plugins:");
                foreach (var plugin in plugins)
                {
                    var pluginName = plugin.GetName();
                    this.LogInfo(string.Format("\t{0} {1}", pluginName, pluginService.GetPluginVersion(pluginName)));
                }
            }
            else
            {
                this.LogInfo("No plugins installed");
            }
            this.LogInfo("--------------------------------------------------------------------------------");
        }

        private void AppAutomationServer_CommandReceived(object sender, CommandEventArgs e)
        {
            Enum.TryParse(e.Command, out Command command);
            switch (command)
            {
                case Command.ShutdownApplication:
                    Environment.Exit(0);
                    break;
                case Command.GetVersion:
                    if (e.Args == Path.GetFileNameWithoutExtension(this.GetType().Assembly.Location))
                    {
                        var fi = new FileInfo(this.GetType().Assembly.Location);
                        e.Response = fi.LastWriteTimeUtc.AddHours(1).Ticks.ToString();//TODO MATTEO: eliminare .AddHours(1). Serve perche` la data di modifica della dll e` sempre minore di quella di modifica dell'msi sul server perche` per forza di cose l'msi viene generato dolo l'exe di butler.
                    }
                    else
                    {
                        e.Response = pluginService.GetPluginVersion(e.Args);
                    }
                    break;
                case Command.GetPluginFolderPath:
                    e.Response = PluginService.PluginsPath;
                    break;
                case Command.GetInstances:
                    {
                        var model = IoCContainer.Instance.Get<Model.Model>();
                        e.Response = string.Join(",", model.Instances.Select(i => i.Name).ToArray());
                        break;
                    }
                case Command.GetZombies:
                    {
                        var model = IoCContainer.Instance.Get<Model.Model>();
                        var fifteenDaysAgo = DateTime.Now.AddDays(-15);
                        var zombies = model.ZombieInstances
                            .Where(zi => zi.CreationTime < fifteenDaysAgo)
                            .Select(d => d.FullName);
                        e.Response = string.Join(",", zombies.ToArray());
                        break;
                    }
                case Command.GetPluginsData:
                    {
                        var responseBld = new StringBuilder();
                        foreach (var plugin in pluginService.Plugins)
                        {
                            var pluginName = plugin.GetName();
                            responseBld.Append(pluginName).Append("-").Append(pluginService.GetPluginVersion(pluginName)).Append(",");
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