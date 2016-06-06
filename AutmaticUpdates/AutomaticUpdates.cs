using Microarea.Mago4Butler.Plugins;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System;
using System.IO;
using System.Linq;

namespace Microarea.Mago4Butler.AutomaticUpdates
{
    public class AutomaticUpdates : IPlugin
    {
        readonly string localCacheForUpdates = Path.GetTempPath();
        const string updatesUri = @"\\srv-bks\Download\Tools Microarea\Mago4ButlerUpdates";
        const string updatesManifestFileName = "Updates.txt";

        bool canUpdate;

        public IEnumerable<ContextMenuItem> GetContextMenuItems()
        {
            return null;
        }

        public DoubleClickHandler GetDoubleClickHandler()
        {
            return null;
        }

        public void OnApplicationStarted()
        {
            canUpdate = true;
            var thread = new Thread(() => InstallAvailableUpdates());
            thread.IsBackground = true;
            thread.Start();
        }

        private void InstallAvailableUpdates()
        {
            try
            {
                Thread.Sleep(5000);

                var remoteVersions = CheckForUpdates();
                var updates = DownloadUpdates(remoteVersions);

                if (updates == null || !updates.UpdatesAvailable)
                {
                    return;
                }

                while (!canUpdate)
                {
                    Thread.Sleep(5000);
                }

                if (updates.RestartRequired)
                {
                    var dr = App.Instance.ShowModalForm(typeof(AskForUpdate));
                    if (dr != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                }

                string pluginsFolderPath = App.Instance.GetPluginFolderPath();
                string msiUpdateFileFullPath = null;
                foreach (var update in updates.Updates)
                {
                    if (update.FileName.EndsWith("dll"))
                    {
                        File.Copy(update.DownloadedFilePath, Path.Combine(pluginsFolderPath, update.FileName), true);
                    }
                    else if (update.FileName.EndsWith("msi"))
                    {
                        msiUpdateFileFullPath = Path.Combine(localCacheForUpdates, update.FileName);
                    }
                }

                if (msiUpdateFileFullPath != null)
                {
                    ThreadPool.QueueUserWorkItem((_) => Process.Start(msiUpdateFileFullPath));

                    Thread.Sleep(500);

                    App.Instance.ShutdownApplication();
                }
            }
            catch
            {
            }
        }

        //Mago4Butler Mago4ButlerSetup.msi 1.0.5930.18522
        //PluginExample.MyButlerPlugin PluginExample.dll 1.1.5931.18522
        private UpdatesInfo DownloadUpdates(IDictionary<string, UpdateDescriptor> updateDescriptorsCache)
        {
            var updates = new UpdatesInfo();
            foreach (var updateDescriptor in updateDescriptorsCache.Values)
            {
                var version = App.Instance.GetVersion(updateDescriptor.Name);
                //Se versione e` nulla significa che il pacchetto non e` installato localmente, lo installo.
                if (version == null || updateDescriptor.Version > version)
                {
                    updates.Updates.Add(updateDescriptor);
                    try
                    {
                        updateDescriptor.DownloadedFilePath = Path.Combine(localCacheForUpdates, updateDescriptor.FileName);
                        File.Copy(Path.Combine(updatesUri, updateDescriptor.FileName), updateDescriptor.DownloadedFilePath, overwrite: true);
                        if (updateDescriptor.FileName.EndsWith("msi"))
                        {
                            updates.RestartRequired = true;
                        }
                    }
                    catch (Exception exc)
                    {
                        App.Instance.Error("Error downloading updates", exc);
                        continue;
                    }
                }
            }

            return updates;
        }

        private IDictionary<string, UpdateDescriptor> CheckForUpdates()
        {
            var remoteVersions = new Dictionary<string, UpdateDescriptor>();
            var updatesManifestPath = Path.Combine(updatesUri, updatesManifestFileName);
            if (!File.Exists(updatesManifestPath))
            {
                return remoteVersions;
            }
            try
            {
                using (var sr = new StreamReader(updatesManifestPath))
                {
                    string remoteVersionStr;
                    while ((remoteVersionStr = sr.ReadLine()) != null)
                    {
                        var entries = remoteVersionStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var entry in entries)
                        {
                            var tokens = entry.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (tokens.Length != 3)
                            {
                                App.Instance.Error("Invalid entry in " + updatesManifestFileName + " file: " + remoteVersionStr);
                                continue;
                            }
                            var remoteVer = new System.Version(0, 0, 0, 0);
                            if (!System.Version.TryParse(tokens[2], out remoteVer))
                            {
                                App.Instance.Error("Unable to parse remote version: " + tokens[1]);
                                continue;
                            }
                            remoteVersions.Add(tokens[0], new UpdateDescriptor() { Name = tokens[0], FileName = tokens[1], Version = remoteVer });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                App.Instance.Error("Error checking for updates", exc);
            }

            return remoteVersions;
        }

        public void OnInstalling(CmdLineInfo cmdLineInfo)
        {
            
        }

        public void OnUpdating(CmdLineInfo cmdLineInfo)
        {
            
        }

        public void OnInstallerServiceStopped()
        {
            canUpdate = true;
        }

        public void OnInstallerServiceStarted()
        {
            canUpdate = false;
        }
    }
}
