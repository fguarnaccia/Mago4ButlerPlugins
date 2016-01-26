using Microarea.TaskBuilderNet.ParametersManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.DigitalSignerClient
{
    class SessionManager
    {
        string country;
        IParametersManager parametersManager = new ParametersManagerV1();

        public SessionManager(string country)
        {
            this.country = country;
        }
        //---------------------------------------------------------------------------
        public string PackData(byte[] file, bool isOn, Guid sessionId)
        {
            string inputString = Convert.ToBase64String(file);

            return parametersManager.SetParameter(
                isOn,
                inputString,
                sessionId.ToString(),
                this.country
                );
        }

        //---------------------------------------------------------------------------
        public byte[] UnpackData(string dataFile, bool isOn, Guid sessionId)
        {
            string outputString = parametersManager.GetParameter(
                 isOn,
                 dataFile,
                 sessionId.ToString(),
                 this.country
                 );

            return Convert.FromBase64String(outputString);
        }
    }
}
