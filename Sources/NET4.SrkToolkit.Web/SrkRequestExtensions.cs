// -----------------------------------------------------------------------
// <copyright file="SrkRequestExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Extension methods for the <see cref="HttpRequest"/> and <see cref="HttpRequestBase"/> classes.
    /// </summary>
    public static class SrkRequestExtensions
    {
        /// <summary>
        /// Determines whether the specified request is made via AJAX.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if [is XML HTTP request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsXmlHttpRequest(this HttpRequest request)
        {
            var header = request.Headers["X-Requested-With"];
            if (string.IsNullOrEmpty(header))
                return false;

            string[] values = new string[]
            {
                "XMLHttpRequest".ToUpperInvariant(),
                "XHR",
            };
            return values.Contains(header.ToUpperInvariant());
        }

        /// <summary>
        /// Determines whether the specified request is made via AJAX.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if [is XML HTTP request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsXmlHttpRequest(this HttpRequestBase request)
        {
            var header = request.Headers["X-Requested-With"];
            if (string.IsNullOrEmpty(header))
                return false;

            string[] values = new string[]
            {
                "XMLHttpRequest".ToUpperInvariant(),
                "XHR",
            };
            return values.Contains(header.ToUpperInvariant());
        }

        /// <summary>
        /// Determines whether [is HTTP post request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpPostRequest(this HttpRequest request)
        {
            return request.HttpMethod.ToUpperInvariant() == "POST";
        }

        /// <summary>
        /// Determines whether [is HTTP post request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpPostRequest(this HttpRequestBase request)
        {
            return request.HttpMethod.ToUpperInvariant() == "POST";
        }

        /// <summary>
        /// Determines whether the specified URL is local to the request's host.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsUrlLocalToHost(this HttpRequestBase request, string url)
        {
            return IsUrlLocalToHost(url);
        }

        /// <summary>
        /// Determines whether the specified URL is local to the request's host.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsUrlLocalToHost(this HttpRequest request, string url)
        {
            return IsUrlLocalToHost(url);
        }

        private static bool IsUrlLocalToHost(string url)
        {
            return !string.IsNullOrEmpty(url) 
                &&
                (
                    (url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\')))
                 || (url.Length > 1 && url[0] == '~' && url[1] == '/')
                );
        }

        /// <summary>
        /// Determines whether the client prefers a JSON response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>true is the first accept type is JSON; otherwise, false</returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public static bool PrefersJson(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.AcceptTypes == null || request.AcceptTypes.Length == 0)
                return false;

            return PrefersJson(request.AcceptTypes);
        }

        public static bool PrefersJson(this HttpRequestBase request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.AcceptTypes == null || request.AcceptTypes.Length == 0)
                return false;

            return PrefersJson(request.AcceptTypes);
        }

        private static bool PrefersJson(string[] acceptTypes)
        {
            if (acceptTypes[0].StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (acceptTypes[0].StartsWith("text/json", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }
    }
}
