using Microarea.Mago4Butler.Model;
using System;

namespace Microarea.Mago4Butler.BL
{
    public class InstallInstanceEventArgs : EventArgs
    {
        public CmdLineInfo CmdLineInfo { get; internal set; }
        public Instance Instance { get; set; }
    }
}