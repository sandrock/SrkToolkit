
namespace SrkToolkit.Web.HttpErrors
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Text;
    using System.Diagnostics;
    using System.Web.Routing;

    public static class ErrorControllerHandler
    {
        public static void Register<TErrorController>(HttpApplication app, bool includeExceptionDetails)
            where TErrorController : IErrorController, new()
        {
            app.Error += (s, e) =>
            {
                Handle(HttpContext.Current, new TErrorController(), includeExceptionDetails);
            };
        }

        public static Exception Handle(HttpContext context, IErrorController errorController, bool includeExceptionDetails)
        {
            Trace.TraceInformation("Application_Error: begin");

            Exception exception = context.Server.GetLastError();

            context.Response.Clear();

            HttpException httpException = exception as HttpException;

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            if (httpException == null)
            {
                routeData.Values.Add("action", "Internal");
            }
            else
            { // It's an Http Exception, Let's handle it.
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "NotFound");
                        break;
                    case 401:
                    case 403:
                        routeData.Values.Add("action", "Forbidden");
                        break;
                    case 500:
                        routeData.Values.Add("action", "Internal");
                        break;
                    default:
                        routeData.Values.Add("action", "Internal");
                        break;
                }
            }

            // Pass exception details to the target error View.
            routeData.Values.Add("error", exception);

            // Clear the error on server.
            context.Server.ClearError();

            // Avoid IIS7 getting in the middle
            context.Response.TrySkipIisCustomErrors = true;

            Trace.TraceInformation("Application_Error: invoking mvc");

            try
            {
                // Call target Controller and pass the routeData.
                errorController.IncludeExceptionDetails = includeExceptionDetails;
                errorController.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());

                if (includeExceptionDetails)
                    BasicHttpErrorResponse.Execute(context, httpException, ex);
                else
                    BasicHttpErrorResponse.Execute(context, null);
            }

            Trace.TraceInformation("Application_Error: end");

            return exception;
        }
    }
}
