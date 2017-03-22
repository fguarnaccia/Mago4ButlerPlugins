using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInstaller;

namespace Microarea.Mago4Butler.BL
{
    internal class MsiService : ILogger
    {
        static readonly Type InstallerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
        const string msiexecPath = @"C:\Windows\System32\msiexec.exe";
        ISettings settings;
        IFileLocker fileLocker;
        MsiZapper msiZapper;
        RegistryService registryService;
        IisService iisService;

        public MsiService(
            ISettings settings,
            IFileLocker fileLocker,
            MsiZapper msiZapper,
            IisService iisService,
            RegistryService registryService
            )
        {
            this.settings = settings;
            this.fileLocker = fileLocker;
            this.msiZapper = msiZapper;
            this.iisService = iisService;
            this.registryService = registryService;
        }

        public string GetProductName(string msiFilePath)
        {
            var productName = GetProductNameInternal(msiFilePath, "PRODUCTNAME");//Mago4
            if (String.IsNullOrWhiteSpace(productName))
            {
                productName = GetProductNameInternal(msiFilePath, "ProductName");//Mago4 1.0.2.123
                if (!String.IsNullOrWhiteSpace(productName))
                {
                    productName = productName.Substring(0, productName.IndexOf(' '));
                }
            }

            return productName;
        }

        private string GetProductNameInternal(string msiFilePath, string propertyToSearchFor)
        {
            try
            {
                var installer = Activator.CreateInstance(InstallerType) as Installer;
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);
                var view = database.OpenView(String.Format(CultureInfo.InvariantCulture, "SELECT * from Property WHERE Property = '{0}'", propertyToSearchFor));

                view.Execute(null);

                Record record = null;
                try
                {
                    record = view.Fetch();
                    return (record != null) ? record.get_StringData(2) : String.Empty;
                }
                finally
                {
                    if (record != null)
                    {
                        Marshal.ReleaseComObject(record);
                    }

                    if (view != null)
                    {
                        view.Close();
                        Marshal.ReleaseComObject(view);
                    }
                    if (database != null)
                    {
                        Marshal.ReleaseComObject(database);
                    }
                    if (installer != null)
                    {
                        Marshal.ReleaseComObject(installer);
                    }
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error getting product name from " + msiFilePath, exc);
                throw;
            }
        }

        public string GetProductCode(string msiFilePath)
        {
            try
            {
                var installer = Activator.CreateInstance(InstallerType) as Installer;
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);
                var view = database.OpenView("SELECT * from Property WHERE Property = 'ProductCode'");

                view.Execute(null);

                Record record = null;
                try
                {
                    record = view.Fetch();
                    return (record != null) ? record.get_StringData(2) : String.Empty;
                }
                finally
                {
                    if (record != null)
                    {
                        Marshal.ReleaseComObject(record);
                    }

                    if (view != null)
                    {
                        view.Close();
                        Marshal.ReleaseComObject(view);
                    }
                    if (database != null)
                    {
                        Marshal.ReleaseComObject(database);
                    }
                    if (installer != null)
                    {
                        Marshal.ReleaseComObject(installer);
                    }
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error getting product code from " + msiFilePath, exc);
                throw;
            }
        }

        public string GetUpgradeCode(string msiFilePath)
        {
            try
            {
                var installer = Activator.CreateInstance(InstallerType) as Installer;
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);
                var view = database.OpenView("SELECT * from Property WHERE Property = 'UpgradeCode'");

                view.Execute(null);

                Record record = null;
                try
                {
                    record = view.Fetch();
                    return (record != null) ? record.get_StringData(2).Trim('{', '}') : String.Empty;
                }
                finally
                {
                    if (record != null)
                    {
                        Marshal.ReleaseComObject(record);
                    }

                    if (view != null)
                    {
                        view.Close();
                        Marshal.ReleaseComObject(view);
                    }
                    if (database != null)
                    {
                        Marshal.ReleaseComObject(database);
                    }
                    if (installer != null)
                    {
                        Marshal.ReleaseComObject(installer);
                    }
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error getting upgrade code from " + msiFilePath, exc);
                throw;
            }
        }

        public Model.Version GetVersion(string msiFilePath)
        {
            try
            {
                string productName = GetProductNameInternal(msiFilePath, "ProductName");

                if (!String.IsNullOrWhiteSpace(productName))
                {
                    try
                    {
                        return Model.Version.Parse(
                                productName.Split(
                                    new char[] { ' ' },
                                    StringSplitOptions.RemoveEmptyEntries
                                    )[1]
                                );
                    }
                    catch (Exception exc)
                    {
                        this.LogError("Error getting version from 'ProductName' property for " + msiFilePath + ", retrying from 'ProductVersion' property ...", exc);
                    }
                }
                return GetProductVersion(msiFilePath);
            }
            catch (Exception exc)
            {
                this.LogError("Error getting version from " + msiFilePath, exc);
                throw;
            }
        }

