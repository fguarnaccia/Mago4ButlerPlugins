using System;
using System.IO;
using System.Runtime.InteropServices;
using WindowsInstaller;

namespace Microarea.Mago4Butler.BL
{
    public class MsiZapper
    {
        static readonly Type InstallerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");

        public void ZapMsi(string msiFullPath)
        {
            string productCode = ExtractProductCode(msiFullPath);

            if (String.IsNullOrWhiteSpace(productCode))
            {
                throw new ArgumentException(String.Format("'productCode' from {0} is null or empty", msiFullPath));
            }

            var msZapFullPath = Path.Combine(Path.GetTempPath(), "mszap.exe");
            using (var binaryWriter = new BinaryWriter(File.Create(msZapFullPath)))
            {
                binaryWriter.Write(Resource.MsiZap, 0, Resource.MsiZap.Length);
            }

            this.LaunchProcess(
                msZapFullPath,
                String.Format("TW! {0}", productCode),
                3600000
                );

            try { File.Delete(msZapFullPath); }
            catch {}
        }

        private string ExtractProductCode(string msiFullPath)
        {
            var installer = Activator.CreateInstance(InstallerType) as Installer;
            var database = installer.OpenDatabase(msiFullPath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
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
    }
}
