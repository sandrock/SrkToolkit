// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

namespace SrkToolkit.Web
{
#if ASPMVCCORE
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
#endif
    
#if ASPMVC
    using System.Web;
    using System.Web.Mvc;
#endif

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for the <see cref="HttpRequest"/> classes.
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
        public static bool IsXmlHttpRequest(
#if ASPMVCCORE
            this HttpRequest request
#elif ASPMVC
            this HttpRequestBase request
#endif
            )
        {
            var header = request.Headers["X-Requested-With"];
            if (string.IsNullOrEmpty(header))
                return false;

#if ASPMVCCORE
            return xhrValues.Any(searchValue => header.Any(value => value.Equals(searchValue, StringComparison.OrdinalIgnoreCase)));
#elif ASPMVC
            return xhrValues.Contains(header.ToUpperInvariant());
#endif
        }

        /// <summary>
        /// Determines whether the HTTP method is GET.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpGetRequest(
#if ASPMVCCORE
            this HttpRequest request
#elif ASPMVC
            this HttpRequestBase request
#endif
            )
        {

#if ASPMVCCORE
            return "GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
#elif ASPMVC
            return "GET".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
#endif
        }

        /// <summary>
        /// Determines whether the HTTP method is POST.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpPostRequest(
#if ASPMVCCORE
            this HttpRequest request
#elif ASPMVC
            this HttpRequestBase request
#endif
            )
        {

#if ASPMVCCORE
            return "POST".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
#elif ASPMVC
            return "POST".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
#endif
        }

        /// <summary>
        /// Determines whether the HTTP method is DELETE.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpDeleteRequest(
#if ASPMVCCORE
            this HttpRequest request
#elif ASPMVC
            this HttpRequestBase request
#endif
            )
        {

#if ASPMVCCORE
            return "DELETE".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
#elif ASPMVC
            return "DELETE".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
#endif
        }

        /// <summary>
        /// Determines whether the HTTP method is PUT.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpPutRequest(
#if ASPMVCCORE
            this HttpRequest request
#elif ASPMVC
            this HttpRequestBase request
#endif
            )
        {

#if ASPMVCCORE
            return "PUT".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
#elif ASPMVC
            return "PUT".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
#endif
        }

        /// <summary>
        /// Determines whether the HTTP method is HEAD.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static bool IsHttpHeadRequest(
#if ASPMVCCORE
            this HttpRequest request
#elif ASPMVC
            this HttpRequestBase request
#endif
            )
        {

#if ASPMVCCORE
            return "HEAD".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
#elif ASPMVC
            return "HEAD".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
#endif
        }

        /// <summary>
        /// Determines whether the specified URL is local to the request's host.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsUrlLocalToHost(
#if ASPMVCCORE
            this HttpRequest request,
#elif ASPMVC
            this HttpRequestBase request,
#endif
          string url)
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
        public static bool PrefersJson(
#if ASPMVCCORE
            this HttpRequest request
#elif ASPMVC
            this HttpRequestBase request
#endif
            )
        {
            if (request == null)
                throw new ArgumentNullException("request");


#if ASPMVCCORE
            return PrefersJson(request.Headers["Accept"]);
#elif ASPMVC
            return PrefersJson(request.AcceptTypes);
#endif
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