        private Model.Version GetProductVersion(string msiFilePath)
        {
            try
            {
                var installer = Activator.CreateInstance(InstallerType) as Installer;
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);
                var view = database.OpenView("SELECT * from Property WHERE Property = 'ProductVersion'");

                view.Execute(null);

                Record record = null;
                try
                {
                    record = view.Fetch();
                    var strVersion = (record != null) ? record.get_StringData(2) : String.Empty;
                    if (String.IsNullOrWhiteSpace(strVersion))
                    {
                        return new Model.Version();
                    }
                    return Model.Version.Parse(strVersion);
                }
                finally
                {
                    if (record != null)
                    {
                        Marshal.ReleaseComObject(record);
                    }

                    if (view != null)
                    {
                        view.Close();
                        Marshal.ReleaseComObject(view);
                    }
                    if (database != null)
                    {
                        Marshal.ReleaseComObject(database);
                    }
                    if (installer != null)
                    {
                        Marshal.ReleaseComObject(installer);
                    }
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error getting version from " + msiFilePath, exc);
                throw;
            }
        }

        public IList<Feature> GetFeatureNames(string msiFilePath)
        {
            List<Feature> features = new List<Feature>();
            try
            {
                var installer = Activator.CreateInstance(InstallerType) as Installer;
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);
                var view = database.OpenView("SELECT * from Feature");

                view.Execute(null);

                Record record = null;
                int nameColIdx = 1;
                int descriptionColIdx = 3;
                try
                {
                    record = view.Fetch();
                    while (record != null)
                    {
                        features.Add(
                            new Feature()
                            {
                                Name = record.get_StringData(nameColIdx),
                                Description = record.get_StringData(descriptionColIdx)
                            });
                        Marshal.ReleaseComObject(record);
                        record = view.Fetch();
                    }
                }
                finally
                {
                    if (view != null)
                    {
                        view.Close();
                        Marshal.ReleaseComObject(view);
                    }
                    if (database != null)
                    {
                        Marshal.ReleaseComObject(database);
                    }
                    if (installer != null)
                    {
                        Marshal.ReleaseComObject(installer);
                    }
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error getting feature names from " + msiFilePath, exc);
                throw;
            }
            return features;
        }

        public string CalculateMsiFullFilePath()
        {
            var msiDirInfo = new DirectoryInfo(settings.MsiFolder);
            if (!msiDirInfo.Exists)
            {
                throw new Exception(settings.MsiFolder + " does not exist");
            }

            var msiFileInfos = from FileInfo f in msiDirInfo.GetFiles("Mago*.msi")
                               orderby f.LastWriteTime descending
                               select f;

            if (msiFileInfos.Count() == 0)
            {
                throw new Exception("No msi files found in " + settings.MsiFolder);
            }

            return msiFileInfos.First().FullName;
        }

        public void InstallMsi(Request currentRequest, CmdLineInfo cmdLineInfo)
        {
            //Lock-o il file msi perche` fino a che windows installer non comincia il suo lavoro
            //nessuno lo lock-a e quindi l'utente potrebbe cancellarlo causando errori.
            using (var lockToken = this.fileLocker.CreateLockToken(currentRequest.MsiPath))
            {
                //OnNotification(new NotificationEventArgs() { Message = "Removing installation info..." });
                //Rimuovo le informazioni di installazione dal registry se presenti in
                //modo che la mia installazione non le trovi e tenga i parametri che passo io da riga di comando.
                this.msiZapper.ZapMsi(currentRequest.MsiPath);
                this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
                this.registryService.RemoveInstallerFoldersKeys(currentRequest.RootFolder, currentRequest.Instance);
                //OnNotification(new NotificationEventArgs() { Message = "Installation info removed" });

                //OnNotification(new NotificationEventArgs() { Message = "Launching msi..." });
                this.LogInfo("Launching msi with command line " + cmdLineInfo.ToString());
                string installLogFilePath = Path.Combine(this.settings.LogsFolder, "Mago4_" + currentRequest.Instance.Name + "_InstallLog_" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture) + ".log");

                try
                {
                    this.LaunchProcess(
                            msiexecPath,
                            String.Format("/i \"{0}\" /qn /norestart {1} UICULTURE=\"it-IT\" INSTALLLOCATION=\"{2}\" INSTANCENAME=\"{3}\" DEFAULTWEBSITENAME=\"{4}\" DEFAULTWEBSITEID={5} DEFAULTWEBSITEPORT={6} {7}", currentRequest.MsiPath, this.settings.MsiLog ? String.Format("/l*vx \"{0}\"", installLogFilePath) : string.Empty, currentRequest.RootFolder, currentRequest.Instance.Name, currentRequest.Instance.WebSiteInfo.SiteName, currentRequest.Instance.WebSiteInfo.SiteID, currentRequest.Instance.WebSiteInfo.SitePort, cmdLineInfo.ToString()),
                            3600000
                            );
                    //OnNotification(new NotificationEventArgs() { Message = "Msi execution successfully terminated..." });
                }
                catch (Exception exc)
                {
                    this.LogError("Msi execution terminated with errors...", exc);
                    //OnNotification(new NotificationEventArgs() { Message = "Msi execution terminated with errors..." });
                    throw;
                }
            }

