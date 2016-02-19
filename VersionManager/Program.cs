using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VersionManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = null;

            string versionRegexPattern = "[0-9]+\\.[0-9]+\\.(\\*|[0-9\\.]+)";
            Regex versionRegex = new Regex(versionRegexPattern);
            string path = "..\\..\\..\\Mago4Butler\\Properties\\AssemblyInfo.cs";
            var fileInfo = new FileInfo(path);
            fileInfo.Attributes &= ~FileAttributes.ReadOnly;
            using (StreamReader sr = new StreamReader(path))
            {
                content = sr.ReadToEnd();
            }
            Match versionMatch = versionRegex.Match(content);
            if (versionMatch.Success)
            {
                string version = typeof(Program).Assembly.GetName().Version.ToString();
                Version v = new Version(version);
                if (versionMatch.Value.IndexOf("*") != -1)
                {
                     version = String.Format("{0}.{1}.{2}.0", v.Major, v.Build, v.Revision);
                }
                else
                {
                    version = String.Format("{0}.{1}.{2}.{3}", v.Major, v.Minor, v.Build + 1, v.Revision);
                }

                content = content.Replace(versionMatch.Value, version);
            }
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(content);
            }
        }
    }
}
