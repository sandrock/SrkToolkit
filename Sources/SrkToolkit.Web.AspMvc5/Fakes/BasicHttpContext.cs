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

namespace SrkToolkit.Web.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Implementation of <see cref="HttpContextBase"/> where the developer can set any property to any value.
    /// </summary>
    public class BasicHttpContext : HttpContextBase
    {
        private HttpSessionStateBase session;
        private IPrincipal user;
        private Dictionary<object, object> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicHttpContext"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="user">The user.</param>
        public BasicHttpContext(
            HttpSessionStateBase session = null,
            IPrincipal user = null)
        {
            this.session = session;
            this.user = user;
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.SessionState.HttpSessionState" /> object for the current HTTP request.
        /// </summary>
        /// <returns>The session-state object for the current HTTP request.</returns>
        public override HttpSessionStateBase Session
        {
            get
            {
                return this.session ?? (this.session = new BasicHttpSessionState());
            }
        }

        /// <summary>
        /// Gets or sets security information for the current HTTP request.
        /// </summary>
        /// <returns>An object that contains security information for the current HTTP request.</returns>
        public override IPrincipal User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }

        /// <summary>
        /// Gets a key/value collection that can be used to organize and share data between a module and a handler during an HTTP request.
        /// </summary>
        /// <returns>A key/value collection that provides access to an individual value in the collection by using a specified key.</returns>
        public override System.Collections.IDictionary Items
        {
            get
            {
                return this.items ?? (this.items = new Dictionary<object, object>());
            }
        }
    }
}
