﻿using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Plugins
{
    public interface IPlugin
    {
        IEnumerable<ContextMenuItem> GetContextMenuItems();
    }
}