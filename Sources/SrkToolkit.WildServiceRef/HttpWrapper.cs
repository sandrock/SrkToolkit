using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace SrkToolkit.WildServiceRef {
    partial class HttpWrapper : IHttpWrapper {

        #region IHttpWrapper Members
#pragma warning disable 1591

        public string GetString(string url) {
            throw new NotImplementedException();
        }

        public string GetString(string url, Dictionary<string, string> postParameters) {
            throw new NotImplementedException();
        }

        public string GetString(string url, Stream postStream) {
            throw new NotImplementedException();
        }

        public Stream GetStream(string url) {
            throw new NotImplementedException();
        }

        public Stream GetStream(string url, Dictionary<string, string> postParameters) {
            throw new NotImplementedException();
        }

        public Stream GetStream(string url, Stream postStream) {
            throw new NotImplementedException();
        }

        public HttpWebResponse GetResponse(string url) {
            throw new NotImplementedException();
        }

        public HttpWebResponse GetResponse(string url, Dictionary<string, string> postParameters) {
            throw new NotImplementedException();
        }

        public HttpWebResponse GetResponse(string url, Stream postStream) {
            throw new NotImplementedException();
        }

        public HttpWebResponse Execute(string url) {

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(new Uri(url, UriKind.Absolute));

            request.AllowAutoRedirect = false;
            request.UserAgent = UserAgent;

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            HandleHttpCodes(response.StatusCode);

            return response;
        }

        public HttpWebResponse Execute(string url, Dictionary<string, string> postParameters) {

            // prepare POST data in memory
            var postDataStream = new MemoryStream();
            string sep = string.Empty;
            foreach (var item in postParameters) {
                var bytes = UTF8Encoding.UTF8.GetBytes(string.Concat(
                    item.Key, '=', HttpTools.PostEncode(item.Value)));
                postDataStream.Write(bytes, 0, bytes.Length);
            }
            postDataStream.Seek(0L, SeekOrigin.Begin);

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && false) {
                string postString = null;
                using (var sr = new StreamReader(postDataStream)) {
                    postString = sr.ReadToEnd();
                }
                postDataStream.Seek(0L, SeekOrigin.Begin);
                System.Diagnostics.Debugger.Break();
            }
#endif

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(new Uri(url, UriKind.Absolute));
            request.AllowAutoRedirect = false;
            request.UserAgent = UserAgent;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // get request stream
            using (var requestStream = request.GetRequestStream()) {
                try {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0) {
                        requestStream.Write(buffer, 0, bytesRead);
                        requestStream.Flush();
                    }
                    requestStream.Close();
                    postDataStream.Close();
                } catch (Exception ex) {
                    throw;
                } finally {
                    postDataStream.Dispose();
                }
            }

            // execute the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            HandleHttpCodes(response.StatusCode);

            return response;
        }

        public HttpWebResponse Execute(string url, Stream postStream) {

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(new Uri(url, UriKind.Absolute));
            request.AllowAutoRedirect = false;
            request.UserAgent = UserAgent;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // get request stream
            using (var requestStream = request.GetRequestStream()) {
                try {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0) {
                        requestStream.Write(buffer, 0, bytesRead);
                        requestStream.Flush();
                    }
                    requestStream.Close();
                    postStream.Close();
                } catch (Exception ex) {
                    //TODO: handle this exception?
                    throw;
                } finally {
                    postStream.Dispose();
                }
            }

            // execute the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            HandleHttpCodes(response.StatusCode);

            return response;
        }

#pragma warning restore 1591
        #endregion
    }
}
