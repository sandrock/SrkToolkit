
namespace SrkToolkit.Web.Filters
{
    using System.Web.Mvc;
    using SrkToolkit.Web.Services;
    using System.Web;

    /// <summary>
    /// The proper attribute to allow access only to authenticated users.
    /// </summary>
    public abstract class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        /// <summary>
        /// Gets the result service (you should inherit from <see cref="ResultService{TErrorController}"/> or <see cref="IResultService"/>).
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        protected abstract IResultService GetResultService(HttpContextBase httpContext);

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // if authenticated, display a forbidden page
                filterContext.Result = new HttpStatusCodeResult(403, "Forbidden");
                var resultService = this.GetResultService(filterContext.HttpContext);
                resultService.Forbidden();
            }
            else
            {
                // if not authenticated, invite to login using the default behavior
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}