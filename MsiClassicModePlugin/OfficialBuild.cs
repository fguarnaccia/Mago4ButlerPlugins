using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsiClassicModePlugin.MABuilds
{
    partial class GetUpdatesResponse
    {

        public override string ToString()
        {
            return UppercaseFirst((this.msiFileNameField).ToLower());
        }

        internal static string UppercaseFirst(string s)
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
