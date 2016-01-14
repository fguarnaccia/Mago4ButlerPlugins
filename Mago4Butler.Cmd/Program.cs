using Microarea.Mago4Butler;
using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Cmd
{
    static class Program
    {
        const string updateSwitch = "/update";
        const string installSwitch = "/install";
        const string uninstallSwitch = "/uninstall";
        const string msiFilaFullPathSwitch = "/msi";

        static List<Instance> instanceToUpdate = new List<Instance>();
        static List<Instance> instanceToInstall = new List<Instance>();
        static List<Instance> instanceToUninstall = new List<Instance>();

        static string msiFullFilePath;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!ParseArgs(args))
            {
                Environment.Exit(1);
                return;
            }

            Model model = new Model();
            model.Init(Settings.Default.RootFolder);

            var batch = new Batch(model, Console.Out);

            batch.Install(msiFullFilePath, instanceToInstall.ToArray());
            batch.Update(msiFullFilePath, instanceToUpdate.ToArray());
            batch.Uninstall(instanceToUninstall.ToArray());

            //Giving time to the install service to start...
            Thread.Sleep(10000);

            while (batch.IsRunning)
            {
                Thread.Sleep(5000);
            }
        }

        static bool ParseArgs(string[] args)
        {
            if (args == null || args.Length == 0 || !args[0].StartsWith("/"))
            {
                PrintHelp();
                return false;
            }

            for (int i = 0; i < args.Length; i = i + 2)
            {
                switch (args[i])
                {
                    case updateSwitch:
                        {
                            if ((i + 1) == args.Length || args[i + 1].StartsWith("/"))
                            {
                                PrintHelp();
                                return false;
                            }
                            instanceToUpdate.AddRange(Parse(args[i + 1]));
                            break;
                        }
                    case installSwitch:
                        {
                            if ((i + 1) == args.Length || args[i + 1].StartsWith("/"))
                            {
                                PrintHelp();
                                return false;
                            }
                            instanceToInstall.AddRange(Parse(args[i + 1]));
                            break;
                        }
                    case uninstallSwitch:
                        {
                            if ((i + 1) == args.Length || args[i + 1].StartsWith("/"))
                            {
                                PrintHelp();
                                return false;
                            }
                            instanceToUninstall.AddRange(Parse(args[i + 1]));
                            break;
                        }
                    case msiFilaFullPathSwitch:
                        {
                            if ((i + 1) == args.Length || args[i + 1].StartsWith("/"))
                            {
                                PrintHelp();
                                return false;
                            }
                            msiFullFilePath = args[i + 1];
                            break;
                        }
                    default:
                        {
                            PrintHelp();
                            return false;
                        }
                }
            }
            if ((instanceToInstall.Count > 0 || instanceToUpdate.Count > 0) && String.IsNullOrWhiteSpace(msiFullFilePath))
            {
                PrintHelp();
                return false;
            }

            if (!File.Exists(msiFullFilePath))
            {
                Console.Out.WriteLine("Msi file " + msiFullFilePath + " does not exist.");
                return false;
            }

            var msiService = new MsiService();
            var version = msiService.GetVersion(msiFullFilePath);
            foreach (var instance in instanceToInstall)
            {
                instance.Version = version;
            }
            foreach (var instance in instanceToUpdate)
            {
                instance.Version = version;
            }
            foreach (var instance in instanceToUninstall)
            {
                instance.Version = version;
            }

            return true;
        }

        static IEnumerable<Instance> Parse(string instanceNames)
        {
            string[] tokens = instanceNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var instances = new List<Instance>();
            foreach (var instanceName in tokens)
            {
                instances.Add(new Instance() { Name = instanceName, WebSiteInfo = WebSiteInfo.DefaultWebSite });
            }

            return instances;
        }

        static void PrintHelp()
        {
            var version = typeof(Program).Assembly.GetName().Version.ToString();

            Console.Out.WriteLine("Microarea Mago4 multi instance manager version " + version);
            Console.Out.WriteLine("[Microsoft .NET Framework, version " + Environment.Version.ToString() + "]");
            Console.Out.WriteLine("Copyright (C) Microarea S.p.A. All rights reserved.");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("Usage:");
            Console.Out.WriteLine("\tMago4Butler.exe " + installSwitch + " InstanceName1;InstanceName2;...;InstanceNameN " + msiFilaFullPathSwitch + " <msi file path>");
            Console.Out.WriteLine("\tInstall all the instances specified by the semicolon separated list using the specified msi file");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("\tMago4Butler.exe " + updateSwitch + " InstanceName1;InstanceName2;...;InstanceNameN " + msiFilaFullPathSwitch + " <msi file path>");
            Console.Out.WriteLine("\tUpdate all the instances specified by the semicolon separated list using the specified msi file");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("\tMago4Butler.exe " + uninstallSwitch + " InstanceName1;InstanceName2;...;InstanceNameN");
            Console.Out.WriteLine("\tUninstall all the instances specified by the semicolon separated list");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("\t" + installSwitch + ", " + uninstallSwitch + " and " + updateSwitch + " can be used together:");
            Console.Out.WriteLine("\tMago4Butler.exe " + installSwitch + " InstanceName1;InstanceName2;...;InstanceNameN " + updateSwitch + " InstanceName1;InstanceName2;...;InstanceNameN " + uninstallSwitch + " InstanceName1;InstanceName2;...;InstanceNameN " + msiFilaFullPathSwitch + " <msi file path>");
        }
    }
}
