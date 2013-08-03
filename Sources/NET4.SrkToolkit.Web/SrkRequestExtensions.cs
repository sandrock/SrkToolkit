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
    }
}
