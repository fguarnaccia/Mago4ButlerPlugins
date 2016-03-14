using Microarea.TaskBuilderNet.ParametersManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.DigitalSignerClient
{
    public class DigitalSignerClient
    {
        public static int Main(params string[] args)
        {
            if (args == null || args.Length != 1)
            {
                return 1;
            }

            try
            {
                SignFile(args[0]);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public static void SignFile(string filePath)
        {
            using (var proxy = new DigitalSignerServer.DigitalSignerSoapClient())
            {
                (proxy.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 10000000;
# if DEBUG
                proxy.Endpoint.Address = new EndpointAddress("http://localhost/DigitalSigner/DigitalSigner.asmx");
#endif
                var country = GetCountry();
                var sessionManager = new ClickOnceDeployerSessionManager() { Country = country };
                var sessionToken = Guid.NewGuid();

                var dataFile = ReadFileAsByteArray(filePath);

                var envelope = new ClickOnceDeployerEnvelope() { File = dataFile };

                string result = proxy.SignBootstrapper(
                        sessionManager.PackData(envelope, true, sessionToken),
                        sessionToken.ToString("N", CultureInfo.InvariantCulture),
                        country
                        );

                envelope = sessionManager.UnpackData(result, false, sessionToken);

                if (!envelope.Error)
                {
                    SaveFile(envelope.File, filePath);
                }
            }
        }

        static void SaveFile(byte[] result, string filePath)
        {
            using (var bw = new BinaryWriter(File.OpenWrite(filePath)))
            {
                bw.Write(result, 0, result.Length);
            }
        }

        static byte[] ReadFileAsByteArray(string filePath)
        {
            using (var br = new BinaryReader(File.OpenRead(filePath)))
            {
                byte[] buffer = new byte[br.BaseStream.Length];
                br.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }

        static string GetCountry()
        {
            return Path.GetFileNameWithoutExtension(Path.GetTempFileName()).Substring(2, 2);
        }
    }
}
