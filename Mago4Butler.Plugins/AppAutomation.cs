using Microarea.Mago4Butler.Automation;
using Microarea.Mago4Butler.Log;
using System;

namespace Microarea.Mago4Butler.Plugins
{
    class AppAutomation : AppAutomationClient, ILogger
    {
        public void ShutdownApplication()
        {
            if (Client.IsConnected)
            {
                Writer.WriteLine(Command.ShutdownApplication);
                Writer.Flush();
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send ShutdownApplication command");
            }
        }

        public Int64 GetPluginVersion(string pluginName)
        {
            if (Client.IsConnected)
            {
                Writer.WriteLine(String.Join(" ", Command.GetVersion, pluginName));
                Writer.Flush();
                var response = Reader.ReadLine();
                Int64 pluginVersion = 0;
                Int64.TryParse(response, out pluginVersion);

                return pluginVersion;
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send GetPluginVersion command");
                return Int64.MaxValue;
            }
        }

        public string GetPluginFolderPath()
        {
            if (Client.IsConnected)
            {
                Writer.WriteLine(Command.GetPluginFolderPath);
                Writer.Flush();
                var response = Reader.ReadLine();

                return response;
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send GetPluginFolderPath command");
                return string.Empty;
            }
        }

        public string[] GetInstances()
        {
            if (Client.IsConnected)
            {
                Writer.WriteLine(Command.GetInstances);
                Writer.Flush();
                var response = Reader.ReadLine();

                return response.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send GetInstances command");
                return new string[] { };
            }
        }

        public string[] GetZombies()
        {
            if (Client.IsConnected)
            {
                Writer.WriteLine(Command.GetZombies);
                Writer.Flush();
                var response = Reader.ReadLine();

                return response.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send GetInstances command");
                return new string[] { };
            }
        }

        public string[] GetPluginsData()
        {
            if (Client.IsConnected)
            {
                Writer.WriteLine(Command.GetPluginsData);
                Writer.Flush();
                var response = Reader.ReadLine();

                return response.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send GetPluginsData command");
                return new string[] { };
            }
        }
    }
}
