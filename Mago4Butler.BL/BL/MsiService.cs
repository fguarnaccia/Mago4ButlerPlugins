using System;
using System.Collections.Generic;
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

        public string GetProductName(string msiFilePath)
        {
            try
            {
                var installer = Activator.CreateInstance(InstallerType) as Installer;
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
                var view = database.OpenView("SELECT * from Property WHERE Property = 'PRODUCTNAME'");

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

        public Version GetVersion(string msiFilePath)
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
                        return new Version();
                    }
                    return Version.Parse(strVersion);
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
    }
}
