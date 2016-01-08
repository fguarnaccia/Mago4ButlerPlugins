using System;

namespace Microarea.Mago4Butler.BL
{
    public class InstanceEventArgs : EventArgs
    {
        public Instance Instance { get; set; }
        public string MsiFullFilePath { get; set; }
    }
}