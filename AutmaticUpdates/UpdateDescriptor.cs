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
        public string[] FileNames { get; set; }
        public Version Version { get; set; }
        public type Type { get; set; }

        public static UpdateDescriptor From(update upd)
        {
            var remoteVer = new System.Version(0, 0, 0, 0);
            if (!System.Version.TryParse(upd.version, out remoteVer))
            {
                return null;
            }
            return new UpdateDescriptor() { Name = upd.name, FileNames = upd.fileNames.Select(f => f.name).ToArray(), Version = remoteVer, Type = upd.type };
        }
    }
}
