using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microarea.Mago4Butler.Model;

namespace Microarea.Mago4Butler
{
    public partial class CefForm : Form
    {
        Model.Model model;
        IUIMediator uiMediator;
        ChromiumWebBrowser chromeBrowser;

        public CefForm(IUIMediator uiMediator, Model.Model model)
        {
            this.model = model;
            this.uiMediator = uiMediator;

            InitializeComponent();

            InitializeChromium();

            this.Text = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} v. {1}", this.Text, this.GetType().Assembly.GetName().Version.ToString());

            this.uiMediator.JobNotification += UiMediator_Notification;
            this.uiMediator.ParametersForInstallNeeded += UiMediator_AskForParametersForInstall;
            this.uiMediator.ProvisioningNeeded += UiMediator_ProvisioningNeeded;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Cef.Shutdown();
        }

        private void InitializeChromium()
        {
            var settings = new CefSettings
            {
                BrowserSubprocessPath = "CefSharp.BrowserSubprocess.exe",
                RemoteDebuggingPort = 8088,
                LogSeverity = LogSeverity.Verbose
            };

            var schemeHandlerFactory = IoCContainer.Instance.Get<ButlerSchemeHandlerFactory>();

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeHandlerFactory = schemeHandlerFactory,
                SchemeName = schemeHandlerFactory.ButlerSchemeName
            });

            if (!Cef.Initialize(settings))
            {
                throw new Exception("Failed to init view");
            }

            var startPage = string.Format(CultureInfo.InvariantCulture, "{0}://cef/index.html", schemeHandlerFactory.ButlerSchemeName);
            this.chromeBrowser = new ChromiumWebBrowser(startPage);
            this.chromeBrowser.RegisterJsObject("model", this.model);
            this.chromeBrowser.MenuHandler = IoCContainer.Instance.Get<ContextMenuHandler>();
            this.Controls.Add(chromeBrowser);
            this.chromeBrowser.Dock = DockStyle.Fill;
        }

        private void UiMediator_ProvisioningNeeded(object sender, ProvisioningEventArgs e)
        {
            //using (var provisioningForm = new ProvisioningFormLITE(instanceName: e.InstanceName, preconfigurationMode: true, loadDataFromFile: false))
            //{
            //    var provisioningSuggestions = new ProvisioningData()
            //    {
            //        CompanyDbName = string.Format("{0}_DB", e.InstanceName),
            //        DMSDbName = string.Format("{0}_DBDMS", e.InstanceName),
            //        SystemDbName = string.Format("{0}_SystemDB", e.InstanceName),
            //        AdminLoginName = string.Format("{0}_admin", e.InstanceName),
            //        UserLoginName = string.Format("{0}_user", e.InstanceName)
            //    };

            //    provisioningForm.ProvisioningData = provisioningSuggestions;

            //    var diagRes = provisioningForm.ShowDialog(this);

            //    if (diagRes == DialogResult.Cancel)
            //    {
            //        e.Cancel = true;
            //        return;
            //    }
            //    e.ProvisioningCommandLine = provisioningForm.PreconfigurationCommandLine;
            //}
        }

        private void UiMediator_AskForParametersForInstall(object sender, JobEventArgs e)
        {
            //using (var askForParametersDialog = IoCContainer.Instance.Get<AskForParametersForm>())
            //{
            //    var diagRes = askForParametersDialog.ShowDialog(this);

            //    if (diagRes != DialogResult.OK)
            //    {
            //        e.Bag.Cancel = true;
            //        return;
            //    }
            //    e.Bag.InstanceName = askForParametersDialog.InstanceName;
            //}
        }

        private void UiMediator_Notification(object sender, JobEventArgs e)
        {
            if ((e.NotificationType & NotificationTypes.JobStarted) == NotificationTypes.JobStarted)
            {
               
            }
            if ((e.NotificationType & NotificationTypes.JobEnded) == NotificationTypes.JobEnded)
            {
                
            }
            if ((e.NotificationType & NotificationTypes.Progress) == NotificationTypes.Progress)
            {
                
            }
            if ((e.NotificationType & NotificationTypes.Notification) == NotificationTypes.Notification)
            {
               
            }
            if ((e.NotificationType & NotificationTypes.Error) == NotificationTypes.Error)
            {
                
            }
        }
    }
}
