using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace Microarea.Mago4Butler
{
    internal class UIRunner : IForrest
    {
        public int Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var pluginService = IoCContainer.Instance.Get<PluginService>();
            pluginService.ErrorLoadingPlugins += PluginService_ErrorLoadingPlugins;

            var mainForm = IoCContainer.Instance.Get<MainForm>();

            Application.Run(mainForm);

            return 0;
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