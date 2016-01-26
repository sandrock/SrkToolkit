
namespace SrkToolkit.Web.Services
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using SrkToolkit.Web.HttpErrors;

    /// <summary>
    /// Helps return generic HTTP responses.
    /// </summary>
    /// <typeparam name="TErrorController">The type of the error controller.</typeparam>
    public class ResultService<TErrorController> : ResultServiceBase, IResultService
        where TErrorController : IErrorController, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultService{TErrorController}"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public ResultService(HttpContextBase httpContext)
            : base(httpContext)
        {
        }

        /// <summary>
        /// Gets the route to the "forbidden" page.
        /// </summary>
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

        /// <summary>
        /// Gets the route to the "not found" page.
        /// </summary>
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

        /// <summary>
        /// Gets the route to the "method not allowed" page.
        /// </summary>
        public static RouteData MethodNotAllowedRoute
        {
            get
            {
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "MethodNotAllowed");
                return routeData;
            }
        }

        /// <summary>
        /// Gets the route to the "gone" page.
        /// </summary>
        public static RouteData GoneRoute
        {
            get
            {
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Gone");
                return routeData;
            }
        }

        /// <summary>
        /// Gets the route to the "bad request" page.
        /// </summary>
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

        /// <summary>
        /// Gets the route to the "internal error" page.
        /// </summary>
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
            this.HttpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.HttpContext;
            ctrlContext.RouteData = ForbiddenRoute;
            if (message != null)
            {
                ctrlContext.RouteData.Values[ResultServiceBase.RouteDataMessageKey] = message;
                ctrlContext.RouteData.DataTokens[ResultServiceBase.RouteDataMessageKey] = message;
            }

            IController ctrl = new TErrorController();

            ctrl.Execute(new RequestContext(this.HttpContext, ctrlContext.RouteData));
            return null;
        }

        /// <summary>
        /// Returns a 500 error view with a custom message.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for standard message.</param>
        public ActionResult Error(string message)
        {
            this.HttpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.HttpContext;
            ctrlContext.RouteData = InternalRoute;
            ctrlContext.RouteData.Values["error"] = new Exception(message);
            if (message != null)
            {
                ctrlContext.RouteData.Values[ResultServiceBase.RouteDataMessageKey] = message;
                ctrlContext.RouteData.DataTokens[ResultServiceBase.RouteDataMessageKey] = message;
            }

            IController ctrl = new BaseErrorController
            {
                ControllerContext = ctrlContext,
            };

            ctrl.Execute(new RequestContext(this.HttpContext, ctrlContext.RouteData));
            return null;
        }

        /// <summary>
        /// Shows a 404 page.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for random message.</param>
        /// <returns></returns>
        public ActionResult NotFound(string message = null)
        {
            this.HttpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.HttpContext;
            ctrlContext.RouteData = NotFoundRoute;
            if (message != null)
            {
                ctrlContext.RouteData.Values[ResultServiceBase.RouteDataMessageKey] = message;
                ctrlContext.RouteData.DataTokens[ResultServiceBase.RouteDataMessageKey] = message;
            }

            IController ctrl = new BaseErrorController
            {
                ControllerContext = ctrlContext,
            };

            ctrl.Execute(new RequestContext(this.HttpContext, ctrlContext.RouteData));

            return null;
        }

        public ActionResult MethodNotAllowed()
        {
            this.HttpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.HttpContext;
            ctrlContext.RouteData = MethodNotAllowedRoute;

            IController ctrl = new BaseErrorController
            {
                ControllerContext = ctrlContext,
            };

            ctrl.Execute(new RequestContext(this.HttpContext, ctrlContext.RouteData));

            return null;
        }

        /// <summary>
        /// Shows a 410 page.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for random message.</param>
        /// <returns></returns>
        public ActionResult Gone(string message = null)
        {
            this.HttpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.HttpContext;
            ctrlContext.RouteData = GoneRoute;
            if (message != null)
            {
                ctrlContext.RouteData.Values[ResultServiceBase.RouteDataMessageKey] = message;
                ctrlContext.RouteData.DataTokens[ResultServiceBase.RouteDataMessageKey] = message;
            }

            IController ctrl = new BaseErrorController
            {
                ControllerContext = ctrlContext,
            };

            ctrl.Execute(new RequestContext(this.HttpContext, ctrlContext.RouteData));

            return null;
        }

        /// <summary>
        /// Shows a 400 page.
        /// </summary>
        public ActionResult BadRequest(string message = null)
        {
            this.HttpContext.Response.TrySkipIisCustomErrors = true; // motherfucking helpfull

            var ctrlContext = new ControllerContext();
            ctrlContext.HttpContext = this.HttpContext;
            ctrlContext.RouteData = BadRequestRoute;
            if (message != null)
            {
                ctrlContext.RouteData.Values[ResultServiceBase.RouteDataMessageKey] = message;
                ctrlContext.RouteData.DataTokens[ResultServiceBase.RouteDataMessageKey] = message;
            }

            IController ctrl = new BaseErrorController
            {
                ControllerContext = ctrlContext,
            };

            ctrl.Execute(new RequestContext(this.HttpContext, ctrlContext.RouteData));

            return null;
        }
    }
}
