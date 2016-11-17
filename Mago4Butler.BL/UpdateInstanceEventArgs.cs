using Microarea.Mago4Butler.Model;
using System;
using System.Collections.Generic;

namespace Microarea.Mago4Butler.BL
{
    public class UpdateInstanceEventArgs : EventArgs
    {
        public CmdLineInfo CmdLineInfo { get; internal set; }
        public Instance[] Instances { get; internal set; }
    }
}