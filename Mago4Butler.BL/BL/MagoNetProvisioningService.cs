using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class MagoNetProvisioningService : IProvisioningService
    {
        public bool ShouldStartProvisioning { get { return false; } }
        public void StartProvisioning(Instance instance)
        {
            //do nothing
        }
    }
}
