
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

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
        public static WebDependencies WebDependencies(this WebViewPage view)
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
                obj = new WebDependencies();
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
                obj = new WebDependencies();
                controller.ViewData["WebDependencies"] = obj;
                if (controller.HttpContext != null)
                    controller.HttpContext.Items["WebDependencies"] = obj;
            }

            return obj;
        }
    }
}
