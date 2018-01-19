using Microarea.Mago4Butler.Plugins;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System;
using System.IO;
using System.Linq;

namespace Microarea.Mago4Butler.AutomaticUpdates
{
    public class AutomaticUpdates : Mago4ButlerPlugin
    {
        readonly string localCacheForUpdates = Path.GetTempPath();
        const string updatesUri = @"\\srv-bks\Download\Tools Microarea\Mago4ButlerUpdates";
        const string updatesManifestFileName = "Updates.xml";

        bool canUpdate;

        public override void OnApplicationStarted()
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
                    using (var modalForm = new AskForUpdate())
                    {
                        var dr = App.Instance.ShowModalForm(modalForm);
                        if (dr != System.Windows.Forms.DialogResult.Yes)
                        {
                            return;
                        }
                    }
                }

                string pluginsFolderPath = App.Instance.GetPluginFolderPath();
                string msiUpdateFileFullPath = null;
                string exeUpdateFileFullPath = null;
                foreach (var update in updates.Updates)
                {
                    if (update.Type == type.dll)
                    {
                        try
                        {
                            foreach (var file in update.FileNames)
                            {
                                File.Copy(Path.Combine(localCacheForUpdates, file), Path.Combine(pluginsFolderPath, file), true);
                            }
                        }
                        catch (Exception exc)
                        {
                            App.Instance.Error("Error during the update process of " + update.Name, exc);
                            continue;
                        }
                    }
                    else if (update.Type == type.msi)
                    {
                        foreach (var file in update.FileNames)
                        {
                            //Lo so lo so....ma un update con msi e` costituito da un solo file...per adesso
                            msiUpdateFileFullPath = Path.Combine(localCacheForUpdates, file);
                        }
                    }
                    else if (update.Type == type.exe)
                    {
                        foreach (var file in update.FileNames)
                        {
                            //Anche ui...lo so....ma anche un update con exe e` costituito da un solo file...per adesso
                            exeUpdateFileFullPath = Path.Combine(localCacheForUpdates, file);
                        }
                    }
                }

                if (msiUpdateFileFullPath != null)
                {
                    ThreadPool.QueueUserWorkItem((_) => Process.Start(msiUpdateFileFullPath));

                    Thread.Sleep(500);

                    App.Instance.ShutdownApplication();
                }
                else if (exeUpdateFileFullPath != null)
                {
                    ThreadPool.QueueUserWorkItem((_) => Process.Start(exeUpdateFileFullPath));

                    Thread.Sleep(500);

                    App.Instance.ShutdownApplication();
                }
            }
            catch (Exception exc)
            {
                App.Instance.Error("Error during update", exc);
            }
        }

        private UpdatesInfo DownloadUpdates(IDictionary<string, UpdateDescriptor> updateDescriptorsCache)
        {
            var updates = new UpdatesInfo();
            foreach (var updateDescriptor in updateDescriptorsCache.Values)
            {
                var version = App.Instance.GetVersion(updateDescriptor.Name);
                //Se versione e` nulla significa che il pacchetto non e` installato localmente, lo installo.
                if (updateDescriptor.Version > version)
                {
                    updates.Updates.Add(updateDescriptor);
                    try
                    {
                        foreach (var file in updateDescriptor.FileNames)
                        {
                            File.Copy(Path.Combine(updatesUri, file), Path.Combine(localCacheForUpdates, file), overwrite: true);
                            if (updateDescriptor.Type == type.msi)
                            {
                                updates.RestartRequired = true;
                            }
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
                var upds = updates.Load(updatesManifestPath);
                if (upds == null)
                {
                    App.Instance.Error("Unable to load updates file from: " + updatesManifestPath);
                    return remoteVersions;
                }
                foreach (var upd in upds.Items)
                {
                    remoteVersions.Add(upd.name, UpdateDescriptor.From(upd, updatesManifestPath));
                }
            }
            catch (Exception exc)
            {
                App.Instance.Error("Error checking for updates", exc);
            }

            return remoteVersions;
        }

        public override void OnInstallerServiceStopped()
        {
            canUpdate = true;
        }

        public override void OnInstallerServiceStarted()
        {
            canUpdate = false;
        }
    }
}
