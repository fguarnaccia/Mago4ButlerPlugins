using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInstaller;

namespace Microarea.Mago4Butler.BL
{
    public class RegistryService
    {
        MsiService msiService;

        public RegistryService(MsiService msiService)
        {
            this.msiService = msiService;
        }
        public void RemoveInstallationInfoKey(string msiFullPath)
        {
            string upgradeCode = msiService.GetUpgradeCode(msiFullPath);
            if (String.IsNullOrWhiteSpace(upgradeCode))
            {
                throw new ArgumentException(String.Format("'upgradeCode' from {0} is null or empty", msiFullPath));
            }

            string productName =
                msiService.GetProductName(msiFullPath)
                .Replace(".", string.Empty);
            if (String.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException(String.Format("'PRODUCTNAME' from {0} is null or empty", msiFullPath));
            }

            string keyName = String.Format(@"Software\Microarea\{0}\", productName);
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    foreach (var subKeyName in key.GetSubKeyNames())
                    {
                        if (String.Compare(subKeyName, upgradeCode, StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            key.DeleteSubKeyTree(upgradeCode);
                            break;
                        }
                    }
                }
            }
        }

        public void RemoveInstallerFoldersKeys(string rootPath, Instance instance)
        {
            var localMachineKey = GetLocalMachine();

            string keyName = @"Software\Microsoft\Windows\CurrentVersion\Installer\Folders";
            var foldersKey = localMachineKey.OpenSubKey(keyName, true);

            var instanceRootPath = Path.Combine(rootPath, instance.Name);
            foreach (var keyValue in foldersKey.GetValueNames())
            {
                if (keyValue.StartsWith(instanceRootPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    foldersKey.DeleteValue(keyValue);
                }
            }
        }

        private RegistryKey GetLocalMachine()
        {
            RegistryView registryView =
                Environment.Is64BitOperatingSystem ?
                RegistryView.Registry64 :
                RegistryView.Registry32;

            var localMachineKey =
                        RegistryKey.OpenBaseKey(
                            RegistryHive.LocalMachine,
                            registryView
                            );

            return localMachineKey;
        }
    }
}
