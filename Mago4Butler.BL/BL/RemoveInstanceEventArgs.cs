using System;
using System.Collections.Generic;

namespace Microarea.Mago4Butler.BL
{
    public class RemoveInstanceEventArgs : EventArgs
    {
        public IList<string> InstanceNames { get; internal set; }
    }
}