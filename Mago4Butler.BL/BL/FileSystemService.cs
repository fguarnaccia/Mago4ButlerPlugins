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

        public FileSystemService(ISettings settings)
        {
            this.rootFolder = settings.RootFolder;
        }

        public void RemoveAllFiles(Instance instance)
        {
            var instanceRootFolder = new DirectoryInfo(Path.Combine(this.rootFolder, instance.Name));

            DeleteDirectory(instanceRootFolder);
        }

        public void RemoveAllFilesButTheCustomAndAppDataFolder(Instance instance)
        {
            var instanceRootFolder = new DirectoryInfo(Path.Combine(this.rootFolder, instance.Name));

            DeleteDirectory(instanceRootFolder, "Custom", "AppData");
        }

        static void DeleteDirectory(DirectoryInfo dirInfo, params string[] tokensToBeSkipped)
        {
            var files = dirInfo.GetFiles();
            var subDirs = dirInfo.GetDirectories();

            foreach (var file in files)
            {
                if (tokensToBeSkipped.Contains(file.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                file.Attributes |= FileAttributes.Normal;
                File.Delete(file.FullName);
            }

            foreach (var subDir in subDirs)
            {
                if (tokensToBeSkipped.Contains(subDir.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                DeleteDirectory(subDir);
            }

            dirInfo.Delete();
        }
    }
}
