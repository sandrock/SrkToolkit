// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

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
        private bool authenticateWhenAnonymous = true;
        private bool authenticateWhenForbiden = false;

        public AuthorizeAttribute()
        {
        }

        /// <summary>
        /// Use a 401 (redirect to authenticate) instead of a 403 when the user is not authenticated. Default is true.
        /// </summary>
        public bool AuthenticateWhenAnonymous
        {
            get { return this.authenticateWhenAnonymous; }
            set { this.authenticateWhenAnonymous = value; }
        }

        /// <summary>
        /// Use a 401 (redirect to authenticate) instead of a 403 (forbidden) when the user is authenticated. Default is false.
        /// </summary>
        public bool AuthenticateWhenForbidden
        {
            get { return this.authenticateWhenForbiden; }
            set { this.authenticateWhenForbiden = value; }
        }

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
            if (filterContext != null && filterContext.HttpContext != null && filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity != null && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (this.authenticateWhenForbiden)
                {
                    // if authenticated, invite to login using the default behavior
                    base.HandleUnauthorizedRequest(filterContext);
                }
                else
                {
                    // if authenticated, display a forbidden page
                    filterContext.Result = new HttpStatusCodeResult(403, "Forbidden");
                    var resultService = this.GetResultService(filterContext.HttpContext);
                    resultService.Forbidden();
                }
            }
            else
            {
                if (this.authenticateWhenAnonymous)
                {
                    // if not authenticated, invite to login using the default behavior
                    base.HandleUnauthorizedRequest(filterContext);
                }
                else
                {
                    // if not authenticated, display a forbidden page
                    filterContext.Result = new HttpStatusCodeResult(403, "Forbidden");
                    var resultService = this.GetResultService(filterContext.HttpContext);
                    resultService.Forbidden();
                }
            }
        }
    }
}