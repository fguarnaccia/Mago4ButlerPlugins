using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    class AppAutomationClient : ILogger, IDisposable
    {
        internal const string ChannelName = "Mago4Butler_NamedPipe";
        internal const string ShutdownApplicationCommand = "ShutdownApplication";
        internal const string GetVersionCommand = "GetVersion";
        internal const string GetPluginFolderPathCommand = "GetPluginFolderPath";

        NamedPipeClientStream client;
        StreamReader reader;
        StreamWriter writer;

        public AppAutomationClient()
        {
            client = new NamedPipeClientStream(AppAutomationClient.ChannelName);
            try
            {
                client.Connect(2000);
                reader = new StreamReader(client);
                writer = new StreamWriter(client);
            }
            catch (Exception exc)
            {
                this.LogError("AppAutomation client cannot connect to the server", exc);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool managed)
        {
            if (managed)
            {
                try
                {
                    if (client != null)
                    {
                        client.Dispose();
                        client = null;
                    }
                    if (reader != null)
                    {
                        reader.Dispose();
                        reader = null;
                    }
                    if (writer != null)
                    {
                        writer.Dispose();
                        writer = null;
                    }
                }
                catch
                {}
            }
        }

        internal void ShutdownApplication()
        {
            if (client.IsConnected)
            {
                writer.WriteLine(AppAutomationClient.ShutdownApplicationCommand);
                writer.Flush();
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send ShutdownApplication command");
            }
        }

        internal System.Version GetPluginVersion(string pluginName)
        {
            if (client.IsConnected)
            {
                writer.WriteLine(String.Join(" ", AppAutomationClient.GetVersionCommand, pluginName));
                writer.Flush();
                var response = reader.ReadLine();
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

        internal string GetPluginFolderPath()
        {
            if (client.IsConnected)
            {
                writer.WriteLine(AppAutomationClient.GetPluginFolderPathCommand);
                writer.Flush();
                var response = reader.ReadLine();

                return response;
            }
            else
            {
                this.LogInfo("AppAutomation server is not connected, unable to send GetPluginFolderPath command");
                return string.Empty;
            }
        }
    }
}
