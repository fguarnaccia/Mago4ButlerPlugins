using Microarea.Mago4Butler.BL;
using System.IO;
using System.IO.Pipes;
using System;
using System.Collections.Generic;

namespace Microarea.Mago4Butler.Plugins
{
    public class App : IDisposable, ILogger
    {
        static App _app;
        public static App Instance
        {
            get
            {
                if (_app == null)
                {
                    _app = new App();
                }
                return _app;
            }
        }

        AppAutomationClient appAutomation;
        protected App()
        {
            this.appAutomation = new AppAutomationClient();
        }

        public Settings Settings
        {
            get
            {
                return new Settings() { RootFolder = BL.Settings.Default.RootFolder };
            }
        }

        public void ShutdownApplication()
        {
            this.appAutomation.ShutdownApplication();
        }

        public System.Windows.Forms.DialogResult ShowModalForm(Type formType)
        {
            System.Windows.Forms.Form activeForm = System.Windows.Forms.Form.ActiveForm;
            if (activeForm == null)
            {
                foreach (System.Windows.Forms.Form form in System.Windows.Forms.Application.OpenForms)
                {
                    activeForm = form;
                    if (activeForm != null)
                    {
                        break;
                    }
                }
            }

            System.Windows.Forms.DialogResult res = System.Windows.Forms.DialogResult.None;
            activeForm.Invoke(new Action(
                ()
                =>
                {
                    using (var modalForm = Activator.CreateInstance(formType) as System.Windows.Forms.Form)
                    {
                        res = modalForm.ShowDialog(activeForm);
                    }
                }
                ));

            return res;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool managed)
        {
            if (managed)
            {
                if (this.appAutomation != null)
                {
                    this.appAutomation.Dispose();
                    this.appAutomation = null;
                }
            }
        }

        public System.Version GetVersion(string pluginName)
        {
            return this.appAutomation.GetPluginVersion(pluginName);
        }

        public string GetPluginFolderPath()
        {
            return this.appAutomation.GetPluginFolderPath();
        }

        public void Error(string errorMessage, Exception exc = null)
        {
            this.LogError(errorMessage, exc);
        }
        public void Info(string message)
        {
            this.LogInfo(message);
        }
    }
}
