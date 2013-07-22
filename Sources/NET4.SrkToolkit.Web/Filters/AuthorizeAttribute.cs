
namespace SrkToolkit.Web.Filters
{
    using System.Web.Mvc;
    using SrkToolkit.Web.Services;

    /// <summary>
    /// The proper attribute to allow access only to authenticated users.
    /// </summary>
    public abstract class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected abstract IResultService GetResultService();

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // if authenticated, display a forbidden page
                filterContext.Result = new HttpStatusCodeResult(403, "Forbidden");
                var resultService = this.GetResultService();
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