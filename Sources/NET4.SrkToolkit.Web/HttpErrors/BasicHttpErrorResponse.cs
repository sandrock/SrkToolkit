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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Helps ouput a plain text error page to a HTTP response.
    /// </summary>
    public static class BasicHttpErrorResponse
    {
        /// <summary>
        /// Outputs a plain text error page including exception details.
        /// </summary>
        /// <param name="context">the HTTP context</param>
        /// <param name="originalError">the first exception (optionnal)</param>
        /// <param name="extraError">a subsequent exception (optionnal)</param>
        /// <param name="message">an optionnal message to display</param>
        public static void Execute(HttpContext context, Exception originalError = null, Exception extraError = null, string message = null)
        {
            if (context == null)
                return;

            WriteHeader(context, message);

            if (originalError != null)
            {
                context.Response.Write("" + Environment.NewLine);
                context.Response.Write("DEBUG INFORMATION" + Environment.NewLine);
                context.Response.Write("-------------------------------" + Environment.NewLine);
                context.Response.Write("" + Environment.NewLine);

                WriteException(context, "Source error", originalError);
            }
         
            if (extraError != null)
            {
                WriteException(context, "Subsequent error", extraError);
            }
        }

        private static void WriteException(HttpContext context, string title, Exception exception)
        {
            context.Response.Write(title.ToUpperInvariant() + Environment.NewLine);
            context.Response.Write("" + Environment.NewLine);
            OutputExceptionDetails(context, exception);
            context.Response.Write("" + Environment.NewLine);
        }

        private static void WriteHeader(HttpContext context, string message)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write("HTTP " + context.Response.StatusCode + " error" + Environment.NewLine);
            context.Response.Write("-------------------------------" + Environment.NewLine);
            context.Response.Write("" + Environment.NewLine);
            context.Response.Write("There was a problem serving your request." + Environment.NewLine);
            context.Response.Write("Maintenance team is working on this." + Environment.NewLine);
            context.Response.Write("" + Environment.NewLine);
            context.Response.Write("" + Environment.NewLine);
            if (message != null)
                context.Response.Write(message + Environment.NewLine);
            context.Response.Write("" + Environment.NewLine);
        }

        private static void OutputExceptionDetails(HttpContext context, Exception ex)
        {
            Exception exc = ex;
            int i = 0;
            while (exc != null)
            {
                if (i++ == 0)
                    context.Response.Write("outer exception: ");
                else
                    context.Response.Write("inner " + i + " exception: ");
                context.Response.Write(exc.GetType().FullName + Environment.NewLine);
                context.Response.Write("   " + exc.Message + Environment.NewLine);
                context.Response.Write("    " + exc.StackTrace + Environment.NewLine);
                context.Response.Write("" + Environment.NewLine);
                exc = exc.InnerException;
            }

            context.Response.Write("" + Environment.NewLine);
        }
    }
}
