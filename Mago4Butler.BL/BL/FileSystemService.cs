using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class FileSystemService
    {
        string rootFolder;

        public FileSystemService(string rootFolder)
        {
            this.rootFolder = rootFolder;
        }

        public void RemoveAllFiles(Instance instance)
        {
            var instanceRootFolder = new DirectoryInfo(Path.Combine(this.rootFolder, instance.Name));
            instanceRootFolder.Delete(true);
        }
    }
}
