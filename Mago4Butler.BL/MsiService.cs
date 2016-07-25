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
    public class MsiService : ILogger
    {
        static readonly Type InstallerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
        ISettings settings;

        public MsiService(ISettings settings)
        {
            this.settings = settings;
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
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
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
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
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
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
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
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
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
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
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
    }
}
