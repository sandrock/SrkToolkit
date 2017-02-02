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
    using System.Web;
    using System.Text;
    using System.Diagnostics;
    using System.Web.Routing;
    using SrkToolkit.Web.Services;

    /// <summary>
    /// Helps configure a <see cref="IErrorController"/> to handle all HTTP errors as nice proper HTML pages.
    /// </summary>
    public static class ErrorControllerHandler
    {
        /// <summary>
        /// Registers the specified app for error handling.
        /// </summary>
        /// <typeparam name="TErrorController">The type of the error controller (you should inherit from <see cref="BaseErrorController"/> or <see cref="IErrorController"/>).</typeparam>
        /// <param name="app">The application.</param>
        /// <param name="includeExceptionDetails">if set to <c>true</c> [include exception details].</param>
        public static void Register<TErrorController>(HttpApplication app, bool includeExceptionDetails)
            where TErrorController : IErrorController, new()
        {
            app.Error += (s, e) =>
            {
                var context = HttpContext.Current;
                Handle(context, new TErrorController(), includeExceptionDetails);
            };
        }

        /// <summary>
        /// Handles the specified HTTP errorfull context (typically in Application_Error).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="errorController">The error controller.</param>
        /// <param name="includeExceptionDetails">if set to <c>true</c> [include exception details].</param>
        /// <returns></returns>
        public static Exception Handle(HttpContext context, IErrorController errorController, bool includeExceptionDetails)
        {
            return Handle(context, errorController, includeExceptionDetails, null);
        }

        /// <summary>
        /// Handles the specified HTTP errorfull context (typically in Application_Error).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="errorController">The error controller.</param>
        /// <param name="includeExceptionDetails">if set to <c>true</c> [include exception details].</param>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public static Exception Handle(HttpContext context, IErrorController errorController, bool includeExceptionDetails, RequestContext requestContext)
        {
            Trace.WriteLine("ErrorControllerHandler: Application_Error: begin");

            RouteData routeData;

            if (requestContext != null)
            {
                routeData = requestContext.RouteData;
            }
            else
            {
                routeData = new RouteData();
                requestContext = new RequestContext(new HttpContextWrapper(context), routeData);
            }

            // get the error for the current request
            Exception exception = context.Error;
            context.ClearError();
            HttpException httpException = exception as HttpException;

            // define the correct action depending on the error (500, 404, 403...)
            routeData.Values.Add("controller", "Error");
            if (httpException == null)
            {
                routeData.Values.Add("action", "Internal");
            }
            else
            { // It's an Http Exception, Let's handle it.
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "NotFound");
                        break;
                    case 401:
                    case 403:
                        routeData.Values.Add("action", "Forbidden");
                        break;
                    case 500:
                        routeData.Values.Add("action", "Internal");
                        break;
                    default:
                        routeData.Values.Add("action", "Internal");
                        break;
                }
            }

            // Pass exception details to the target error View.
            routeData.DataTokens.Add(ResultServiceBase.RouteDataExceptionKey, exception);

            // Avoid IIS7 getting in the middle
            context.Response.TrySkipIisCustomErrors = true;

            Trace.WriteLine("ErrorControllerHandler: Application_Error: invoking mvc");

            try
            {
                // Call target Controller and pass the routeData.
                errorController.IncludeExceptionDetails = includeExceptionDetails;
                errorController.Execute(requestContext);
            }
            catch (Exception ex)
            {
                context.Response.Clear();

                Trace.TraceError(ex.ToString());

                if (includeExceptionDetails)
                {
                    BasicHttpErrorResponse.Execute(context, exception, ex);
                }
                else
                {
                    BasicHttpErrorResponse.Execute(context, null);
                }
            }

            Trace.WriteLine("ErrorControllerHandler: Application_Error: end");

            return exception;
        }
    }
}
