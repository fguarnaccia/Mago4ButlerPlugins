using Microarea.Mago4Butler.Log;
using System;
using System.IO;
using System.IO.Pipes;

namespace Microarea.Mago4Butler.Automation
{
    public class AppAutomationServer : ILogger, IDisposable
    {
        NamedPipeServerStream server;
        StreamReader reader;
        StreamWriter writer;

        public event EventHandler<CommandEventArgs> CommandReceived;
        protected virtual void OnCommandReceived(CommandEventArgs e)
        {
            var handler = CommandReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        bool running;
        public void Start()
        {
            try
            {
                running = true;

                server = new NamedPipeServerStream(AppAutomationClient.ChannelName);
                server.WaitForConnection();
                reader = new StreamReader(server);
                writer = new StreamWriter(server);
                while (running)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var tokens = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        string args = null;
                        if (tokens.Length > 1)
                        {
                            args = tokens[1];
                        }
                        var e = new CommandEventArgs() { Command = tokens[0], Args = args };
                        OnCommandReceived(e);
                        writer.WriteLine(e.Response);
                        writer.Flush();
                    }
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error in AppAutomationServer", exc);
            }
        }

        internal void Stop()
        {
            running = false;
        }

        protected virtual void Dispose(bool managed)
        {
           if (managed)
            {
                this.Stop();
                try
                {
                    if (server != null)
                    {
                        server.Dispose();
                        server = null;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
