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

namespace SrkToolkit.Web.HttpErrors
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Writes a plain-text error page directly to an HTTP response.
    /// Useful as a last-resort fallback when the MVC pipeline cannot render a view.
    /// </summary>
    public static class BasicHttpErrorResponse
    {
        /// <summary>
        /// Writes a plain-text error page including exception details to the response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="originalError">The first exception (optional).</param>
        /// <param name="extraError">A subsequent exception (optional).</param>
        /// <param name="message">An optional message to display.</param>
        public static async Task Execute(HttpContext context, Exception originalError = null, Exception extraError = null, string message = null)
        {
            if (context == null)
                return;

            await WriteHeader(context, message);

            if (originalError != null)
            {
                await context.Response.WriteAsync(Environment.NewLine);
                await context.Response.WriteAsync("DEBUG INFORMATION" + Environment.NewLine);
                await context.Response.WriteAsync("-------------------------------" + Environment.NewLine);
                await context.Response.WriteAsync(Environment.NewLine);
                await WriteException(context, "Source error", originalError);
            }

            if (extraError != null)
            {
                await WriteException(context, "Subsequent error", extraError);
            }
        }

        private static async Task WriteException(HttpContext context, string title, Exception exception)
        {
            await context.Response.WriteAsync(title.ToUpperInvariant() + Environment.NewLine);
            await context.Response.WriteAsync(Environment.NewLine);
            await OutputExceptionDetails(context, exception);
            await context.Response.WriteAsync(Environment.NewLine);
        }

        private static async Task WriteHeader(HttpContext context, string message)
        {
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync("HTTP " + context.Response.StatusCode + " error" + Environment.NewLine);
            await context.Response.WriteAsync("-------------------------------" + Environment.NewLine);
            await context.Response.WriteAsync(Environment.NewLine);
            await context.Response.WriteAsync("There was a problem serving your request." + Environment.NewLine);
            await context.Response.WriteAsync("Maintenance team is working on this." + Environment.NewLine);
            await context.Response.WriteAsync(Environment.NewLine);
            await context.Response.WriteAsync(Environment.NewLine);
            if (message != null)
                await context.Response.WriteAsync(message + Environment.NewLine);
            await context.Response.WriteAsync(Environment.NewLine);
        }

        private static async Task OutputExceptionDetails(HttpContext context, Exception ex)
        {
            var exc = ex;
            int i = 0;
            while (exc != null)
            {
                var label = i++ == 0 ? "outer exception: " : "inner " + i + " exception: ";
                await context.Response.WriteAsync(label + exc.GetType().FullName + Environment.NewLine);
                await context.Response.WriteAsync("   " + exc.Message + Environment.NewLine);
                await context.Response.WriteAsync("    " + exc.StackTrace + Environment.NewLine);
                await context.Response.WriteAsync(Environment.NewLine);
                exc = exc.InnerException;
            }

            await context.Response.WriteAsync(Environment.NewLine);
        }
    }
}
