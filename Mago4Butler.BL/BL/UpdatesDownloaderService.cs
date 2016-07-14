using Microarea.Mago4Butler.BL.PAASUpdates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class UpdatesDownloaderService : ILogger
    {
        readonly HttpService httpService;
        readonly MsiService msiService;
        readonly ISettings settings;

        public UpdatesDownloaderService(MsiService msiService, ISettings settings, HttpService httpService)
        {
            this.msiService = msiService;
            this.settings = settings;
            this.httpService = httpService;
        }
        public void DownloadUpdatedMsiIfAny()
        {
            var versions = new List<Version>();

            //Estraggo la versione dall'msi corrente...
            var msiFolder = this.settings.MsiFolder;
            var msiFiles = Directory.GetFiles(msiFolder, "*.msi");
            foreach (var msiFile in msiFiles)
            {
                try
                {
                    versions.Add(this.msiService.GetVersion(msiFile));
                }
                catch (Exception exc)
                {
                    this.LogError("Error reading version from " + msiFile, exc);
                }
            }
            var version = new Version();
            if (versions.Count > 0)
            {
                versions.Sort();
                version = versions.Last();
                this.LogInfo("Current version is " + version.ToString());
            }
            else
            {
                this.LogInfo("No msi file are present in " + msiFolder + ", I'm going to download the first one");
            }

            //...richiedo al server se ci sono versioni piu` aggiornate...
            GetUpdatesResponse response = this.httpService.GetUpdates(version);

            //...se non ci sono allora esco...
            if (!response.UpdatesAvailable)
            {
                this.LogInfo("No updates available");
                return;
            }

            //...altrimenti ho ricevuto dal server il link per il download del nuovo msi
            //(es: http://www.microarea.it/Downloads/MAGO4/it-IT/MAGO4-1.1.2(build0055-20160704).msi)
            //lo scarico...
            var tempDownloadFilePath = Path.Combine(Path.GetTempPath(), response.MsiFileName);
            this.LogInfo("Updates available, downloading from " + response.DownloadUri + " to " + tempDownloadFilePath);

            try
            {
                this.httpService.DownloadFile(response.DownloadUri, tempDownloadFilePath);
                this.LogInfo("Msi file successfully downloaded");
            }
            catch (Exception exc)
            {
                this.LogError("Error downloading msi file", exc);
                throw;
            }

            //...cancello i vecchi...
            this.LogInfo("I'm going to back up old msi files");
            foreach (var msiFile in msiFiles)
            {
                try
                {
                    var bakFileName = string.Concat(msiFile, ".bak");
                    if (File.Exists(bakFileName))
                    {
                        File.Delete(bakFileName);
                    }
                    File.Move(msiFile, bakFileName);
                }
                catch (Exception exc)
                {
                    this.LogError("Error backing up msi file " + msiFile, exc);
                    throw;
                }
            }
            this.LogInfo("Old msi files backed up");

            //...e sposto il nuovo appena scaricato nella cartella MSI
            var destinationPath = Path.Combine(msiFolder, response.MsiFileName);
            try
            {
                this.LogInfo("I'm going to move the downloaded file from " + tempDownloadFilePath + " to " + destinationPath);
                File.Move(tempDownloadFilePath, destinationPath);
                this.LogInfo("File successfully moved");
            }
            catch (Exception exc)
            {
                this.LogError("Error moving msi file from " + tempDownloadFilePath + " to " + destinationPath, exc);
                throw;
            }
        }
    }
}
