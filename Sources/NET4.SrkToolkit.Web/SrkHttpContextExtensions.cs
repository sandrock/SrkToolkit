
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public static class SrkHttpContextExtensions
    {
        /// <summary>
        /// Fast access to HttpContext.User.Identity.Name.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">httpContext</exception>
        public static string GetUserIdentityName(this HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            return httpContext.User != null && httpContext.User.Identity != null ? httpContext.User.Identity.Name.NullIfEmptyOrWhitespace() : null;
        }

        /// <summary>
        /// Fast access to HttpContext.User.Identity.Name.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">httpContext</exception>
        public static string GetUserIdentityName(this HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            return httpContext.User != null && httpContext.User.Identity != null ? httpContext.User.Identity.Name.NullIfEmptyOrWhitespace() : null;
        }
    }
}
