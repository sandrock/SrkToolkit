
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
        private static readonly string[] xhrValues = new string[]
        {
            "XMLHttpRequest".ToUpperInvariant(),
            "XHR",
        };

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

            return xhrValues.Contains(header.ToUpperInvariant());
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

            return xhrValues.Contains(header.ToUpperInvariant());
        }

        /// <summary>
        /// Determines whether the HTTP method is POST.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpPostRequest(this HttpRequest request)
        {
            return "POST".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the HTTP method is POST.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpPostRequest(this HttpRequestBase request)
        {
            return "POST".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the HTTP method is DELETE.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpDeleteRequest(this HttpRequest request)
        {
            return "DELETE".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether is HTTP method is DELETE.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpDeleteRequest(this HttpRequestBase request)
        {
            return "DELETE".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the HTTP method is PUT.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpPutRequest(this HttpRequest request)
        {
            return "PUT".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether is HTTP method is PUT.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpPutRequest(this HttpRequestBase request)
        {
            return "PUT".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the HTTP method is HEAD.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpHeadRequest(this HttpRequest request)
        {
            return "HEAD".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether is HTTP method is HEAD.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpHeadRequest(this HttpRequestBase request)
        {
            return "HEAD".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
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
        /// <returns>true if the first accept type is JSON; otherwise, false</returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public static bool PrefersJson(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return PrefersJson(request.AcceptTypes);
        }

        /// <summary>
        /// Determines whether the client prefers a JSON response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>true if the first accept type is JSON; otherwise, false</returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public static bool PrefersJson(this HttpRequestBase request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return PrefersJson(request.AcceptTypes);
        }

        private static bool PrefersJson(string[] acceptTypes)
        {
            if (acceptTypes == null || acceptTypes.Length == 0)
                return false;

            if (acceptTypes[0].StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (acceptTypes[0].StartsWith("text/json", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }
    }
}
