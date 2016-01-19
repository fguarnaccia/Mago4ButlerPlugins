using Microarea.Mago4Butler.BL;
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
    public partial class SettingsForm : Form
    {
        Settings settings;
        IisService iisService;

        internal SettingsForm(Settings settings, IisService iisService)
        {
            this.settings = settings;
            this.iisService = iisService;
            InitializeComponent();
        }

        private void btnRootFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.Description = "Select the path where all Mago4 installations will live";
                fbd.ShowNewFolderButton = true;

                var res = fbd.ShowDialog(this);

                if (res != DialogResult.OK)
                {
                    return;
                }

                this.txtRootFolder.Text = fbd.SelectedPath;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.txtRootFolder.Text = this.settings.RootFolder;

            var availableWebSites = this.iisService.GetAvailableWebSites();
            int selectedIdx = -1;
            int idx = -1;
            foreach (var site in availableWebSites)
            {
                idx = this.cmbWebSites.Items.Add(site);
                if (String.Compare(site.SiteName, this.settings.SiteName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    selectedIdx = idx;
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (this.DialogResult == DialogResult.OK)
            {
                this.settings.RootFolder = this.txtRootFolder.Text;
                //this.settings.SiteName = (this.cmbWebSites.SelectedItem as WebSiteInfo).SiteName;

                this.settings.Save();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmbWebSites_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
