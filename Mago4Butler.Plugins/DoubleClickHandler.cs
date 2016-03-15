using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Plugins
{
    public class DoubleClickHandler
    {
        public string Name { get; set; }
        public Action<Instance> Command { get; set; }
    }
}
