using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.AutomaticUpdates
{
    class UpdatesInfo
    {
        public IList<UpdateDescriptor> Updates { get; set; } = new List<UpdateDescriptor>();
        public bool RestartRequired { get; set; }
        public bool UpdatesAvailable { get { return Updates.Count > 0; } }
    }
}
