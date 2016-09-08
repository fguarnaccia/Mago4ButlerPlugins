using Microarea.Mago4Butler.Automation;
using Microarea.Mago4Butler.Log;
using System;
using System.Windows.Forms;

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

        AppAutomation appAutomation;
        protected App()
        {
            
        }
        public void Init()
        {
            if (this.appAutomation != null)
            {
                Dispose();
            }
            this.appAutomation = new AppAutomation();
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

        public DialogResult ShowModalForm(Form modalForm)
        {
            var activeForm = Form.ActiveForm;
            if (activeForm == null)
            {
                foreach (Form form in Application.OpenForms)
                {
                    activeForm = form;
                    if (activeForm != null)
                    {
                        break;
                    }
                }
            }

            DialogResult res = DialogResult.None;
            activeForm.Invoke(new Action(
                ()
                =>
                {
                    res = modalForm.ShowDialog(activeForm);
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

        public string[] GetInstances()
        {
            return this.appAutomation.GetInstances();
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
