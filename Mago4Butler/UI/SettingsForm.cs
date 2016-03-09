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
        ISettings settings;
        IisService iisService;
        TextBoxCueDecorator textBoxCueDecorator;

        internal SettingsForm(ISettings settings, IisService iisService)
        {
            this.settings = settings;
            this.iisService = iisService;
            InitializeComponent();
            this.tabControl.TabPages.Remove(tabPageWebSite);//nascondo le impostazioni per il sito web fino a che non saranno completate

            this.textBoxCueDecorator = new TextBoxCueDecorator(this.txtServerUrl) { CueMessage = "e.g. http://serverurl" };
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
            this.ckbAlsoDeleteCustom.Checked = this.settings.AlsoDeleteCustom;
            this.ckbCreateMsiLog.Checked = this.settings.MsiLog;
            this.ckbUseProxy.Checked = this.settings.UseProxy;
            this.txtServerUrl.Text = this.settings.ProxyServerUrl;
            this.txtProxyPort.Text = this.settings.ProxyServerPort.ToString(System.Globalization.CultureInfo.InvariantCulture);
            this.ckbUseCredentials.Checked = this.settings.UseCredentials;
            this.txtDomain.Text = this.settings.DomainName;
            this.txtUsername.Text = this.settings.Username;
            this.txtPassword.Text = this.settings.Password;

            //var availableWebSites = this.iisService.GetAvailableWebSites();
            //int selectedIdx = -1;
            //int idx = -1;
            //foreach (var site in availableWebSites)
            //{
            //    idx = this.cmbWebSites.Items.Add(site);
            //    if (String.Compare(site.SiteName, this.settings.SiteName, StringComparison.InvariantCultureIgnoreCase) == 0)
            //    {
            //        selectedIdx = idx;
            //    }
            //}
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (this.DialogResult == DialogResult.OK)
            {
                this.settings.RootFolder = this.txtRootFolder.Text;
                this.settings.AlsoDeleteCustom = this.ckbAlsoDeleteCustom.Checked;
                this.settings.MsiLog = this.ckbCreateMsiLog.Checked;
                this.settings.UseProxy = this.ckbUseProxy.Checked;
                if (this.settings.UseProxy)
                {
                    this.settings.ProxyServerUrl = this.txtServerUrl.Text;

                    int proxyPort = 0;
                    Int32.TryParse(this.txtProxyPort.Text, out proxyPort);
                    this.settings.ProxyServerPort = proxyPort;
                }
                else
                {
                    this.settings.ProxyServerUrl = string.Empty;
                    this.settings.ProxyServerPort = 0;
                }

                this.settings.UseCredentials = this.ckbUseCredentials.Checked;
                if (this.settings.UseProxy && this.settings.UseCredentials)
                {
                    this.settings.DomainName = this.txtDomain.Text;
                    this.settings.Username = this.txtUsername.Text;
                    this.settings.Password = this.txtPassword.Text;
                }
                else
                {
                    this.settings.DomainName = string.Empty;
                    this.settings.Username = string.Empty;
                    this.settings.Password = string.Empty;
                }

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

        private void ckbUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            this.grpProxy.Enabled = this.ckbUseProxy.Checked;
            this.grpCredentials.Enabled = this.ckbUseCredentials.Checked;
        }

        private void ckbUseCredentials_CheckedChanged(object sender, EventArgs e)
        {
            this.grpCredentials.Enabled = this.ckbUseCredentials.Checked;
        }

        private void EnabledisableBtnOk()
        {
            this.btnOk.Enabled =
                !this.ckbUseProxy.Checked ||
                (
                    this.ckbUseProxy.Checked && IsValidProxyUrl() && IsValidProxyPort() &&
                    (
                        !this.ckbUseCredentials.Checked ||
                        this.ckbUseCredentials.Checked && IsValidDomainName() && IsValidUsername()
                    )
                )
                ;
        }

        private bool IsValidUsername()
        {
            return txtUsername.Text.Length > 0;
        }

        private bool IsValidDomainName()
        {
            return txtDomain.Text.Length > 0;
        }

        private bool IsValidProxyPort()
        {
            int port = 0;
            return Int32.TryParse(txtProxyPort.Text, out port) && port < 65535 && port > 0;
        }

        private bool IsValidProxyUrl()
        {
            try
            {
                var uri = new Uri(txtServerUrl.Text);
                return uri.Scheme.StartsWith(Uri.UriSchemeHttp) || uri.Scheme.StartsWith(Uri.UriSchemeHttps);
            }
            catch
            {
                return false;
            }
        }
    }
}
