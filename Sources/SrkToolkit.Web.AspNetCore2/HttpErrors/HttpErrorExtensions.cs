// © SandRock, generated 2026-05-31
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
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using SrkToolkit.Web.Models;
    using SrkToolkit.Web.Services;
    using System;
    using System.Diagnostics;
    using System.Net;

    /// <summary>
    /// Extension methods on <see cref="Controller"/> for producing HTTP error responses.
    /// Use these when you cannot inherit <see cref="BaseErrorController"/> because your
    /// controller already extends another base class.
    /// </summary>
    /// <remarks>
    /// Typical usage in your own error controller:
    /// <code>
    /// public class ErrorController : MyBaseController
    /// {
    ///     [Route("Error/Show/{code:int}")]
    ///     public ActionResult Show(int code) => this.HttpErrorShow(code);
    /// }
    /// </code>
    /// </remarks>
    public static class HttpErrorExtensions
    {
        /// <summary>
        /// Handles the generic error entry point used by
        /// <c>UseExceptionHandler</c> and <c>UseStatusCodePagesWithReExecute</c>.
        /// Equivalent to <see cref="BaseErrorController.Show"/>.
        /// </summary>
        public static ActionResult HttpErrorShow(
            this Controller controller, 
            int code, 
            bool includeExceptionDetails = false,
            string viewName = "Error")
        {
            if (!Enum.IsDefined(typeof(HttpStatusCode), code))
            {
                code = 500;
            }

            var statusCodeFeature = controller.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var exceptionFeature = controller.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var model = HttpErrorModel.Create(code, null, null);
            model.UrlPath = statusCodeFeature?.OriginalPath
                ?? exceptionFeature?.Path
                ?? controller.Request.Path.ToString();

            if (includeExceptionDetails)
            {
                model.Exception = exceptionFeature?.Error;
            }

            return controller.HttpErrorWork("Show", model, code, viewName: viewName);
        }

        /// <summary>
        /// Produces the error response: JSON for AJAX/JSON requests,
        /// <c>View("Error", model)</c> otherwise. Equivalent to <see cref="BaseErrorController"/>'s
        /// internal <c>Work</c> method.
        /// </summary>
        /// <param name="controller">The calling controller.</param>
        /// <param name="action">Logical action name used in tracing and the model.</param>
        /// <param name="model">Error model; title and message are filled from resx when empty.</param>
        /// <param name="code">HTTP status code to set on the response.</param>
        /// <param name="onReady">Optional callback invoked just before the response is written.</param>
        public static ActionResult HttpErrorWork(
            this Controller controller,
            string action,
            HttpErrorModel model,
            int code,
            Action<string, HttpErrorModel, int> onReady = null,
            string viewName = "Error")
        {
            Trace.WriteLine("ErrorController." + action + ": begin");

            if (model.UrlPath == null)
            {
                var statusCodeFeature = controller.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                var exceptionFeature = controller.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                model.UrlPath = statusCodeFeature?.OriginalPath
                    ?? exceptionFeature?.Path
                    ?? controller.Request.Path.ToString();
            }

            if (string.IsNullOrEmpty(model.Title))
            {
                model.Title = "HTTP " + model.Code + " — " + model.CodeName;
            }

            if (string.IsNullOrEmpty(model.Message))
            {
                model.Message = model.CodeDefinition;
            }

            model.ErrorAction = action;
            controller.Response.StatusCode = code;

            onReady?.Invoke(action, model, code);

            Trace.WriteLine("ErrorController." + action + ": end");

            if (controller.Request.IsXmlHttpRequest() || controller.Request.PrefersJson())
            {
                return new ResultServiceBase(controller.HttpContext)
                    .JsonErrorWithException(code, action, model.Message ?? "Unknown error.", model.Exception);
            }

            controller.Response.ContentType = "text/html; charset=utf-8";
            return controller.View(viewName, model);
        }
    }
}
