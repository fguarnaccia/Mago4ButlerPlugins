using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsiClassicModePlugin.MABuilds
{
    partial class NightlyBuildsResult
    {

        public override string ToString()
        {
            return UppercaseFirst((this.FileName).ToLower());
        }


         static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

    }
}
