using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microarea.Mago4Butler.Model;
using System.IO;
using Microarea.Mago4Butler.Log;

namespace Microarea.Mago4Butler.BL
{
    public class ProductIdentifierService : ILogger
    {
        ISettings settings;

        public ProductIdentifierService(ISettings settings)
        {
            this.settings = settings;
        }
        public bool IsMagoNet(string msiFilename)
        {
            return msiFilename.StartsWith(ProductType.Magonet.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsMago4(string msiFilename)
        {
            return msiFilename.StartsWith(ProductType.Mago4.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsMagoNet(Instance instance)
        {
            var installationVerPath = GetInstallationVer(instance.Name);

            if (!installationVerPath.Exists)
            {
                this.LogError(installationVerPath.FullName + " does not exists, I cannot state if it is a Mago.net installation");
                return false;
            }

            var content = ReadFileContent(installationVerPath);

            return content.IndexOf("<ProductName>Magonet</ProductName>", StringComparison.OrdinalIgnoreCase) != -1;
        }

        public bool IsMago4(Instance instance)
        {
            var installationVerPath = GetInstallationVer(instance.Name);

            if (!installationVerPath.Exists)
            {
                this.LogError(installationVerPath.FullName + " does not exists, I cannot state if it is a Mago.net installation");
                return false;
            }

            var content = ReadFileContent(installationVerPath);

            return content.IndexOf("<ProductName>Mago4</ProductName>", StringComparison.OrdinalIgnoreCase) != -1;
        }

        private static string ReadFileContent(FileInfo installationVerPath)
        {
            return new StreamReader(File.OpenRead(installationVerPath.FullName)).ReadToEnd();
        }

        private FileInfo GetInstallationVer(string instanceName)
        {
            return new FileInfo(Path.Combine(settings.RootFolder, instanceName, "Standard", "Installation.ver"));
        }
    }
}
