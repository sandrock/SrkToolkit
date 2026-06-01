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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using SrkToolkit.Web.Services;
    using System;
    using System.Linq;

    // Ported from MVC5 AuthorizeAttribute (which extended System.Web.Mvc.AuthorizeAttribute).
    //
    // Migration note for consumers:
    //   MVC5: override AuthorizeCore(HttpContextBase)  →  Core: override IsAuthorized(AuthorizationFilterContext)
    //   MVC5: override GetResultService(HttpContextBase) returning IResultService
    //         →  Core: override GetResultService(HttpContext) returning IResultService
    //                  (default returns null, which falls back to ForbidResult)

    /// <summary>
    /// Base authorization filter. Inherit, implement <see cref="IsAuthorized"/>, apply to
    /// controllers or actions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private bool authenticateWhenAnonymous = true;
        private bool authenticateWhenForbidden = false;

        /// <summary>
        /// Issue a 401 challenge (redirect to login) instead of 403 when the user is not
        /// authenticated. Default is true.
        /// </summary>
        public bool AuthenticateWhenAnonymous
        {
            get { return this.authenticateWhenAnonymous; }
            set { this.authenticateWhenAnonymous = value; }
        }

        /// <summary>
        /// Issue a 401 challenge instead of 403 when the user is authenticated but not
        /// authorized. Default is false.
        /// </summary>
        public bool AuthenticateWhenForbidden
        {
            get { return this.authenticateWhenForbidden; }
            set { this.authenticateWhenForbidden = value; }
        }

        /// <summary>
        /// Returns true if the current request is authorized. Implement your authorization logic here.
        /// </summary>
        protected abstract bool IsAuthorized(AuthorizationFilterContext context);

        /// <summary>
        /// Returns an <see cref="IResultService"/> used to produce the 403 response when the
        /// request is denied without a challenge. Override to return a <see cref="ResultService"/>
        /// (or a custom subclass). Default returns null, which falls back to <see cref="ForbidResult"/>.
        /// </summary>
        protected virtual IResultService GetResultService(HttpContext httpContext) => null;

        /// <inheritdoc />
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            // Respect [AllowAnonymous] on the endpoint or action.
            if (context.Filters.OfType<IAllowAnonymous>().Any())
                return;

            if (IsAuthorized(context))
                return;

            HandleUnauthorized(context);
        }

        /// <summary>
        /// Called when <see cref="IsAuthorized"/> returns false. Sets
        /// <see cref="AuthorizationFilterContext.Result"/> based on the authentication state and
        /// the <see cref="AuthenticateWhenAnonymous"/> / <see cref="AuthenticateWhenForbidden"/> flags.
        /// Override for full control over the unauthorized response.
        /// </summary>
        protected virtual void HandleUnauthorized(AuthorizationFilterContext context)
        {
            bool isAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;

            bool challenge = isAuthenticated ? this.authenticateWhenForbidden : this.authenticateWhenAnonymous;

            if (challenge)
            {
                context.Result = new ChallengeResult();
            }
            else
            {
                var resultService = GetResultService(context.HttpContext);
                context.Result = resultService?.Forbidden() ?? new ForbidResult();
            }
        }
    }
}
