using CefSharp;
using Microarea.Mago4Butler.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    internal class ButlerSchemeHandler : IResourceHandler
    {
        readonly IDictionary<string, string> resources;
        MemoryStream stream;
        string mimeType;

        //private readonly IDictionary<string, Bitmap> images;

        public ButlerSchemeHandler()
        {
            resources = new Dictionary<string, string>
            {
                { "/index.html", Resources.Index },
                { "/js/app.js", Resources.app }

                //{ "/js/foundation.min.js", Resources.foundation_min_js },

                //{ "/js/vendor/fastclick.js", Resources.fastclick_js },
                //{ "/js/vendor/jquery.cookie.js", Resources.jquery_cookie_js },
                //{ "/js/vendor/jquery.js", Resources.jquery_js },
                //{ "/js/vendor/modernizr.js", Resources.modernizr_js },
                //{ "/js/vendor/placeholder.js", Resources.placeholder_js },
                //{ "/js/vendor/velocity.js", Resources.velocity_js },
                //{ "/js/vendor/spin.js", Resources.spin_js },

                //{ "/js/foundation/foundation.dropdown.js", Resources.foundation_dropdown_js },
                //{ "/js/foundation/foundation.interchange.js", Resources.foundation_interchange_js },
                //{ "/js/foundation/foundation.js", Resources.foundation_js },
                //{ "/js/foundation/foundation.reveal.js", Resources.foundation_reveal_js },
                //{ "/js/app.js", Resources.app_js },
                //{ "/js/msiUpdateService.js", Resources.msiUpdateService_js },

                //{ "/css/foundation.css", Resources.foundation_css },
                //{ "/css/foundation.min.css", Resources.foundation_min_css },
                //{ "/css/normalize.css", Resources.normalize_css },
                //{ "/css/app.css", Resources.app_css },
            };

            //images = new Dictionary<string, Bitmap>
            //{
            //    // { "/img/green-checkmark-and-red-minus-hi.png", Resources.green_checkmark_and_red_minus_hi }
            //};
        }

        //public bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback)
        //{
    //    // The 'host' portion is entirely ignored by this scheme handler.
    //    var uri = new Uri(request.Url);
    //    var fileName = uri.AbsolutePath;

    //    string resource;
    //        if (resources.TryGetValue(fileName, out resource) &&
    //            !String.IsNullOrEmpty(resource))
    //        {
    //            var bytes = Encoding.UTF8.GetBytes(resource);
    //    response.ResponseStream = new MemoryStream(bytes);
    //    response.MimeType = GetMimeType(fileName);
    //            requestCompletedCallback();

    //            return true;
    //        }
    //Bitmap image = null;
    //        if (images.TryGetValue(fileName, out image) &&
    //            image != null)
    //        {
    //            MemoryStream imageStream = new MemoryStream();
    //image.Save(imageStream, image.RawFormat);
    //            response.ResponseStream = imageStream;
    //            response.MimeType = GetMimeType(fileName);
    //            requestCompletedCallback();

    //            return true;
    //        }

    //        return false;
        //}

        public bool ProcessRequest(IRequest request, ICallback callback)
        {
            // The 'host' portion is entirely ignored by this scheme handler.
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            string resource;
            if (resources.TryGetValue(fileName, out resource) && !string.IsNullOrEmpty(resource))
            {
                Task.Run(() =>
                {
                    using (callback)
                    {
                        var bytes = Encoding.UTF8.GetBytes(resource);
                        stream = new MemoryStream(bytes);

                        var fileExtension = Path.GetExtension(fileName);
                        mimeType = ResourceHandler.GetMimeType(fileExtension);

                        callback.Continue();
                    }
                });

                return true;
            }
            else
            {
                callback.Dispose();
            }

            return false;
        }

        public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = stream == null ? 0 : stream.Length;
            redirectUrl = null;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusText = HttpStatusCode.OK.ToString();
            response.MimeType = mimeType;
        }

        public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            //Dispose the callback as it's an unmanaged resource, we don't need it in this case
            callback.Dispose();

            if (stream == null)
            {
                bytesRead = 0;
                return false;
            }

            //Data out represents an underlying buffer (typically 32kb in size).
            var buffer = new byte[dataOut.Length];
            bytesRead = stream.Read(buffer, 0, buffer.Length);

            dataOut.Write(buffer, 0, buffer.Length);

            return bytesRead > 0;
        }

        public bool CanGetCookie(CefSharp.Cookie cookie)
        {
            return true;
        }

        public bool CanSetCookie(CefSharp.Cookie cookie)
        {
            return true;
        }

        public void Cancel()
        {
            
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool managed)
        {
            
        }
    }
}
