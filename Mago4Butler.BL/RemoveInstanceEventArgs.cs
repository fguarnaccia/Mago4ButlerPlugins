﻿using Microarea.Mago4Butler.Model;
using System;
using System.Collections.Generic;

namespace Microarea.Mago4Butler.BL
{
    public class RemoveInstanceEventArgs : EventArgs
    {
        public Instance[] Instances { get; internal set; }
    }
}