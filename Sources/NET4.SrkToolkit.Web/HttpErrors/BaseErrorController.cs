
namespace SrkToolkit.Web.HttpErrors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Diagnostics;
    using System.Web;
    using SrkToolkit.Web.Models;
    using SrkToolkit.Web.Services;

    /// <summary>
    /// Base controller implementing <see cref="IErrorController"/>. Everything is overridable.
    /// </summary>
    public class BaseErrorController : Controller, IErrorController
    {
        /// <summary>
        /// Gets the built-in "forbidden" model.
        /// </summary>
        protected static HttpErrorModel DefaultForbiddenModel =
            HttpErrorModel.Create(403, "Forbidden", "You are not allowed to access the requested page.");

        /// <summary>
        /// Gets the built-in "not found" model.
        /// </summary>
        protected static HttpErrorModel DefaultNotFoundModel =
            HttpErrorModel.Create(404, "Page not found", "The requested page does not exist.");

        /// <summary>
        /// Gets the built-in "gone" model.
        /// </summary>
        protected static HttpErrorModel DefaultGoneModel =
            HttpErrorModel.Create(410, "Gone", "The resource requested is no longer available and will not be available again.");

        /// <summary>
        /// Gets the built-in "bad request" model.
        /// </summary>
        protected static HttpErrorModel DefaultBadRequestModel =
            HttpErrorModel.Create(400, "Bad request", "The request is not valid and cannot be handled.");

        /// <summary>
        /// Gets the built-in "internal error" model.
        /// </summary>
        protected static HttpErrorModel DefaultInternalModel =
            HttpErrorModel.Create(500, "Internal Server Error", "Sorry, an error occurred while processing your request.");

        /// <summary>
        /// Gets the "forbidden" model.
        /// </summary>
        public virtual HttpErrorModel ForbiddenModel
        {
            get { return DefaultForbiddenModel; }
        }

        /// <summary>
        /// Gets the "not found" model.
        /// </summary>
        public virtual HttpErrorModel NotFoundModel
        {
            get { return DefaultNotFoundModel; }
        }

        /// <summary>
        /// Gets the "gone" model.
        /// </summary>
        public virtual HttpErrorModel GoneModel
        {
            get { return DefaultGoneModel; }
        }

        /// <summary>
        /// Gets the "bad request" model.
        /// </summary>
        public virtual HttpErrorModel BadRequestModel
        {
            get { return DefaultBadRequestModel; }
        }

        /// <summary>
        /// Gets the "internal error" model.
        /// </summary>
        public virtual HttpErrorModel InternalModel
        {
            get { return DefaultInternalModel; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include exception details.
        /// </summary>
        /// <value>
        /// <c>true</c> if include exception details; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeExceptionDetails { get; set; }

        /// <summary>
        /// Returns a 401 Forbidden page.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Forbidden()
        {
            return this.Work("Forbidden", this.ForbiddenModel, 403);
        }

        /// <summary>
        /// Returns a 404 Not found page.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult NotFound()
        {
            var code = this.RouteData.Values.ContainsKey(ResultServiceBase.RouteDataHttpCodeKey)
                     ? (int)this.RouteData.Values[ResultServiceBase.RouteDataHttpCodeKey]
                     : 404;
            var model = this.NotFoundModel;
            return this.Work("NotFound", model, code);
        }

        /// <summary>
        /// Returns a 410 Gone page.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Gone()
        {
            var code = this.RouteData.Values.ContainsKey(ResultServiceBase.RouteDataHttpCodeKey)
                     ? (int)this.RouteData.Values[ResultServiceBase.RouteDataHttpCodeKey]
                     : 410;
            var model = this.GoneModel;
            return this.Work("Gone", model, code);
        }

        /// <summary>
        /// Returns a 400 Bad request page.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult BadRequest()
        {
            return this.Work("BadRequest", this.BadRequestModel, 400);
        }

        /// <summary>
        /// Returns a 500 Internal error page.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Internal()
        {
            return this.Work("Internal", this.InternalModel, 500);
        }

        /// <summary>
        /// Returns an error page using a view names "Error", passing a nice <see cref="HttpErrorModel"/>.
        /// </summary>
        /// <param name="action">The action we com from (NotFound, Forbidden...).</param>
        /// <param name="model">The model.</param>
        /// <param name="code">The HTTP code.</param>
        /// <returns></returns>
        protected virtual ActionResult Work(string action, HttpErrorModel model, int code)
        {
            Trace.TraceInformation("ErrorController." + action + ": begin");

            var ex = RouteData.Values[ResultServiceBase.RouteDataExceptionKey] as Exception;
            var httpex = RouteData.Values[ResultServiceBase.RouteDataExceptionKey] as HttpException;

            var msg = model;
            msg.Code = httpex != null ? httpex.GetHttpCode() : code;
            msg.UrlPath = Request.Url.PathAndQuery;
            msg.ErrorAction = action;

            var message = RouteData.Values[ResultServiceBase.RouteDataMessageKey] as string;
            if (message != null)
            {
                msg.Message = message;
            }

            if (this.IncludeExceptionDetails)
            {
                msg.Exception = RouteData.Values.ContainsKey(ResultServiceBase.RouteDataExceptionKey) ? RouteData.Values[ResultServiceBase.RouteDataExceptionKey] as Exception : null;
            }

            this.Response.StatusCode = msg.Code;

            Trace.TraceInformation("ErrorController." + action + ": end");

            if (this.Request.IsXmlHttpRequest())
            {
                return new ResultServiceBase(this.HttpContext).JsonError(action, model.Message ?? "Unknown error.");
            }
            else
            {
                return this.View("Error", msg);
            }
        }
    }
}
