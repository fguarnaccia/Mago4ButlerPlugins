using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class CompanyDBUpdateService
    {
        string rootFolder;

        public CompanyDBUpdateService(ISettings settings)
        {
            this.rootFolder = settings.RootFolder;
        }

        public void UpdateCompanyDB(Instance instance)
        {
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
        }
    }
}
