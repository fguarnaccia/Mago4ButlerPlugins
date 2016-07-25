using Microarea.Mago4Butler.Model;

namespace Microarea.Mago4Butler.BL
{
    public interface IProvisioningService
    {
        bool ShouldStartProvisioning { get; }
        void StartProvisioning(Instance instance);
    }
}