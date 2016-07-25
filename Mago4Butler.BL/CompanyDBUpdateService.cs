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

        public CompanyDBUpdateService(ISettings settings)
        {
            this.rootFolder = settings.RootFolder;
        }

        public void UpdateCompanyDB(Instance instance)
        {
            try
            {
                this.LogInfo("Starting AdministrationConsoleLite.exe with command line parameters /autologin yes /UpgradeAllCompaniesAndExit yes");
                string consoleExePath = Path.Combine(
                        this.rootFolder,
                        instance.Name,
                        "Apps",
                        "Publish",
                        "AdministrationConsoleLite.exe"
                        );

                this.LaunchProcess(
                    consoleExePath,
                    "/autologin yes /UpgradeAllCompaniesAndExit yes",
                    3600000
                    );
                this.LogInfo("AdministrationConsoleLite.exe terminated successfully");

            }
            catch (Exception exc)
            {
                this.LogError("Error executing AdministrationConsoleLite.exe", exc);
            }
        }
    }
}
