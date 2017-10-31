using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.DustmanButler
{
    public class DustmanButler : Mago4ButlerPlugin
    {
        public override void OnApplicationStarted()
        {
            var thread = new Thread(() => CollectDust())
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void CollectDust()
        {
            try
            {
                Thread.Sleep(1379);
                DateTime yesterday = DateTime.Now.AddDays(-1).Date;
                DeleteMsiFilesOlderThan(yesterday);
                DeleteLogFilesOlderThan(yesterday);

            }
            catch (Exception exc)
            {
                App.Instance.Error("Error collecting dust", exc);
            }
        }

        private void DeleteMsiFilesOlderThan(DateTime threshold)
        {
            var msiFolderDirInfo = new DirectoryInfo(App.Instance.Settings.MsiFolder);
            if (!msiFolderDirInfo.Exists)
            {
                return;
            }

            var toBeDeleted = msiFolderDirInfo.
                GetFiles("*.msi", SearchOption.TopDirectoryOnly)
                .Where(
                    f
                    => f.Name.StartsWith("mago", StringComparison.InvariantCultureIgnoreCase)
                        && f.CreationTime.Date < threshold
                        );
            foreach (var msiFile in toBeDeleted)
            {
                try
                {
                    msiFile.Delete();
                }
                catch (Exception exc)
                {
                    App.Instance.Error(string.Format(CultureInfo.InvariantCulture, "Error deleting {0}", msiFile), exc);
                }
            }
        }

        private void DeleteLogFilesOlderThan(DateTime threshold)
        {
            var logsFolderDirInfo = new DirectoryInfo(App.Instance.Settings.LogsFolder);
            if (!logsFolderDirInfo.Exists)
            {
                return;
            }

            foreach (var logFile in logsFolderDirInfo.GetFiles("*.log", SearchOption.TopDirectoryOnly))
            {
                if (logFile.CreationTime.Date < threshold)
                {
                    try
                    {
                        logFile.Delete();
                    }
                    catch (Exception exc)
                    {
                        App.Instance.Error(string.Format(CultureInfo.InvariantCulture, "Error deleting {0}", logFile), exc);
                    }
                }
            }
        }
    }
}
