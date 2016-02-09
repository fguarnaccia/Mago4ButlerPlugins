using System;
using System.Linq;
using System.Collections.Generic;
using Microarea.Mago4Butler.BL;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Microarea.Mago4Butler
{
    internal class ConsoleRunner : IForrest
    {
        const string helpSwitch = "/?";
        const string statusSwitch = "/status";
        const string updateSwitch = "/update";
        const string updateAllSwitch = "/updateall";
        const string installSwitch = "/install";
        const string uninstallSwitch = "/uninstall";
        const string uninstallAllSwitch = "/uninstallall";
        const string msiFilaFullPathSwitch = "/msi";

        List<Instance> instanceToUpdate = new List<Instance>();
        List<Instance> instanceToInstall = new List<Instance>();
        List<Instance> instanceToUninstall = new List<Instance>();
        bool printCurrentStatus;
        bool updateAll;
        bool uninstallAll;

        string msiFullFilePath;
        string[] args;

        public ConsoleRunner(string[] args)
        {
            this.args = args;
        }

        bool ParseArgs(string[] args)
        {
            if (args == null || args.Length == 0 || !args[0].StartsWith("/"))
            {
                return false;
            }

            for (int i = 0; i < args.Length; i = i + 2)
            {
                switch (args[i].ToLowerInvariant())
                {
                    case statusSwitch:
                        {
                            if (args.Length > 1)
                            {
                                return false;
                            }
                            this.printCurrentStatus = true;
                            break;
                        }
                    case updateSwitch:
                        {
                            if ((i + 1) == args.Length || args[i + 1].StartsWith("/"))
                            {
                                return false;
                            }
                            instanceToUpdate.AddRange(Parse(args[i + 1]));
                            break;
                        }
                    case updateAllSwitch:
                        {
                            this.updateAll = true;
                            break;
                        }
                    case installSwitch:
                        {
                            if ((i + 1) == args.Length || args[i + 1].StartsWith("/"))
                            {
                                return false;
                            }
                            instanceToInstall.AddRange(Parse(args[i + 1]));
                            break;
                        }
                    case uninstallSwitch:
                        {
                            if ((i + 1) == args.Length || args[i + 1].StartsWith("/"))
                            {
                                return false;
                            }
                            instanceToUninstall.AddRange(Parse(args[i + 1]));
                            break;
                        }
                    case uninstallAllSwitch:
                        {
                            this.uninstallAll = true;
                            break;
                        }
                    case msiFilaFullPathSwitch:
                        {
                            if ((i + 1) == args.Length || args[i + 1].StartsWith("/"))
                            {
                                return false;
                            }
                            msiFullFilePath = args[i + 1];
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Unrecognized arg '{0}'", args[i], Color.Red);
                            return false;
                        }
                }
            }

            if (this.updateAll && instanceToUpdate.Count > 0)
            {
                Console.WriteLine("/updateAll and /update are incompatible", Color.Red);
                return false;
            }
            if (this.uninstallAll && instanceToUninstall.Count > 0)
            {
                Console.WriteLine("/uninstallAll and /uninstall are incompatible", Color.Red);
                return false;
            }
            if ((instanceToInstall.Count > 0 || instanceToUpdate.Count > 0))
            {
                if (String.IsNullOrWhiteSpace(msiFullFilePath))
                {
                    Console.WriteLine("No msi file specified.", Color.Red);
                    Console.WriteLine("");
                    return false;
                }
                if (!File.Exists(msiFullFilePath))
                {
                    Console.WriteLine("Msi file " + msiFullFilePath + " does not exist.", Color.Red);
                    return false;
                }

                var msiService = IoCContainer.Instance.Get<MsiService>();
                var version = msiService.GetVersion(msiFullFilePath);
                foreach (var instance in instanceToInstall)
                {
                    instance.Version = version;
                }
                foreach (var instance in instanceToUpdate)
                {
                    instance.Version = version;
                }
            }

            return true;
        }

        IEnumerable<Instance> Parse(string instanceNames)
        {
            string[] tokens = instanceNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var instances = new List<Instance>();
            foreach (var instanceName in tokens)
            {
                instances.Add(new Instance() { Name = instanceName, WebSiteInfo = WebSiteInfo.DefaultWebSite });
            }

            return instances;
        }

        void PrintHelp(bool skipHeader = false)
        {
            var version = typeof(Program).Assembly.GetName().Version.ToString();

            if (!skipHeader)
            {
                Console.WriteLine("Microarea Mago4 multi instance manager version " + version, Color.White);
                Console.WriteLine("[Microsoft .NET Framework, version " + Environment.Version.ToString() + "]", Color.White);
                Console.WriteLine("Copyright (C) Microarea S.p.A. All rights reserved.", Color.White);
                Console.WriteLine("");
            }
            Console.WriteLine("Usage:");
            Console.WriteLine("");
            Console.WriteLine("Mago4Butler.exe " + statusSwitch, Color.White);
            Console.WriteLine("Print installation status for all the instances");
            Console.WriteLine("");
            Console.WriteLine("Mago4Butler.exe " + installSwitch + " InstanceName1;...;InstanceNameN " + msiFilaFullPathSwitch + " <msi file path>", Color.White);
            Console.WriteLine("Install all the specified instances using the specified msi file");
            Console.WriteLine("");
            Console.WriteLine("Mago4Butler.exe " + updateSwitch + " InstanceName1;...;InstanceNameN " + msiFilaFullPathSwitch + " <msi file path>", Color.White);
            Console.WriteLine("Update all the specified instances using the specified msi file");
            Console.WriteLine("");
            Console.WriteLine("Mago4Butler.exe " + uninstallSwitch + " InstanceName1;...;InstanceNameN", Color.White);
            Console.WriteLine("Uninstall all the specified instances");
            Console.WriteLine("");
            Console.WriteLine("Mago4Butler.exe " + updateAllSwitch + msiFilaFullPathSwitch + " <msi file path>", Color.White);
            Console.WriteLine("Update all instances using the specified msi file");
            Console.WriteLine("");
            Console.WriteLine("Mago4Butler.exe " + uninstallAllSwitch, Color.White);
            Console.WriteLine("Uninstall all instances");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(installSwitch + ", " + uninstallSwitch + " and " + updateSwitch + " can be used together:");
            Console.WriteLine("Mago4Butler.exe " + installSwitch + " InstanceName1;...;InstanceNameN " + updateSwitch + " InstanceNameM;...;InstanceNameQ " + uninstallSwitch + " InstanceNameS;...;InstanceNameZ " + msiFilaFullPathSwitch + " <msi file path>", Color.White);
        }

        public int Run()
        {
            try
            {
                using (var consoleWrapper = new ConsoleWrapper())
                {
                    if (!ParseArgs(this.args))
                    {
                        PrintHelp();
                        return 1;
                    }

                    var batch = IoCContainer.Instance.Get<Batch>();

                    if (printCurrentStatus)
                    {
                        batch.PrintCurrentStatus();
                        return 0;
                    }

                    if (this.updateAll)
                    {
                        batch.UpdateAll(msiFullFilePath);
                    }
                    else
                    {
                        batch.Update(msiFullFilePath, instanceToUpdate.ToArray());
                    }

                    batch.Install(msiFullFilePath, instanceToInstall.ToArray());
                    if (this.uninstallAll)
                    {
                        batch.UninstallAll(msiFullFilePath);
                    }
                    else
                    {
                        batch.Uninstall(instanceToUninstall.ToArray());
                    }

                    //Giving time to the install service to start...
                    Thread.Sleep(3000);

                    batch.WaitingForGodot();
                }
                return 0;
            }
            catch (Exception exc)
            {
                Console.WriteLine("Error: " + exc.ToString());
                return 1;
            }
        }
    }
}