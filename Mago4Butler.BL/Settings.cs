using Microarea.TaskBuilderNet.Core.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Microarea.Mago4Butler.BL
{
    public class Settings : ISettings
    {
        readonly static string settingsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Mago4Butler");
        const string settingsFileName = "Settings.yml";

        static ISettings _settings;

        public static ISettings Default
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Settings.Load();
                    if (_settings == null)
                    {
                        _settings = CreateDefaultSettings();
                    }
                }
                return _settings;
            }
        }

        private static ISettings CreateDefaultSettings()
        {
            return new Settings()
            {
                RootFolder = "C:\\Microarea\\M4",
                LogsFolder = "C:\\Microarea\\M4\\Logs",
                SiteName = WebSiteInfo.DefaultWebSite.SiteName,
                ShowRootFolderChoice = true,
                AlsoDeleteCustom = false,
                MsiLog = true,
                UseProxy = false,
                ProxyServerUrl = string.Empty,
                ProxyServerPort = 0,
                UseCredentials = false,
                DomainName = string.Empty,
                Username = string.Empty,
                Password = string.Empty
            };
        }

        public string RootFolder { get; set; }
        public string LogsFolder { get; set; }
        public string SiteName { get; set; }
        public bool ShowRootFolderChoice { get; set; }
        public bool AlsoDeleteCustom { get; set; }
        public bool MsiLog { get; set; }
        public bool UseProxy { get; set; }
        public string ProxyServerUrl { get; set; }
        public int ProxyServerPort { get; set; }
        public bool UseCredentials { get; set; }
        public string DomainName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void Save()
        {
            var settingsFileInfo = new FileInfo(Path.Combine(settingsFolderPath, settingsFileName));
            var settingsDirInfo = settingsFileInfo.Directory;
            if (!settingsDirInfo.Exists)
            {
                settingsDirInfo.Create();
            }

            var serializer = new Serializer(SerializationOptions.DisableAliases | SerializationOptions.EmitDefaults);
            using (var outputStream = File.Create(settingsFileInfo.FullName))
            using (var streamWriter = new StreamWriter(outputStream))
            {
                Password = Crypto.Encrypt(Password);
                serializer.Serialize(streamWriter, this);
            }
        }

        static ISettings Load()
        {
            var settingsFileInfo = new FileInfo(Path.Combine(settingsFolderPath, settingsFileName));

            if (!settingsFileInfo.Exists)
            {
                return null;
            }

            var deserializer = new Deserializer(null, new PascalCaseNamingConvention());
            using (var inputStream = settingsFileInfo.OpenRead())
            using (var streamReader = new StreamReader(inputStream))
            {
                var settings = deserializer.Deserialize<Settings>(streamReader);
                settings.Password = Crypto.Decrypt(settings.Password);
                return settings;
            }
        }
    }
}
