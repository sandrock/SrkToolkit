
namespace SrkToolkit.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Helps return generic HTTP responses.
    /// </summary>
    public interface IResultService
    {
        /// <summary>
        /// Returns a 403 Forbidden view.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for random message.</param>
        ActionResult Forbidden(string message = null);

        /// <summary>
        /// Shows a 404 page.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for random message.</param>
        /// <returns></returns>
        ActionResult NotFound(string message = null);

        /// <summary>
        /// Shows a 400 page.
        /// </summary>
        ActionResult BadRequest(string message = null);

        /// <summary>
        /// Returns a standard JSON result for a successful operation.
        /// </summary>
        /// <returns></returns>
        ActionResult JsonSuccess();

        /// <summary>
        /// Returns a standard JSON result containing data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ActionResult JsonSuccess(object data);

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <returns></returns>
        ActionResult JsonError();

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <returns></returns>
        ActionResult JsonError(string errorCode);

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <returns></returns>
        ActionResult JsonError(string errorCode, string errorMessage);
        
        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ActionResult JsonError(string errorCode, string errorMessage, object data);
    }
}
