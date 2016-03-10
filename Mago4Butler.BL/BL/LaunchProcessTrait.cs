using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microarea.Mago4Butler.BL
{
    public static class LaunchProcessTrait
    {
        public static void LaunchProcess(this MsiZapper @this, string processFilePath, string args, int timeoutInMillSecs)
        {
            LaunchProcess(processFilePath, args, timeoutInMillSecs);
        }
        public static void LaunchProcess(this InstallerService @this, string processFilePath, string args, int timeoutInMillSecs)
        {
            LaunchProcess(processFilePath, args, timeoutInMillSecs);
        }
        public static void LaunchProcess(this IProvisioningService @this, string processFilePath, string args, int timeoutInMillSecs)
        {
            LaunchProcess(processFilePath, args, timeoutInMillSecs);
        }
        public static void LaunchProcess(this CompanyDBUpdateService @this, string processFilePath, string args, int timeoutInMillSecs)
        {
            LaunchProcess(processFilePath, args, timeoutInMillSecs);
        }
        private static void LaunchProcess(string processFilePath, string args, int timeoutInMillSecs)
        {
            ProcessStartInfo psi = new ProcessStartInfo(processFilePath, args);
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            Process p = Process.Start(psi);
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();

            p.WaitForExit(timeoutInMillSecs);
            if (p.ExitCode != 0)
            {
                throw new Exception(String.Format("Process '{0}' returned following errors: {1}, {2}", processFilePath, output, error));
            }
        }
    }
}
