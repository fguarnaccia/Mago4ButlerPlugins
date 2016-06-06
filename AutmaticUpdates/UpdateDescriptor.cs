using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.AutomaticUpdates
{
    class UpdateDescriptor
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public Version Version { get; set; }
        public string DownloadedFilePath { get; set; }
    }
}
