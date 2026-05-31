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

namespace SrkToolkit.Web.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;

    // Ported from MVC5 ResultService<TErrorController> which rendered error views by instantiating
    // the error controller and calling IController.Execute — a pattern that does not exist in Core.
    //
    // In Core, error responses are shaped by the status-code pages middleware. Configure it in
    // startup with app.UseStatusCodePagesWithReExecute("/Error/{0}") or equivalent, pointing to
    // your error controller actions, so that the returned status codes trigger the right views.
    //
    // The TErrorController generic parameter is gone: the consumer's error controller is invoked
    // by the middleware, not by this service.

    /// <summary>
    /// Helps return generic HTTP responses. Inherit <see cref="ResultServiceBase"/> for JSON helpers.
    /// </summary>
    public class ResultService : ResultServiceBase, IResultService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultService"/> class.
        /// </summary>
        public ResultService(HttpContext httpContext)
            : base(httpContext)
        {
        }

        /// <summary>
        /// Returns a 403 Forbidden result. Configure status-code pages middleware to display a view.
        /// </summary>
        public ActionResult Forbidden(string message = null)
        {
            return new ForbidResult();
        }

        /// <summary>
        /// Returns a 404 Not Found result. Configure status-code pages middleware to display a view.
        /// </summary>
        public ActionResult NotFound(string message = null)
        {
            return new NotFoundResult();
        }

        /// <summary>
        /// Returns a 400 Bad Request result. Configure status-code pages middleware to display a view.
        /// </summary>
        public ActionResult BadRequest(string message = null)
        {
            return new BadRequestResult();
        }

        /// <summary>
        /// Returns a 410 Gone result.
        /// </summary>
        public ActionResult Gone(string message = null)
        {
            return new StatusCodeResult(410);
        }

        /// <summary>
        /// Returns a 405 Method Not Allowed result.
        /// </summary>
        public ActionResult MethodNotAllowed()
        {
            return new StatusCodeResult(405);
        }

        /// <summary>
        /// Returns a 500 Internal Server Error result.
        /// </summary>
        public ActionResult Error(string message = null)
        {
            return new StatusCodeResult(500);
        }
    }
}
