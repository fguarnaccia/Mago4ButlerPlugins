using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class FileSystemService : ILogger
    {
        ISettings settings;

        public FileSystemService(ISettings settings)
        {
            this.settings = settings;
        }

        public void RemoveAllFiles(Instance instance)
        {
            var instanceRootFolder = new DirectoryInfo(Path.Combine(this.settings.RootFolder, instance.Name));

            try
            {
                if (this.settings.AlsoDeleteCustom)
                {
                    DeleteDirectory(instanceRootFolder);
                }
                else
                {
                    DeleteDirectory(instanceRootFolder, "Custom", "App_Data");
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error deleting " + instanceRootFolder, exc);
            }
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
                DeleteDirectory(subDir, tokensToBeSkipped);
            }

            try
            {
                dirInfo.Delete();
            }
            catch
            {}
        }
    }
}
