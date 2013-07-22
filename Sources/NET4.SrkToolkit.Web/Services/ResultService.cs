
namespace SrkToolkit.Web.Services
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using SrkToolkit.Web.HttpErrors;

    public class ResultService<TErrorController> : IResultService
        where TErrorController : IErrorController, new()
    {
        private readonly HttpContextBase httpContext;

        public ResultService(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        public static RouteData ForbiddenRoute
        {
            get
            {
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Forbidden");
                return routeData;
            }
        }

        public static RouteData NotFoundRoute
        {
            get
            {
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "NotFound");
                return routeData;
            }
        }

        public static RouteData BadRequestRoute
        {
            get
            {
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "BadRequest");
                return routeData;
            }
        }

        public static RouteData InternalRoute
        {
            get
            {
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Internal");
                return routeData;
            }
        }

        /// <summary>
        /// Returns a 403 Forbidden view.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for random message.</param>
        public ActionResult Forbidden(string message = null)
        {
            this.httpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.httpContext;
            ctrlContext.RouteData = ForbiddenRoute;
            if (message != null)
            {
                ctrlContext.RouteData.Values["message"] = message;
                ctrlContext.RouteData.DataTokens["message"] = message;
            }

            IController ctrl = new TErrorController();

            ctrl.Execute(new RequestContext(this.httpContext, ctrlContext.RouteData));
            return null;
        }

        /// <summary>
        /// Returns a 500 error view with a custom message.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for standard message.</param>
        public ActionResult Error(string message)
        {
            this.httpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.httpContext;
            ctrlContext.RouteData = InternalRoute;
            ctrlContext.RouteData.Values["error"] = new Exception(message);
            if (message != null)
            {
                ctrlContext.RouteData.Values["message"] = message;
                ctrlContext.RouteData.DataTokens["message"] = message;
            }

            IController ctrl = new BaseErrorController
            {
                ControllerContext = ctrlContext,
            };

            ctrl.Execute(new RequestContext(this.httpContext, ctrlContext.RouteData));
            return null;
        }

        /// <summary>
        /// Shows a 404 page.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for random message.</param>
        /// <returns></returns>
        public ActionResult NotFound(string message = null)
        {
            this.httpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.httpContext;
            ctrlContext.RouteData = NotFoundRoute;
            if (message != null)
            {
                ctrlContext.RouteData.Values["message"] = message;
                ctrlContext.RouteData.DataTokens["message"] = message;
            }

            IController ctrl = new BaseErrorController
            {
                ControllerContext = ctrlContext,
            };

            ctrl.Execute(new RequestContext(this.httpContext, ctrlContext.RouteData));

            return null;
        }

        /// <summary>
        /// Shows a 400 page.
        /// </summary>
        public ActionResult BadRequest(string message = null)
        {
            this.httpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.httpContext;
            ctrlContext.RouteData = BadRequestRoute;
            if (message != null)
            {
                ctrlContext.RouteData.Values["message"] = message;
                ctrlContext.RouteData.DataTokens["message"] = message;
            }

            IController ctrl = new BaseErrorController
            {
                ControllerContext = ctrlContext,
            };

            ctrl.Execute(new RequestContext(this.httpContext, ctrlContext.RouteData));

            return null;
        }

        /// <summary>
        /// Returns a standard JSON result for a successful operation.
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonSuccess()
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = true,
                    ErrorCode = default(string),
                    ErrorMessage = default(string),
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult JsonSuccess(object data)
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = true,
                    ErrorCode = default(string),
                    ErrorMessage = default(string),
                    Data = data,
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonError()
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = false,
                    ErrorCode = default(string),
                    ErrorMessage = default(string),
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <returns></returns>
        public ActionResult JsonError(string errorCode)
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = default(string),
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <returns></returns>
        public ActionResult JsonError(string errorCode, string errorMessage)
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage,
                },
            };
        }
    }
}
