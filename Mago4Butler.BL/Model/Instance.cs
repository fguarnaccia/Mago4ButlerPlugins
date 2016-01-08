﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class Instance
    {
        public string Name { get; set; }
        public Version Version { get; set; }

        public static Instance FromStandardDirectoryInfo(DirectoryInfo standardDirInfo)
        {
            var parentDirInfo = standardDirInfo.Parent;
            var installationVerFilePath = Path.Combine(standardDirInfo.FullName, "Installation.ver");

            string content = null;
            using (var sr = new StreamReader(installationVerFilePath))
            {
                content = sr.ReadToEnd();
            }
            var versionRegex = new Regex("<Version>(?<version>.*)</Version>", RegexOptions.IgnoreCase);
            var match = versionRegex.Match(content);
            if (match.Success)
            {
                var group = match.Groups["version"];
                if (group != null)
                {
                    return new Instance() { Name = parentDirInfo.Name, Version = Version.Parse(group.Value) };
                }
            }

            Debug.Assert(false);
            return null;
        }

        public override string ToString()
        {
            return String.Format("{0}, v.{1}", this.Name, this.Version.ToString());
        }
    }
}
