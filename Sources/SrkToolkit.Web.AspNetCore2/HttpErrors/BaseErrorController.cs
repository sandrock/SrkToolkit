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

    // Recommended startup wiring:
    //
    //   app.UseExceptionHandler("/Error/Show/500");
    //   app.UseStatusCodePagesWithReExecute("/Error/Show/{0}");
    //
    // Both routes reach Show(int code), which handles both IExceptionHandlerPathFeature
    // (unhandled exceptions) and IStatusCodeReExecuteFeature (status-code pages).

    /// <summary>
    /// Base controller implementing <see cref="IErrorController"/>. Everything is overridable.
    /// Subclass this in your app and register the routes shown above in startup.
    /// </summary>
    public class BaseErrorController : Controller, IErrorController
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include exception details in error pages.
        /// </summary>
        public bool IncludeExceptionDetails { get; set; }

        // --- Primary entry point (UseStatusCodePagesWithReExecute / UseExceptionHandler) ---

        /// <summary>
        /// Generic error action. Configure in startup as:
        /// <c>app.UseStatusCodePagesWithReExecute("/Error/Show/{0}");</c>
        /// <c>app.UseExceptionHandler("/Error/Show/500");</c>
        /// </summary>
        [Route("Error/Show/{code:int}")]
        public virtual ActionResult Show(int code)
        {
            if (!Enum.IsDefined(typeof(HttpStatusCode), code))
            {
                code = 500;
            }

            var statusCodeFeature = this.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var exceptionFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var model = HttpErrorModel.Create(code, null, null);
            model.UrlPath = statusCodeFeature?.OriginalPath
                ?? exceptionFeature?.Path
                ?? this.Request.Path.ToString();

            if (this.IncludeExceptionDetails)
            {
                model.Exception = exceptionFeature?.Error;
            }

            return this.Work("Show", model, code);
        }

        // --- Per-code actions (optional; useful when routing directly to named actions) ---

        /// <inheritdoc />
        public virtual ActionResult Forbidden() => this.Work("Forbidden", HttpErrorModel.Create(403, null, null), 403);

        /// <inheritdoc />
        public virtual ActionResult NotFound() => this.Work("NotFound", HttpErrorModel.Create(404, null, null), 404);

        /// <inheritdoc />
        public virtual ActionResult Gone() => this.Work("Gone", HttpErrorModel.Create(410, null, null), 410);

        /// <inheritdoc />
        public virtual ActionResult BadRequest() => this.Work("BadRequest", HttpErrorModel.Create(400, null, null), 400);

        /// <inheritdoc />
        public virtual ActionResult MethodNotAllowed() => this.Work("MethodNotAllowed", HttpErrorModel.Create(405, null, null), 405);

        /// <inheritdoc />
        public virtual ActionResult Internal() => this.Work("Internal", HttpErrorModel.Create(500, null, null), 500);

        // --- Shared implementation ---

        /// <summary>
        /// Produces the error response. Returns a JSON envelope for AJAX/JSON requests,
        /// <c>View("Error", model)</c> otherwise. Override to customise view name or layout.
        /// </summary>
        protected virtual ActionResult Work(string action, HttpErrorModel model, int code)
        {
            Trace.WriteLine("ErrorController." + action + ": begin");

            // When called from per-code actions, resolve original URL from middleware features.
            if (model.UrlPath == null)
            {
                var statusCodeFeature = this.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                var exceptionFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                model.UrlPath = statusCodeFeature?.OriginalPath
                    ?? exceptionFeature?.Path
                    ?? this.Request.Path.ToString();
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
            this.Response.StatusCode = code;

            this.OnErrorResponseReady(action, model, code);

            Trace.WriteLine("ErrorController." + action + ": end");

            if (this.Request.IsXmlHttpRequest() || this.Request.PrefersJson())
            {
                return new ResultServiceBase(this.HttpContext)
                    .JsonErrorWithException(code, action, model.Message ?? "Unknown error.", model.Exception);
            }

            this.Response.ContentType = "text/html; charset=utf-8";
            return this.View("Error", model);
        }

        /// <summary>
        /// Called just before the response is written. Override to add logging or metrics.
        /// </summary>
        protected virtual void OnErrorResponseReady(string action, HttpErrorModel model, int code)
        {
        }
    }
}
