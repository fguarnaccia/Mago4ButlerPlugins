using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace VersionManager
{
    class Program
    {
        static void Main(string[] args)
        {
//#if DEBUG
//            return;
//#endif

            string starVersionRegexPattern = "(?<major>[0-9]+)\\.(?<minor>[0-9]+)\\.\\*";
            Regex starVersionRegex = new Regex(starVersionRegexPattern);

            string versionRegexPattern = "[0-9]+\\.[0-9]+\\.[0-9\\.]+\\.[0-9\\.]";
            Regex versionRegex = new Regex(versionRegexPattern);

            string path = string.Empty;

            if (args[0] == "POST")
            {
               SetPluginVersion(args[1] , args[2]);
                return;
            }
            else
            {
                 path = args[0];
            }

            string content = null;
            using (StreamReader sr = new StreamReader(path))
            {
                content = sr.ReadToEnd();
            }

            string version = null;
            Match starVersionMatch = starVersionRegex.Match(content);
            if (starVersionMatch.Success)
            {
                version = CalculateNewVersion(starVersionMatch.Groups["major"].Value, starVersionMatch.Groups["minor"].Value);
                content = content.Replace(starVersionMatch.Value, version);
            }
            else
            {
                Match versionMatch = versionRegex.Match(content);
                if (versionMatch.Success)
                {
                    version = CalculateNewVersion(versionMatch.Value);
                    content = content.Replace(versionMatch.Value, version);
                }
            }

            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(content);
            }
        }

        private static string CalculateNewVersion(string major, string minor)
        {
            int maj = 0, min = 0;
            Int32.TryParse(major, out maj);
            Int32.TryParse(minor, out min);
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", maj, min, CalculateBuildAsNetFxDoes(), CalculateRevisionAsNetFxDoes());
        }

        private static int CalculateRevisionAsNetFxDoes()
        {
            DateTime nowDate = DateTime.Now.Date;
            DateTime now = DateTime.Now;

            return Convert.ToInt32((now - nowDate).TotalSeconds / 2);
        }

        private static int CalculateBuildAsNetFxDoes()
        {
            DateTime startDate = new DateTime(2000, 1, 1);
            DateTime now = DateTime.Now.Date;

            return Convert.ToInt32((now - startDate).TotalDays);
        }

        private static string CalculateNewVersion(string oldVersion)
        {
            Version v = new Version(oldVersion);
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", v.Major, v.Minor, v.Build + 1, v.Revision);
        }

              

        private static void SetPluginVersion(string plgnNamespace, string pathOfAssemblyinfo)
        {
            
            string xmlfile = string.Empty;
            XmlDocument domdoc = new XmlDocument();

            xmlfile = @"\\SRV-BKS\Download\Tools Microarea\Mago4ButlerUpdates\Updates.xml";
            domdoc.Load(xmlfile);

            foreach (XmlElement item in domdoc.DocumentElement.GetElementsByTagName("update"))
            {

                if (item.GetAttribute("name") == plgnNamespace)
                {
                   item.SetAttribute("version", GetLastVersion(pathOfAssemblyinfo));
                }

            }
            domdoc.Save(xmlfile);

         }

        private static string GetLastVersion(string pathOfAssemblyInfo)
        {

            string versionRegexPattern = "[0-9]+\\.[0-9]+\\.[0-9\\.]+\\.[0-9\\.]+";
            Regex versionRegex = new Regex(versionRegexPattern);
            
            string version = string.Empty;

            pathOfAssemblyInfo = Path.Combine(pathOfAssemblyInfo, "AssemblyInfo.cs");
            string content = string.Empty;
            using (StreamReader sr = new StreamReader(pathOfAssemblyInfo))
            {
                content = sr.ReadToEnd();
            }

            Match versionMatch = versionRegex.Match(content);
            if (versionMatch.Success)
            {
                Version v = new Version(versionMatch.Value);
                version = v.ToString();
              
            }

            return version;
        }

    }
}
