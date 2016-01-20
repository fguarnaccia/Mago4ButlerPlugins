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

            DeleteDirectory(instanceRootFolder);
        }

        static void DeleteDirectory(DirectoryInfo dirInfo)
        {
            var files = dirInfo.GetFiles();
            var subDirs = dirInfo.GetDirectories();

            foreach (var file in files)
            {
                file.Attributes |= FileAttributes.Normal;
                File.Delete(file.FullName);
            }

            foreach (var subDir in subDirs)
            {
                DeleteDirectory(subDir);
            }

            dirInfo.Delete();
        }
    }
}
