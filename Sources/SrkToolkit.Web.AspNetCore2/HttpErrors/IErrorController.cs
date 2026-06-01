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
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// A controller able to render error pages for HTTP codes 400, 403, 404, 405, 410, 500.
    /// </summary>
    public interface IErrorController
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include exception details in error pages.
        /// </summary>
        bool IncludeExceptionDetails { get; set; }

        /// <summary>Returns a 403 Forbidden page.</summary>
        ActionResult Forbidden();

        /// <summary>Returns a 404 Not Found page.</summary>
        ActionResult NotFound();

        /// <summary>Returns a 410 Gone page.</summary>
        ActionResult Gone();

        /// <summary>Returns a 400 Bad Request page.</summary>
        ActionResult BadRequest();

        /// <summary>Returns a 405 Method Not Allowed page.</summary>
        ActionResult MethodNotAllowed();

        /// <summary>Returns a 500 Internal Server Error page.</summary>
        ActionResult Internal();
    }
}
