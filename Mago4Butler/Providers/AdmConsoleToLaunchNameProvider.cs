using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    public class AdmConsoleToLaunchNameProvider : IAdmConsoleToLaunchNameProvider
    {
        ShouldUseProvisioningProvider shouldUseProvisioningProvider;
        public AdmConsoleToLaunchNameProvider(ShouldUseProvisioningProvider shouldUseProvisioningProvider)
        {
            this.shouldUseProvisioningProvider = shouldUseProvisioningProvider;
        }
        public string GetFileNameToLaunch()
        {
            return shouldUseProvisioningProvider.ShouldUseProvisioning ? "AdministrationConsoleLite.exe" : "AdministrationConsole.exe";
        }
    }
}
