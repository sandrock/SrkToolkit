
namespace System.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for classes <see cref="HttpRequestBase"/> and <see cref="HttpRequest"/>
    /// </summary>
    public static class SrkHttpRequestExtensions
    {
        /// <summary>
        /// Determines whether the specified URL is local to the request's host.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsUrlLocalToHost(this HttpRequestBase request, string url)
        {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        /// <summary>
        /// Determines whether the specified URL is local to the request's host.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsUrlLocalToHost(this HttpRequest request, string url)
        {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }
    }
}
