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
    internal partial class CefForm : Form, IMainUI
    {
        Model.Model model;
        WebMediator webMediator;
        CefFactory cefFactory;
        ChromiumWebBrowser chromeBrowser;

        public CefForm(Model.Model model, CefFactory cefFactory, WebMediator webMediator)
        {
            this.model = model;
            this.webMediator = webMediator;
            this.cefFactory = cefFactory;

            InitializeComponent();

            InitializeChromium();

            this.Text = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} v. {1}", this.Text, this.GetType().Assembly.GetName().Version.ToString());
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!this.webMediator.CanClose)
            {
                e.Cancel = true;
                return;
            }
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

            var schemeHandlerFactory = this.cefFactory.CreateSchemeHandlerFactory();

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeHandlerFactory = schemeHandlerFactory,
                SchemeName = schemeHandlerFactory.GetButlerSchemeName()
            });

            if (!Cef.Initialize(settings))
            {
                throw new Exception("Failed to init view");
            }

            var startPage = string.Format(CultureInfo.InvariantCulture, "{0}://cef/index.html", schemeHandlerFactory.GetButlerSchemeName());
            this.chromeBrowser = new ChromiumWebBrowser(startPage);
            this.chromeBrowser.RegisterJsObject("model", this.model);
            this.chromeBrowser.RegisterJsObject("mediator", this.webMediator);
            this.chromeBrowser.MenuHandler = this.cefFactory.CreateContextMenuHandler();
            this.Controls.Add(chromeBrowser);
            this.chromeBrowser.Dock = DockStyle.Fill;
        }
    }
}
