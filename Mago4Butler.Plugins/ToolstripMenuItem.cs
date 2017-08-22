using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Plugins
{
    public class ToolstripMenuItem
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Action Command { get; set; }
    }
}
