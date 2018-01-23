using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class AboutForm : Form
    {
        readonly PluginService pluginService;

        public AboutForm(PluginService pluginService)
        {
            this.pluginService = pluginService;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var version = GetType().Assembly.GetName().Version.ToString();
            this.Text += String.Format(" Mago4 Butler v. {0}", version);

            var plugins = pluginService.Plugins;
            if (plugins.Any())
            {
                var sb = new StringBuilder();
                foreach (var plugin in plugins)
                {
                    var pluginName = plugin.GetName();
                    sb.AppendLine(string.Concat(pluginName, " ", pluginService.GetPluginVersion(pluginName)));
                }
                lblPlugins.Text = sb.ToString();
            }
            else
            {
                this.tabControl.TabPages.Remove(this.tabPagePlugins);
            }
        }
    }
}
