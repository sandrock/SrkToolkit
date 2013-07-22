
namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SrkToolkit.Web.Models;

    /// <summary>
    /// Extension methods for <see cref="WebViewPage"/>.
    /// </summary>
    public static class SrkViewExtensions
    {
        /// <summary>
        /// Returns all temp messages, clearing them from session.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static IList<TempMessage> TempMessages(this WebViewPage view)
        {
            if (view.TempData == null)
                return new List<TempMessage>();

            var list = view.TempData["TempMessages"] as IList<TempMessage>;
            return list ?? new List<TempMessage>();
        }
    }
}
