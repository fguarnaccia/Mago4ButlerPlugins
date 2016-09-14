using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Model
{

    public class Application
    {
        public string Name { get; set; }
        public List<Module> Modules { get; set; } = new List<Module>();
    }

}