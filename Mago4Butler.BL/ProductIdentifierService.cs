using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microarea.Mago4Butler.Model;
using System.IO;
using Microarea.Mago4Butler.Log;

namespace Microarea.Mago4Butler.BL
{
    public class ProductIdentifierService
    {
        public bool IsMagoNet(string msiFilename)
        {
            return msiFilename.StartsWith(ProductType.Magonet.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsMago4(string msiFilename)
        {
            return msiFilename.StartsWith(ProductType.Mago4.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
