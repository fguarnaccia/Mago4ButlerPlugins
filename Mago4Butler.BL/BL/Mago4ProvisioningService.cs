using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class Mago4ProvisioningService : ILogger, IProvisioningService
    {
        string rootFolder;

        public bool ShouldStartProvisioning { get { return true; } }

        public Mago4ProvisioningService(ISettings settings)
        {
            this.rootFolder = settings.RootFolder;
        }

        public void StartProvisioning(Instance instance)
        {
            try
            {
                string provisioningExePath = Path.Combine(
                        this.rootFolder,
                        instance.Name,
                        "Apps",
                        "ProvisioningConfigurator",
                        "ProvisioningConfiguratorLauncher.exe"
                        );

                this.LaunchProcess(
                    provisioningExePath,
                    instance.ProvisioningCommandLine,
                    3600000
                    );
            }
            catch (Exception exc)
            {
                this.LogError("Error starting provisioning configurator for " + instance.Name, exc);
            }
        }
    }
}