            //OnNotification(new NotificationEventArgs() { Message = "Cleaning registry..." });
            System.Threading.Thread.Sleep(2000);//Wait for the msiexec process to unlock the msi file...
            this.msiZapper.ZapMsi(currentRequest.MsiPath);
            this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
            this.registryService.RemoveInstallerFoldersKeys(currentRequest.RootFolder, currentRequest.Instance);
            //OnNotification(new NotificationEventArgs() { Message = "Now the registry is clean" });
        }

        public void UpdateMsi(Request currentRequest, CmdLineInfo cmdLineInfo)
        {
            //Lock-o il file msi perche` fino a che windows installer non comincia il suo lavoro
            //nessuno lo lock-a e quindi l'utente potrebbe cancellarlo causando errori.
            using (var lockToken = this.fileLocker.CreateLockToken(currentRequest.MsiPath))
            {
                //OnNotification(new NotificationEventArgs() { Message = "Removing installation info..." });
                //Rimuovo le informazioni di installazione dal registry se presenti in
                //modo che la mia installazione non le trovi e tenga i parametri che passo io da riga di comando.
                this.msiZapper.ZapMsi(currentRequest.MsiPath);
                this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
                this.registryService.RemoveInstallerFoldersKeys(currentRequest.RootFolder, currentRequest.Instance);
                //OnNotification(new NotificationEventArgs() { Message = "Installation info removed" });

                //Rimuovo la parte di installazione su IIS per evitare che, se tra un setup e il successivo
                //alcuni componenti cambiano noe, mi rimangano dei cadaveri.
                //Rimuovere prima le virtual folder e le application, poi gli application pool.
                //Un application pool a cui sono collegate ancora applicazioni non puo` essere eliminato
                //OnNotification(new NotificationEventArgs() { Message = "Removing virtual folders..." });
                this.iisService.RemoveVirtualFoldersAndApplications(currentRequest.Instance);
                //OnNotification(new NotificationEventArgs() { Message = "Virtual folders removed" });
                //OnNotification(new NotificationEventArgs() { Message = "Removing application pools..." });
                this.iisService.RemoveApplicationPools(currentRequest.Instance);
                //OnNotification(new NotificationEventArgs() { Message = "Application pools removed" });

                //OnNotification(new NotificationEventArgs() { Message = "Launching msi..." });
                this.LogInfo("Launching msi with command line " + cmdLineInfo.ToString());
                string installLogFilePath = Path.Combine(this.settings.LogsFolder, "Mago4_" + currentRequest.Instance.Name + "_UpdateLog_" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture) + ".log");

                try
                {
                    this.LaunchProcess(
                            msiexecPath,
                            String.Format("/i \"{0}\" /qn /norestart {1} UICULTURE=\"it-IT\" INSTALLLOCATION=\"{2}\" INSTANCENAME=\"{3}\" DEFAULTWEBSITENAME=\"{4}\" DEFAULTWEBSITEID={5} DEFAULTWEBSITEPORT={6} {7}", currentRequest.MsiPath, this.settings.MsiLog ? String.Format("/l*vx \"{0}\"", installLogFilePath) : string.Empty, currentRequest.RootFolder, currentRequest.Instance.Name, currentRequest.Instance.WebSiteInfo.SiteName, currentRequest.Instance.WebSiteInfo.SiteID, currentRequest.Instance.WebSiteInfo.SitePort, cmdLineInfo.ToString()),
                            3600000
                            );
                    //OnNotification(new NotificationEventArgs() { Message = "Msi execution successfully terminated..." });
                }
                catch (Exception exc)
                {
                    this.LogError("Msi execution terminated with errors...", exc);
                    //OnNotification(new NotificationEventArgs() { Message = "Msi execution terminated with errors..." });
                    throw;
                }
            }

            //OnNotification(new NotificationEventArgs() { Message = "Cleaning registry..." });
            System.Threading.Thread.Sleep(2000);//Wait for the msiexec process to unlock the msi file...
            this.msiZapper.ZapMsi(currentRequest.MsiPath);
            this.registryService.RemoveInstallationInfoKey(currentRequest.MsiPath);
            this.registryService.RemoveInstallerFoldersKeys(currentRequest.RootFolder, currentRequest.Instance);
            //OnNotification(new NotificationEventArgs() { Message = "Now the registry is clean" });
        }
    }
}
