﻿using Microarea.Mago4Butler.Log;
using System;
using System.IO;
using System.Runtime.InteropServices;
using WindowsInstaller;

namespace Microarea.Mago4Butler.BL
{
    public class MsiZapper : ILogger
    {
        MsiService msiService;

        public MsiZapper(MsiService msiService)
        {
            this.msiService = msiService;
        }
        public void ZapMsi(string msiFullPath)
        {
            try
            {
                string productCode = msiService.GetProductCode(msiFullPath);

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
                catch { }
            }
            catch (Exception exc)
            {
                this.LogError("Error zapping file " + msiFullPath, exc);
            }
        }
    }
}