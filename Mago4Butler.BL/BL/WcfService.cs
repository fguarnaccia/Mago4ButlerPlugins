﻿using System;
using System.Globalization;
using System.IO;

namespace Microarea.Mago4Butler.BL
{
    public class WcfService : ILogger
    {
        readonly ISettings settings;

        public WcfService(ISettings settings)
        {
            this.settings = settings;
        }
        public void RegisterWcf(int startPort, string instanceName)
        {
            var instanceRootFolder = Path.Combine(this.settings.RootFolder, instanceName);
            var processFilePath = Path.Combine(instanceRootFolder, "Apps", "ClickOnceDeployer", "ClickOnceDeployer.exe");
            string user = GetUserNameForWcfRegistration();

            var args = string.Format(
                CultureInfo.InvariantCulture,
                "RegisterWcf /root \"{0}\" /port {1} /user {2}",
                instanceRootFolder,
                startPort.ToString(CultureInfo.InvariantCulture),
                user
                );

            try
            {
                this.LogInfo("Wcf registration started with parameters: " + args);
                this.LaunchProcess(processFilePath, args, 10000);
                this.LogInfo("Wcf registration successfully ended");
            }
            catch (Exception exc)
            {
                this.LogError("Wcf registration terminated with errors...", exc);
            }
        }

        private static string GetUserNameForWcfRegistration()
        {
#warning Chiamare il metodo GetAspNetUser di LoginManager
            return "NETWORKSERVICE";
        }

        public void CreateSettingsConfigFile(string instanceName, int port)
        {
            var customFolderPath = Path.Combine(this.settings.RootFolder, instanceName, "Custom\\Companies\\AllCompanies\\TaskBuilder\\Framework\\TbGenlib\\Settings\\AllUsers");
            var settingsConfigFilePath = Path.Combine(customFolderPath, "Settings.config");

            var customDirInfo = new DirectoryInfo(customFolderPath);
            if (!customDirInfo.Exists)
            {
                customDirInfo.Create();
            }

            string settingsConfigTemplateContent;
            using (var sr = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("Microarea.Mago4Butler.BL.res.SettingsConfig.template")))
            {
                settingsConfigTemplateContent = sr.ReadToEnd();
            }

            var data = new
            {
                WcfSoapPort = port.ToString(CultureInfo.InvariantCulture)
            };

            var settingsConfigFileInfo = new FileInfo(settingsConfigFilePath);
            if (settingsConfigFileInfo.Exists)
            {
                settingsConfigFileInfo.Delete();
            }

            Nustache.Core.Render.StringToFile(settingsConfigTemplateContent, data, settingsConfigFilePath);
        }
    }
}