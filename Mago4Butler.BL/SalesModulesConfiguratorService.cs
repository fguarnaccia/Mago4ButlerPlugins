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
        const string csmFileExtension = "csm";
        static List<Application> applications = InitApplications();

        private static List<Application> InitApplications()
        {
            var app = new Application() { Name="TBF", AppFolderName = "TBF" };
            app.Modules.Add(new Module() { Name= "JobScheduler" });

            var erpProApp = new Application() { Name = "ERP-Pro", AppFolderName="ERP" };
            erpProApp.Modules.Add(new Module() { Name= "Basel_II" });
            erpProApp.Modules.Add(new Module() { Name= "BEOConnector" });
            erpProApp.Modules.Add(new Module() { Name= "IBConnector" });
            erpProApp.Modules.Add(new Module() { Name= "InfiniteCRM" });
            erpProApp.Modules.Add(new Module() { Name= "InfinityConnector" });
            erpProApp.Modules.Add(new Module() { Name= "TESANConnector" });
            erpProApp.Modules.Add(new Module() { Name= "IMago" });
            erpProApp.Modules.Add(new Module() { Name= "Manufacturing Mobile" });
            erpProApp.Modules.Add(new Module() { Name= "WMS Mobile" });

            var erpEntApp = new Application() { Name = "ERP-Ent", AppFolderName = "ERP" };
            erpEntApp.Modules.Add(new Module() { Name = "IBConnector" });
            erpEntApp.Modules.Add(new Module() { Name = "InfiniteCRM" });
            erpEntApp.Modules.Add(new Module() { Name = "InfinityConnector" });
            erpEntApp.Modules.Add(new Module() { Name = "TESANConnector" });
            erpEntApp.Modules.Add(new Module() { Name = "IMago" });

            return new List<Application>() { app, erpProApp, erpEntApp };
        }

        readonly ISettings settings;

        public SalesModulesConfiguratorService(ISettings settings)
        {
            this.settings = settings;
        }
        public void ConfigureSalesModules(Instance instance)
        {
            var applicationsFolderPath = Path.Combine(this.settings.RootFolder, instance.Name, "Standard", "Applications");

            string modulesFolderPath = null;
            string moduleFileFullPath = null;
            FileInfo moduleFileInfo = null;
            foreach (var application in applications)
            {
                modulesFolderPath = Path.Combine(applicationsFolderPath, application.AppFolderName, "Solutions", "Modules");

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
        }
    }
}
