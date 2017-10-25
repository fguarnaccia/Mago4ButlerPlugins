using AutoMapper;
using Microarea.Mago4Butler.Automation;
using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        const string instanceNameRegexPattern = "^[\\-a-zA-Z0-9]+$";
        readonly Regex instanceNameRegex = new Regex(instanceNameRegexPattern);

        MapperConfiguration settingsMapperConfig;
        IMapper settingsLineMapper;

        protected App()
        {
            this.settingsMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BL.Settings, Settings>();
            });

            this.settingsLineMapper = settingsMapperConfig.CreateMapper();
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
                //return new Settings() { RootFolder = BL.Settings.Default.RootFolder, MsiFolder = BL.Settings.Default.MsiFolder, LogsFolder = BL.Settings.Default.LogsFolder };
                return settingsLineMapper.Map<Settings>(BL.Settings.Default);
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

        public string[] GetPluginsData()
        {
            return this.appAutomation.GetPluginsData();
        }

        public void Error(string errorMessage, Exception exc = null)
        {
            this.LogError(errorMessage, exc);
        }
        public void Info(string message)
        {
            this.LogInfo(message);
        }

        public void DownloadMsi(string msiUri, string localMsiPath)
        {
            var httpService = new HttpService(Microarea.Mago4Butler.BL.Settings.Default);

            httpService.DownloadFile(msiUri, localMsiPath);
        }

        [Obsolete("Use InstallMsi(string msiFilePath, string masterProductUpgradeCode, string masterProductName, string instance)")]
        public void InstallMsi(string msiFilePath)
        {
            InstallMsi(msiFilePath, null, null, null);
        }

        public void InstallMsi(string msiFilePath, string masterProductUpgradeCode, string masterProductName, string instance)
        {
            Task.Factory.StartNew(()
                =>
            {
                var argsBuilder = new StringBuilder();

                argsBuilder
                    .Append("/i")
                    .Append(" \"")
                    .Append(msiFilePath)
                    .Append("\"");

                if (BL.Settings.Default.MsiLog)
                {
                    argsBuilder
                        .Append(" /lv*x")
                        .Append(" \"")
                        .Append(Path.Combine(BL.Settings.Default.LogsFolder, Path.GetFileNameWithoutExtension(msiFilePath) + ".log"))
                        .Append("\"");
                }

                LaunchProcessTrait.LaunchProcess(null as MsiService, MsiService.msiexecPath, argsBuilder.ToString(), 12000);

                //Rimuovo le informazioni di installazione dal registry se presenti in
                //modo che la mia installazione non le trovi e tenga i parametri che passo io da riga di comando.
                var msiService = new MsiService(null, null, null, null, null);
                var msiZapper = new MsiZapper();
                msiZapper.ZapMsi(msiFilePath, msiService.GetProductCode(msiFilePath));

                if (
                    !string.IsNullOrWhiteSpace(masterProductUpgradeCode) &&
                    !string.IsNullOrWhiteSpace(masterProductName) &&
                    !string.IsNullOrWhiteSpace(instance)
                    )
                {
                    var cleanMasterProductName = masterProductName
                        .Replace(".", string.Empty)
                        .Trim();
                    var registryService = new RegistryService();
                    registryService.RemoveInstallationInfoKey(
                            string.Empty,
                            masterProductUpgradeCode,
                            cleanMasterProductName
                            );
                    registryService.RemoveInstallationInfoKey(
                            string.Empty,
                            msiService.GetUpgradeCode(msiFilePath),
                            cleanMasterProductName
                            );
                    registryService.RemoveInstallerFoldersKeys(Settings.RootFolder, instance);
                }
            });
        }

        public bool IsInstanceNameValid(string instanceName)
        {
            return instanceNameRegex.IsMatch(instanceName);
        }
    }
}
