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
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
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
        public static WebDependencies WebDependencies(this IRazorPage view)
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
                obj = new WebDependencies(view.ViewContext.HttpContext.Request.PathBase);
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
                obj = new WebDependencies(controller.Request.PathBase);
                controller.ViewData["WebDependencies"] = obj;
                if (controller.HttpContext != null)
                    controller.HttpContext.Items["WebDependencies"] = obj;
            }

            return obj;
        }
    }
}
