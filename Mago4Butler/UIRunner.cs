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

namespace Microarea.Mago4Butler
{
    internal class UIRunner : IForrest, ILogger
    {
        AppAutomationServer appAutomationServer = new AppAutomationServer();

        public int Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var pluginService = IoCContainer.Instance.Get<PluginService>();
                //pluginService.ErrorLoadingPlugins += PluginService_ErrorLoadingPlugins;
                pluginService.PluginsLoaded += PluginService_PluginsLoaded;

                var mainForm = IoCContainer.Instance.Get<MainForm>();

                Application.Run(mainForm);
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
            var workingThread = new Thread(() => appAutomationServer.Start());
            workingThread.IsBackground = true;
            workingThread.Start();

            App.Instance.Init();
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