using System;

namespace Microarea.Mago4Butler
{
    public class ProvisioningEventArgs : EventArgs
    {
        public string InstanceName { get; set; }
        public string ProvisioningCommandLine { get; set; }
        public bool Cancel { get; set; }
    }
}