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
                        _settings = new Settings();
                    }
                }
                return _settings;
            }
        }

        public string RootFolder { get; set; } = "C:\\Microarea\\M4";

        public string MsiFolder { get; set; } = "C:\\Microarea\\M4\\Msi";
        public string LogsFolder { get; set; } = "C:\\Microarea\\M4\\Logs";
        public string SiteName { get; set; } = WebSiteInfo.DefaultWebSite.SiteName;
        public bool AlsoDeleteCustom { get; set; }
        public bool MsiLog { get; set; } = true;
        public bool UseProxy { get; set; }
        public string ProxyServerUrl { get; set; } = string.Empty;
        public int ProxyServerPort { get; set; }
        public bool UseCredentials { get; set; }
        public string DomainName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string LastFolderOpenedBrowsingForMsi { get; set; } = string.Empty;
        public string UpdatesUri { get; set; } = "http://www.microarea.it/PAASUpdates/UpdatesService.asmx";

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

            var deserializer = new Deserializer(null, new PascalCaseNamingConvention(), ignoreUnmatched: true);
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
