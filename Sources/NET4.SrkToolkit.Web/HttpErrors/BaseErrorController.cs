
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

    public class BaseErrorController : Controller, IErrorController
    {
        protected static HttpErrorModel forbiddenModel =
            new HttpErrorModel("Forbidden", "You are not allowed to access the requested page.");
        protected static HttpErrorModel notFoundModel =
            new HttpErrorModel("Page not found", "The requested page does not exist.");
        protected static HttpErrorModel badRequestModel =
            new HttpErrorModel("Bad request", "The request is not valid and cannot be handled.");
        protected static HttpErrorModel internalModel =
            new HttpErrorModel("Internal Server Error", "Sorry, an error occurred while processing your request.");
        
        public virtual HttpErrorModel ForbiddenModel
        {
            get { return forbiddenModel; }
        }

        public virtual HttpErrorModel NotFoundModel
        {
            get { return notFoundModel; }
        }

        public virtual HttpErrorModel BadRequestModel
        {
            get { return badRequestModel; }
        }

        public virtual HttpErrorModel InternalModel
        {
            get { return internalModel; }
        }

        public bool IncludeExceptionDetails { get; set; }

        public ActionResult Forbidden()
        {
            return this.Work("Forbidden", this.ForbiddenModel, 403);
        }

        public ActionResult NotFound()
        {
            return this.Work("NotFound", this.NotFoundModel, 404);
        }

        public ActionResult BadRequest()
        {
            return this.Work("BadRequest", this.BadRequestModel, 400);
        }

        public ActionResult Internal()
        {
            return this.Work("BadRequest", this.InternalModel, 500);
        }

        protected virtual ActionResult Work(string action, HttpErrorModel model, int code)
        {
            Trace.TraceInformation("ErrorController." + action + ": begin");
            var ex = RouteData.Values["error"] as Exception;
            var httpex = RouteData.Values["error"] as HttpException;

            var msg = model;
            msg.Code = httpex != null ? httpex.GetHttpCode() : code;
            msg.UrlPath = Request.Url.PathAndQuery;
            msg.ErrorAction = action;

            if (this.IncludeExceptionDetails)
            {
                msg.Exception = RouteData.Values.ContainsKey("error") ? RouteData.Values["error"] as Exception : null;
            }

            Response.StatusCode = msg.Code;
            Trace.TraceInformation("ErrorController." + action + ": end");

            return this.View("Error", msg);
        }
    }
}
