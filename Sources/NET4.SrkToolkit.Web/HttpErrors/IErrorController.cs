
namespace SrkToolkit.Web.HttpErrors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// A controller able to render error pages for HTTP codes 404, 401, 400, 500.
    /// </summary>
    public interface IErrorController : IController
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include exception details.
        /// </summary>
        /// <value>
        /// <c>true</c> if include exception details; otherwise, <c>false</c>.
        /// </value>
        bool IncludeExceptionDetails { get; set; }

        /// <summary>
        /// Returns a 401 Forbidden page.
        /// </summary>
        /// <returns></returns>
        ActionResult Forbidden();

        /// <summary>
        /// Returns a 404 Not found page.
        /// </summary>
        /// <returns></returns>
        ActionResult NotFound();

        /// <summary>
        /// Returns a 410 Gone page.
        /// </summary>
        /// <returns></returns>
        ActionResult Gone();

        /// <summary>
        /// Returns a 400 Bad request page.
        /// </summary>
        /// <returns></returns>
        ActionResult BadRequest();

        /// <summary>
        /// Shows a 405 page.
        /// </summary>
        /// <returns></returns>
        ActionResult MethodNotAllowed();

        /// <summary>
        /// Returns a 500 Internal error page.
        /// </summary>
        /// <returns></returns>
        ActionResult Internal();
    }
}
