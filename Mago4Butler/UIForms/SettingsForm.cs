using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class SettingsForm : Form, ILogger
    {
        ISettings settings;
        IisService iisService;
        TextBoxCueDecorator serverTextBoxCueDecorator;
        NumericTextboxBehaviour numericTextboxBehaviour;
        SynchronizationContext syncCtx;

        public SettingsForm(ISettings settings, IisService iisService)
        {
            syncCtx = SynchronizationContext.Current;
            if (syncCtx == null)
            {
                syncCtx = new WindowsFormsSynchronizationContext();
            }
            this.settings = settings;
            this.iisService = iisService;
            InitializeComponent();
            this.tabControl.TabPages.Remove(tabPageWebSite);//nascondo le impostazioni per il sito web fino a che non saranno completate

            this.serverTextBoxCueDecorator = new TextBoxCueDecorator(this.txtServerUrl) { CueMessage = "e.g. http://serverurl" };
            this.numericTextboxBehaviour = new NumericTextboxBehaviour(this.txtProxyPort);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.txtRootFolder.Text = this.settings.RootFolder;
            this.txtMsi.Text = this.settings.MsiFolder;
            this.ckbCreateMsiLog.Checked = this.settings.MsiLog;
            this.ckbUseProxy.Checked = this.settings.UseProxy;
            this.txtServerUrl.Text = this.settings.ProxyServerUrl;
            this.txtProxyPort.Text = this.settings.ProxyServerPort.ToString(System.Globalization.CultureInfo.InvariantCulture);
            this.ckbUseCredentials.Checked = this.settings.UseCredentials;
            this.txtDomain.Text = this.settings.DomainName;
            this.txtUsername.Text = this.settings.Username;
            this.txtPassword.Text = this.settings.Password;

            //Trucco per capire se siamo in ambiente PAAS oppure no.
            var svc = IoCContainer.Instance.Get<ShouldUseProvisioningProvider>();
            if (!svc.ShouldUseProvisioning)
            {
                Task.Factory
                    .StartNew(() => LoadSuggestions(this.settings.RootFolder));
            }
            else
            {
                //In ambiente PAAS non applico la logica di suggerimento.
                txtRootFolder.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            }

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

        private void LoadSuggestions(string rootFolder)
        {
            try
            {
                //Do il tempo al control di essere mostrato affinche` abbia handle.
                Thread.Sleep(250);
                var workingFolder = new DirectoryInfo(rootFolder);
                var root = workingFolder.Root;
                while (workingFolder.Parent.Name != root.Name)
                {
                    workingFolder = workingFolder.Parent;
                }

                FileInfo[] ymlFiles = null;

                ymlFiles = workingFolder.GetFiles("*.yml", SearchOption.AllDirectories);

                var datasource = new AutoCompleteStringCollection();
                foreach (var ymlFile in ymlFiles)
                {
                    if (!datasource.Contains(ymlFile.Directory.FullName))
                    {
                        datasource.Add(ymlFile.Directory.FullName);
                    }
                }

                syncCtx.Post(
                    (_) => txtRootFolder.AutoCompleteCustomSource = datasource,
                    null
                    );
            }
            catch (Exception exc)
            {
                this.LogError("Unable to load .yml files from " + rootFolder, exc);
                syncCtx.Post(
                    (_) => txtRootFolder.AutoCompleteCustomSource = new AutoCompleteStringCollection(),
                    null
                    );
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (this.DialogResult != DialogResult.OK)
            {
                return;
            }

            bool ok = true;
            var c = this.txtRootFolder;
            try
            {
                var rootFolderDirInfo = new DirectoryInfo(c.Text);
                if (!rootFolderDirInfo.Exists)
                {
                    throw new Exception("Path does not exist");
                }
            }
            catch (Exception exc)
            {
                this.rootFolderErrorProvider.SetIconPadding(c, -25);
                this.rootFolderErrorProvider.SetError(c, exc.Message);
                ok = false;
            }

            c = this.txtMsi;
            try
            {
                if (SafeNativeMethods.PathIsNetworkPath(c.Text))
                {
                    throw new Exception("Network path is not allowed");
                }
                var msiFolderDirInfo = new DirectoryInfo(c.Text);
                if (!msiFolderDirInfo.Exists)
                {
                    throw new Exception("Path does not exist");
                }
            }
            catch (Exception exc)
            {
                this.msiErrorProvider.SetIconPadding(c, -25);
                this.msiErrorProvider.SetError(c, exc.Message);
                ok = false;
            }

            if (!ok)
            {
                e.Cancel = true;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (this.DialogResult == DialogResult.OK)
            {
                this.settings.RootFolder = this.txtRootFolder.Text;

                var newLogsFolder = Path.Combine(this.settings.RootFolder, new DirectoryInfo(this.settings.LogsFolder).Name);
                if (string.Compare(this.settings.LogsFolder, newLogsFolder, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    this.settings.LogsFolder = newLogsFolder;
                    Program.RefreshLogConfiguration();
                }

                this.settings.MsiFolder = this.txtMsi.Text;

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
            if (!ValidateForm())
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ValidateForm()
        {
            bool isFormValid = true;

            if (!this.ckbUseProxy.Checked)
            {
                return isFormValid;
            }

            if (!IsValidProxyUrl())
            {
                var c = this.txtServerUrl;
                this.proxyUrlErrorProvider.SetIconPadding(c, -25);
                this.proxyUrlErrorProvider.SetError(c, "Invalid proxy url");
                isFormValid = false;

            }
            if (!IsValidProxyPort())
            {
                var c = this.txtProxyPort;
                this.proxyPortErrorProvider.SetIconPadding(c, -25);
                this.proxyPortErrorProvider.SetError(c, "Invalid proxy port");
                isFormValid = false;
            }
            if (!this.ckbUseCredentials.Checked)
            {
                return isFormValid;
            }
            if (!IsValidDomainName())
            {
                var c = this.txtDomain;
                this.domainNameErrorProvider.SetIconPadding(c, -25);
                this.domainNameErrorProvider.SetError(c, "Invalid domain name");
                isFormValid = false;
            }
            if (!IsValidUsername())
            {
                var c = this.txtUsername;
                this.userNameErrorProvider.SetIconPadding(c, -25);
                this.userNameErrorProvider.SetError(c, "Invalid user name");
                isFormValid = false;
            }

            return isFormValid;
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
            ResetErrorProviders(sender, e);
        }

        private void ckbUseCredentials_CheckedChanged(object sender, EventArgs e)
        {
            this.grpCredentials.Enabled = this.ckbUseCredentials.Checked;
            ResetErrorProviders(sender, e);
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

        private void ResetErrorProviders(object sender, EventArgs e)
        {
            this.proxyUrlErrorProvider.Clear();
            this.proxyPortErrorProvider.Clear();
            this.domainNameErrorProvider.Clear();
            this.userNameErrorProvider.Clear();
            this.rootFolderErrorProvider.Clear();
        }

        private void txtRootFolder_TextChanged(object sender, EventArgs e)
        {
            this.ResetErrorProviders(this, EventArgs.Empty);
        }
    }
}
