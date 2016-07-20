using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Plugins;
using System;
using System.IO;
using System.IO.Pipes;

namespace Microarea.Mago4Butler
{
    class AppAutomationServer : ILogger, IDisposable
    {
        NamedPipeServerStream server;
        StreamReader reader;
        StreamWriter writer;

        bool running;
        internal void Start()
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
                        var command = tokens[0];
                        string args = null;
                        if (tokens.Length > 1)
                        {
                            args = tokens[1];
                        }
                        ProcessCommand(command, args);
                    }
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error in AppAutomationServer", exc);
            }
        }

        private void ProcessCommand(string command, string args)
        {
            switch (command)
            {
                case AppAutomationClient.ShutdownApplicationCommand:
                    Environment.Exit(0);
                    break;
                case AppAutomationClient.GetVersionCommand:
                    {
                        if (args == Path.GetFileNameWithoutExtension(this.GetType().Assembly.Location))
                        {
                            writer.WriteLine(this.GetType().Assembly.GetName().Version.ToString());
                            writer.Flush();
                        }
                        else
                        {
                            bool found = false;
                            foreach (var plugin in IoCContainer.Instance.Get<PluginService>().Plugins)
                            {
                                if (plugin.GetName() == args)
                                {
                                    found = true;
                                    writer.WriteLine(plugin.GetVersion().ToString());
                                    writer.Flush();
                                    break;
                                }
                            }
                            if (!found)
                            {
                                writer.WriteLine(string.Empty);
                                writer.Flush();
                                break;
                            }
                        }
                    }
                    break;
                case AppAutomationClient.GetPluginFolderPathCommand:
                    {
                        writer.WriteLine(PluginService.PluginsPath);
                        writer.Flush();

                    }
                    break;
                default:
                    break;
            };
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
