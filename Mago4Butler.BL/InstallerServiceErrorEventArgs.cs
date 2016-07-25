using System;

namespace Microarea.Mago4Butler.BL
{
    public class InstallerServiceErrorEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Exception Error { get; set; }
    }
}