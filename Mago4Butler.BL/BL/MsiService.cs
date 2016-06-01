﻿using System;
using System.Collections.Generic;
using System.Globalization;
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

        public IList<string> GetFeatureNames(string msiFilePath)
        {
            List<string> featureNames = new List<string>();
            try
            {
                var installer = Activator.CreateInstance(InstallerType) as Installer;
                var database = installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
                var view = database.OpenView("SELECT * from Feature");

                view.Execute(null);

                Record record = null;
                try
                {
                    record = view.Fetch();
                    while (record != null)
                    {
                        featureNames.Add(record.get_StringData(1));
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
            return featureNames;
        }
    }
}
