using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
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
    internal class RegistryService : ILogger
    {
        public void RemoveTBFPRegistration()
        {
            using (RegistryKey key = Registry.ClassesRoot)
            {
                if (key != null)
                {
                    key.DeleteSubKeyTree("Microarea.SnapInstaller.v1");
                    key.DeleteSubKeyTree(".tbfp");
                }
            }
        }
        public void RemoveInstallationInfoKey(string msiFullPath, string upgradeCode, string productName)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(upgradeCode))
                {
                    throw new ArgumentException(String.Format("'upgradeCode' from {0} is null or empty", msiFullPath));
                }

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
            catch (Exception exc)
            {
                this.LogError("Error removing installation info keys for " + msiFullPath, exc);
            }
        }

        public void RemoveInstallerFoldersKeys(string rootPath, Instance instance)
        {
            try
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
            catch (Exception exc)
            {
                this.LogError("Error removing installation folder keys for " + instance.Name, exc);
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
