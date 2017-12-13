using Microarea.Mago4Butler.BL.PAASUpdates;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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
            var cookies = new CookieContainer();

            var handler = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true
            };
            if (this.settings.UseProxy)
            {
                var proxyUrl = string.Format("{0}:{1}", this.settings.ProxyServerUrl, this.settings.ProxyServerPort);
                handler.Proxy = new WebProxy(proxyUrl, false);
                handler.UseProxy = true;

                if (this.settings.UseCredentials)
                {
                    handler.Credentials = new NetworkCredential(this.settings.Username, this.settings.Password);
                }
            }

            using (var httpClient = new HttpClient(handler, true))
            {
                httpClient.Timeout = new TimeSpan(0, 15, 0);
                int timeoutMillSecs = Convert.ToInt32(httpClient.Timeout.TotalMilliseconds);

                //Richiedo in get la pagina di login per non allarmare il firewall
                var loginPageRequest = httpClient.GetAsync(new Uri("http://www.microarea.it/common/Login.aspx"));
                loginPageRequest.Wait(timeoutMillSecs);
                var loginPageContentTask = loginPageRequest.Result.Content.ReadAsStringAsync();
                loginPageContentTask.Wait(timeoutMillSecs);

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

                string viewState = GetValue(loginPageContentTask.Result, "__VIEWSTATE");
                if (viewState == null)
                {
                    throw new Exception("I cannot read the view state from the login page");
                }

                string viewStateGenerator = GetValue(loginPageContentTask.Result, "__VIEWSTATEGENERATOR");
                if (viewStateGenerator == null)
                {
                    throw new Exception("I cannot read the view state generator from the login page");
                }

                string eventValidation = GetValue(loginPageContentTask.Result, "__EVENTVALIDATION");
                if (eventValidation == null)
                {
                    throw new Exception("I cannot read the event validation section from the login page");
                }

                var userName = "M4ButlerPAAS";
                var password = "Microarea.";

                var content = new StringContent(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&ctl00%24ctl00%24txtSearch2=&ctl00%24ctl00%24txtSearch=&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24txtUserName={3}&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24pwdUserPass={4}&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24ImageButtonLogin=Accedi",
                        viewState,
                        viewStateGenerator,
                        eventValidation,
                        userName,
                        password
                        ),
                    Encoding.ASCII,
                    "application/x-www-form-urlencoded"
                    );
                //var content = new StringContent(
                //    "__LASTFOCUS=&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUJNDQzOTU4MDM3D2QWAmYPZBYCZg9kFgICAQ8WBB4IeG1sOmxhbmcFAml0HgRsYW5nBQJpdBYCAgMQZGQWBgILD2QWAgIBDxYCHgtwbGFjZWhvbGRlcgUFQ2VyY2FkAhEPZBYCAgMPZBYGAgEPZBYCZg8PFgIeB1Zpc2libGVoZGQCBQ9kFgJmDw9kFgIeBXN0eWxlBRR2aXNpYmlsaXR5OiB2aXNpYmxlOxYEAhMPEA8WAh4EVGV4dAUcUmljb3JkYW1pIHN1IHF1ZXN0byBjb21wdXRlcmRkZGQCFw8PFgIfA2hkZAIHDw8WAh8DaGRkAhkPZBYEAgEPDxYCHwNoZGQCBQ8PFgIfA2hkZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAQVOY3RsMDAkY3RsMDAkTWlkZGxlUGxhY2VIb2xkZXIkUGFnZU1haW5Db250ZW50SG9sZGVyJE15UGFuZWxMb2dpbiRjaGtSZW1lbWJlck1laa9x1iJM5NlhG6oasUHDMpjdveE%3D&__VIEWSTATEGENERATOR=CC3D53C2&__EVENTVALIDATION=%2FwEdAAqapJgRgMG6O9CAGUdocEOGDjFkuAnytIfcLnFtbaXa9vrcGtP%2FE%2FClssnRmFceMynDDU%2BRV%2FjOi%2BvSugvNJrSXHVyaFPRXvCovL8JjX7o2SCZiBC%2BeMyJZj63%2BpSPS5nBzY%2BlcHchuSZ0StYDF%2B7HDWxwVx6LwHQX4%2BKm8WbBL4ENjjpGhxieh0HXKnH19o%2BEaqT21mDu9wAin9C3bkMWVlLQ%2FJYfh%2FbHughZGOMroHyj7RlY%3D&ctl00%24ctl00%24txtSearch2=&ctl00%24ctl00%24txtSearch=&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24txtUserName=M4ButlerPAAS&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24pwdUserPass=Microarea.&ctl00%24ctl00%24MiddlePlaceHolder%24PageMainContentHolder%24MyPanelLogin%24ImageButtonLogin=Accedi",
                //    Encoding.ASCII,
                //    "application/x-www-form-urlencoded"
                //    );

                //Post per effettuare login
                var responseTask = httpClient.PostAsync(loginAddress, content);
                responseTask.Wait(timeoutMillSecs);

                //login effettuata, ora posso scaricare l'msi...
                responseTask = httpClient.GetAsync(address);
                responseTask.Wait(timeoutMillSecs);

                if (responseTask.Result.Content.Headers.ContentLength < 1000000)//1MB
                {
                    throw new Exception("I cannot download the msi file, maybe the login is not correct?");
                }

                using (var outputStream = File.Create(filePath))
                {
                    var contentTask = responseTask.Result.Content.CopyToAsync(outputStream);
                    contentTask.Wait(timeoutMillSecs);
                }
            }
        }

        private static string GetValue(string htmlPage, string field)
        {
            var idIdx = htmlPage.IndexOf("id=\"" + field + "\"");
            if (idIdx < 0)
            {
                return null;
            }
            var startIdx = htmlPage.IndexOf("value=\"", idIdx + 1) + "value=\"".Length;
            var endIdx = htmlPage.IndexOf("\"", startIdx + 1);

            var str = htmlPage.Substring(startIdx, endIdx - startIdx);

            return System.Uri.EscapeDataString(str);
        }

        public GetUpdatesResponse GetUpdates(Model.Version version)
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
