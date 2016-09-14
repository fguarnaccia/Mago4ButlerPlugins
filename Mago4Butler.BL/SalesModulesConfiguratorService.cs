using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class SalesModulesConfiguratorService : ISalesModulesConfiguratorService, ILogger
    {
        class Application
        {
            public string Name { get; set; }
            public List<Module> Modules { get; set; } = new List<Module>();
        }

        class Module
        {
            public string Name { get; set; }
        }


        const string csmFileExtension = "csm";
        static List<Application> applications = InitApplications();

        private static List<Application> InitApplications()
        {
            var app = new Application() { Name="TBF" };
            app.Modules.Add(new Module() { Name= "JobScheduler" });

            return new List<Application>() { app };
        }

        readonly ISettings settings;
        readonly IisService iisService;

        public SalesModulesConfiguratorService(ISettings settings, IisService iisService)
        {
            this.settings = settings;
            this.iisService = iisService;
        }
        public void ConfigureSalesModules(Instance instance)
        {
            var applicationsFolderPath = Path.Combine(this.settings.RootFolder, instance.Name, "Standard", "Applications");

            string modulesFolderPath = null;
            string moduleFileFullPath = null;
            FileInfo moduleFileInfo = null;
            foreach (var application in applications)
            {
                modulesFolderPath = Path.Combine(applicationsFolderPath, application.Name, "Solutions", "Modules");

                foreach (var module in application.Modules)
                {
                    moduleFileFullPath = Path.Combine(
                        modulesFolderPath,
                        string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", application.Name, module.Name, csmFileExtension)
                        );

                    moduleFileInfo = new FileInfo(moduleFileFullPath);
                    if (moduleFileInfo.Exists)
                    {
                        try
                        {
                            moduleFileInfo.Delete();
                        }
                        catch (Exception exc)
                        {
                            this.LogError("Exception deleting " + moduleFileFullPath, exc);
                        }
                    }
                    else
                    {
                        this.LogInfo(moduleFileFullPath + " does not exist");
                    }
                }
            }

            this.iisService.RestartLoginManager(instance);
        }
    }
}
