using Microarea.Mago4Butler.Log;
using System;
using System.IO;
using System.IO.Pipes;

namespace Microarea.Mago4Butler.Automation
{
    public class AppAutomationClient : ILogger, IDisposable
    {
        internal const string ChannelName = "Mago4Butler_NamedPipe";

        NamedPipeClientStream client;
        StreamReader reader;
        StreamWriter writer;

        public NamedPipeClientStream Client
        {
            get
            {
                return client;
            }
        }

        public StreamReader Reader
        {
            get
            {
                return reader;
            }
        }

        public StreamWriter Writer
        {
            get
            {
                return writer;
            }
        }

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
    }
}
