using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class CompanyDBUpdateService : ILogger
    {
        string rootFolder;
        IAdmConsoleToLaunchNameProvider admConsoleToLaunchNameProvider;

        public CompanyDBUpdateService(ISettings settings, IAdmConsoleToLaunchNameProvider admConsoleToLaunchNameProvider)
        {
            this.rootFolder = settings.RootFolder;
            this.admConsoleToLaunchNameProvider = admConsoleToLaunchNameProvider;
        }

        public void UpdateCompanyDB(Instance instance)
        {
            string fileNameToLaunch = null;
            try
            {
                fileNameToLaunch = admConsoleToLaunchNameProvider.GetFileNameToLaunch();

                this.LogInfo("Starting " + fileNameToLaunch + " with command line parameters /autologin yes /UpgradeAllCompaniesAndExit yes");
                string consoleExePath = Path.Combine(
                        this.rootFolder,
                        instance.Name,
                        "Apps",
                        "Publish",
                        fileNameToLaunch
                        );

                this.LaunchProcess(
                    consoleExePath,
                    "/autologin yes /UpgradeAllCompaniesAndExit yes",
                    3600000
                    );
                this.LogInfo(fileNameToLaunch + " terminated successfully");

            }
            catch (Exception exc)
            {
                if (string.IsNullOrWhiteSpace(fileNameToLaunch))
                {
                    fileNameToLaunch = "Administration Console or Administration Console Lite";
                }
                this.LogError("Error executing " + fileNameToLaunch, exc);
            }
        }
    }
}
