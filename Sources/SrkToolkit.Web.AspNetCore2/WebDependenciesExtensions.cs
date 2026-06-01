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

namespace SrkToolkit.Web
{
#if ASPMVCCORE
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
#endif
    
#if ASPMVC
    using System.Web;
    using System.Web.Mvc;
#endif

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for the <see cref="WebDependencies"/> class.
    /// </summary>
    public static class WebDependenciesExtensions
    {
        /// <summary>
        /// Gets the web dependencies manager.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">view</exception>
        public static WebDependencies WebDependencies(
#if ASPMVC
            this WebViewPage view
#elif ASPMVCCORE
            this IRazorPage view
#endif
            )
        {
            if (view == null)
                throw new ArgumentNullException("view");

            var obj = (WebDependencies)view.ViewContext.ViewData["WebDependencies"];
            if (obj == null && view.ViewContext.HttpContext != null)
            {
                obj = (WebDependencies)view.ViewContext.HttpContext.Items["WebDependencies"];
            }

            if (obj == null)
            {
#if ASPMVC
                obj = new WebDependencies();
#elif ASPMVCCORE
                obj = new WebDependencies(view.ViewContext.HttpContext.Request.PathBase);
#endif
                view.ViewContext.ViewData["WebDependencies"] = obj;
                if (view.ViewContext.HttpContext != null)
                    view.ViewContext.HttpContext.Items["WebDependencies"] = obj;
            }

            return obj;
        }

        /// <summary>
        /// Gets the web dependencies manager.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">controller</exception>
        public static WebDependencies WebDependencies(this Controller controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            var obj = (WebDependencies)controller.ViewData["WebDependencies"];
            if (obj == null && controller.HttpContext != null)
            {
                obj = (WebDependencies)controller.HttpContext.Items["WebDependencies"];
            }

            if (obj == null)
            {
#if ASPMVC
                obj = new WebDependencies();
#elif ASPMVCCORE
                obj = new WebDependencies(controller.Request.PathBase);
#endif
                controller.ViewData["WebDependencies"] = obj;
                if (controller.HttpContext != null)
                    controller.HttpContext.Items["WebDependencies"] = obj;
            }

            return obj;
        }
    }
}
