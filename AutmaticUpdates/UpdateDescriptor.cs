using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.AutomaticUpdates
{
    class UpdateDescriptor
    {
        public string Name { get; set; }
        public string[] FileNames { get; set; }
        public Int64 Version { get; set; }
        public type Type { get; set; }

        public static UpdateDescriptor From(update upd, string updatesManifestPath)
        {
            if (upd.fileNames == null || upd.fileNames.Count() == 0)
            {
                return new UpdateDescriptor() { Name = upd.name, FileNames = new string[] { }, Version = 0, Type = upd.type };
            }
            var updatesFolderPath = Path.GetDirectoryName(updatesManifestPath);
            foreach (var fn in upd.fileNames)
            {
                var fi = new FileInfo(Path.Combine(updatesFolderPath, upd.fileNames.First().name));
                if (fi.Exists)
                {
                    Int64 remoteVer = fi.LastWriteTimeUtc.Ticks;
                    return new UpdateDescriptor() { Name = upd.name, FileNames = upd.fileNames.Select(f => f.name).ToArray(), Version = remoteVer, Type = upd.type };
                }
            }

            return new UpdateDescriptor() { Name = upd.name, FileNames = new string[] { }, Version = 0, Type = upd.type };
        }
    }
}
