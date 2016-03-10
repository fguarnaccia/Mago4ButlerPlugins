namespace Microarea.Mago4Butler.BL
{
    public interface IProvisioningService
    {
        bool ShouldStartProvisioning { get; }
        void StartProvisioning(Instance instance);
    }
}