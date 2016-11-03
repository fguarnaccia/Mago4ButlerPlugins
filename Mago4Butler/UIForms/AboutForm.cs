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
            if (plugins.Count() == 0)
                this.tabControl.TabPages.Remove(this.tabPagePlugins);
            else
            {
                var sb = new StringBuilder();
                foreach (var plugin in plugins)
                {
                    sb.AppendLine(string.Concat(plugin.GetName(), " ", plugin.GetVersion()));
                }
                lblPlugins.Text = sb.ToString();
            }
        }
    }
}
