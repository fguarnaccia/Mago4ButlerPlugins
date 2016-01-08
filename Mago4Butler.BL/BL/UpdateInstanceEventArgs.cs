using System;
using System.Collections.Generic;

namespace Microarea.Mago4Butler.BL
{
    public class UpdateInstanceEventArgs : EventArgs
    {
        public IList<Instance> Instances { get; internal set; }
    }
}