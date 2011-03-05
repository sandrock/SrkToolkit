using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace SrkToolkit.WildServiceRef.Clients {

    partial class BaseHttpClient {

        #region Assisted HTTP query
        
        protected string GetStringResponse(string action, params string[] keyValues) {
            Dictionary<string, string> postParams = null;
            return GetStringResponse(action, HttpTools.GetDictionaryFromKeyValues(keyValues), postParams);
        }
        
        #endregion

        #region HTTP query

        /// <summary>
        /// Execute a HTTP request with optional POST parameters. Returns HTTP response body as a string.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="queryParams"></param>
        /// <param name="postParams"></param>
        /// <returns></returns>
        protected virtual string GetStringResponse(string action, Dictionary<string, string> queryParams, Dictionary<string, string> postParams) {
            string url = string.Format(UrlFormat, BaseUrl, action, HttpTools.GetParamsAsQueryString(queryParams));

            if (postParams == null)
                return http.GetString(url);
            else
                return http.GetString(url, postParams);
        }

        /// <summary>
        /// Execute a HTTP request with optional POST stream. Returns HTTP response body as a string.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="queryParams"></param>
        /// <param name="postStream"></param>
        /// <returns></returns>
        protected virtual string GetStringResponse(string action, Dictionary<string, string> queryParams, Stream postStream) {
            string url = string.Format(UrlFormat, BaseUrl, action, HttpTools.GetParamsAsQueryString(queryParams));

            if (postStream == null)
                return http.GetString(url);
            else
                return http.GetString(url, postStream);
        }

        /// <summary>
        /// Execute a HTTP request with optional POST parameters. Returns HTTP response body as a stream.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="queryParams"></param>
        /// <param name="postParams"></param>
        /// <returns></returns>
        protected virtual Stream GetStreamResponse(string action, Dictionary<string, string> queryParams, Dictionary<string, string> postParams) {
            string url = string.Format(UrlFormat, BaseUrl, action, HttpTools.GetParamsAsQueryString(queryParams));

            if (postParams == null)
                return http.GetStream(url);
            else
                return http.GetStream(url, postParams);
        }

        /// <summary>
        /// Execute a HTTP request with optional POST stream. Returns HTTP response body as a stream.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="queryParams"></param>
        /// <param name="postStream"></param>
        /// <returns></returns>
        protected virtual Stream GetStreamResponse(string action, Dictionary<string, string> queryParams, Stream postStream) {
            string url = string.Format(UrlFormat, BaseUrl, action, HttpTools.GetParamsAsQueryString(queryParams));

            if (postStream == null)
                return http.GetStream(url);
            else
                return http.GetStream(url, postStream);
        }

        /// <summary>
        /// Execute a HTTP request with optional POST parameters. Returns a full HTTP response.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="queryParams"></param>
        /// <param name="postParams"></param>
        /// <returns></returns>
        protected virtual HttpWebResponse GetFullResponse(string action, Dictionary<string, string> queryParams, Dictionary<string, string> postParams) {
            string url = string.Format(UrlFormat, BaseUrl, action, HttpTools.GetParamsAsQueryString(queryParams));

            if (postParams == null)
                return http.GetResponse(url);
            else
                return http.GetResponse(url, postParams);
        }

        /// <summary>
        /// Execute a HTTP request with optional POST stream. Returns a full HTTP response.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="queryParams"></param>
        /// <param name="postStream"></param>
        /// <returns></returns>
        protected virtual HttpWebResponse GetFullResponse(string action, Dictionary<string, string> queryParams, Stream postStream) {
            string url = string.Format(UrlFormat, BaseUrl, action, HttpTools.GetParamsAsQueryString(queryParams));

            if (postStream == null)
                return http.GetResponse(url);
            else
                return http.GetResponse(url, postStream);
        }

        #endregion

    }
}
