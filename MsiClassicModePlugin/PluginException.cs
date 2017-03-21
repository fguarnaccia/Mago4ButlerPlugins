using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsiClassicModePlugin
{
  

    [Serializable()]
    public class PluginException : Exception
    {

        public string ParamName { get; set; }
        public object ParamValue { get; set; }



        public PluginException() : base() { }
        public PluginException(string message) : base(message) { }
        public PluginException(string message, System.Exception inner) : base(message, inner) { }
        protected PluginException(System.Runtime.Serialization.SerializationInfo info,
           System.Runtime.Serialization.StreamingContext context)
        { }

        public override string ToString()

        {
            string result;

            result = "***Parameter details***" +
                Environment.NewLine +
                ParamName + "=>" + ParamValue.ToString() +
                Environment.NewLine +
                "***End details***" +
                Environment.NewLine +
                base.ToString();

            return result;


        }
    }
}
