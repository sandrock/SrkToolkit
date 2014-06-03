
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    public static class WebDependenciesExtensions
    {
        public static WebDependencies WebDependencies(this WebViewPage view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            var obj = (WebDependencies)view.ViewContext.ViewData["WebDependencies"];
            if (obj == null)
            {
                obj = new WebDependencies();
                view.ViewContext.ViewData["WebDependencies"] = obj;
            }

            return obj;
        }

        public static WebDependencies WebDependencies(this Controller view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            var obj = (WebDependencies)view.ViewData["WebDependencies"];
            if (obj == null)
            {
                obj = new WebDependencies();
                view.ViewData["WebDependencies"] = obj;
            }

            return obj;
        }
    }
}
