using Microarea.Mago4Butler.BL.PAASUpdates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class HttpService : ILogger
    {
        readonly ISettings settings;
        const string loginAddress = "http://www.microarea.it/common/Login.aspx";

        public HttpService(ISettings settings)
        {
            this.settings = settings;
        }
        public void DownloadFile(string address, string filePath)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Host = "www.microarea.it";
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:47.0) Gecko/20100101 Firefox/47.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("it-IT"));
                httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("it"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                httpClient.DefaultRequestHeaders.Referrer = new Uri("http://www.microarea.it/common/Login.aspx");
                httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                httpClient.DefaultRequestHeaders.ExpectContinue = false;

                var content = new StringContent(
                    "__LASTFOCUS=&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUJMTkzODUxNzI3D2QWAmYPZBYCZg9kFgICAQ8WBB4IeG1sOmxhbmcFAml0HgRsYW5nBQJpdBYCAgMQZGQWCAILD2QWAgIBDxYCHgtwbGFjZWhvbGRlcgUFQ2VyY2FkAhEPZBYCAgMPZBYEAgUPZBYCZg8PZBYCHgVzdHlsZQUUdmlzaWJpbGl0eTogdmlzaWJsZTsWBAITDxAPFgIeBFRleHQFHFJpY29yZGFtaSBzdSBxdWVzdG8gY29tcHV0ZXJkZGRkAhcPDxYCHgdWaXNpYmxlaGRkAgcPDxYCHwVoZGQCFQ9kFgICAQ8WAh8FZ2QCGw9kFgQCAQ8PFgIfBWhkZAIFDw8WAh8FaGRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBU5jdGwwMCRjdGwwMCRNaWRkbGVQbGFjZUhvbGRlciRQYWdlTWFpbkNvbnRlbnRIb2xkZXIkTXlQYW5lbExvZ2luJGNoa1JlbWVtYmVyTWXoEFZ1jLbyNuvzS1GqxwozlmyPbA%3D%3D&__VIEWSTATEGENERATOR=CC3D53C2&__EVENTVALIDATION=%2FwEdAAoAK6gk9kHh14kq1SdkRmUsDjFkuAnytIfcLnFtbaXa9vrcGtP%2FE%2FClssnRmFceMynDDU%2BRV%2FjOi%2BvSugvNJrSXHVyaFPRXvCovL8JjX7o2SCZiBC%2BeMyJZj63%2BpSPS5nBzY%2BlcHchuSZ0StYDF%2B7HDWxwVx6LwHQX4%2BKm8WbBL4ENjjpGhxieh0HXKnH19o%2BEaqT21mDu9wAin9C3bkMWVYKIq0020w03z4u9BoBjLLUbZ9gk%3D&ctl00%24ctl00%24txtSearch2=&ctl00%24ctl00%24txtSearch=&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24txtUserName=mcanessa&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24pwdUserPass=Lunedi06Elisa&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24ImageButtonLogin=Accedi",
                    Encoding.ASCII,
                    "application/x-www-form-urlencoded"
                    );

                var responseTask = httpClient.PostAsync(loginAddress, content);
                responseTask.Wait();
                //login effettuata, scarico l'msi...

                responseTask = httpClient.GetAsync(address);
                responseTask.Wait();
                var contentTask = responseTask.Result.Content.ReadAsByteArrayAsync();
                contentTask.Wait();
                using (var outputStream = File.Create(filePath))
                {
                    outputStream.Write(contentTask.Result, 0, contentTask.Result.Length);
                }
            }
        }

        public GetUpdatesResponse GetUpdates(Version version)
        {
            GetUpdatesResponse response = null;
            using (UpdatesServiceSoapClient svcClient = new UpdatesServiceSoapClient())
            {
                var request = new GetUpdatesRequest() { DeclaredVersion = version.ToString(), ProductSignature = "M4GO" };

                var address = this.settings.UpdatesUri;
#if DEBUG
                address = "http://localhost/PAASUpdates/UpdatesService.asmx";
#endif
                svcClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(address);

                response = svcClient.GetUpdates(request);
            }

            return response;
        }
    }
}
