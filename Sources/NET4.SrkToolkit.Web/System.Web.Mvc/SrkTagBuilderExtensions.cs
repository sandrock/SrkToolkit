﻿
namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Extension methods for the <see cref="TagBuilder"/> class.
    /// </summary>
    public static class SrkTagBuilderExtensions
    {
        /// <summary>
        /// To the MVC HTML string.
        /// </summary>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <returns></returns>
        public static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            return new MvcHtmlString(tagBuilder.ToString(renderMode));
        }
    }
}