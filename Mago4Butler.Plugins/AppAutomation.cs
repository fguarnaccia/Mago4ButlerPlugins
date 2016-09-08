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

        public System.Version GetPluginVersion(string pluginName)
        {
            if (Client.IsConnected)
            {
                Writer.WriteLine(String.Join(" ", Command.GetVersion, pluginName));
                Writer.Flush();
                var response = Reader.ReadLine();
                System.Version pluginVersion = new System.Version();
                System.Version.TryParse(response, out pluginVersion);

                return pluginVersion;
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send GetPluginVersion command");
                return new System.Version(Int32.MaxValue, 0, 0, 0);
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
    }
}
