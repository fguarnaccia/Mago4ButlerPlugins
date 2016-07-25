using System;
using System.Linq;
using System.Collections.Generic;
using Microarea.Mago4Butler.Model;

namespace Microarea.Mago4Butler.BL
{
    internal class Request
    {
        public RequestType RequestType { get; set; }
        public string RootFolder { get; set; }
        public string MsiPath { get; set; }
        public Instance Instance { get; set; }
    }
}