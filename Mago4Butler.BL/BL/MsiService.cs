using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInstaller;

namespace Microarea.Mago4Butler.BL
{
    public class MsiService
    {
        static readonly Type InstallerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");

        public string GetProductCode(string msiFilePath)
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

        public string GetUpgradeCode(string msiFilePath)
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

        public Version GetVersion(string msiFilePath)
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
    }
}
