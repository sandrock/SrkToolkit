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
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for the <see cref="TagBuilder"/> class.
    /// </summary>
    public static class SrkTagBuilderExtensions
    {
/*
        /// <summary>
        /// To the MVC HTML string.
        /// </summary>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <returns></returns>
        public static HtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            return new HtmlString(tagBuilder.ToString(renderMode));
        }
*/
        public static HtmlString ToHtmlString(this TagBuilder tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            return new HtmlString(tag.ToString());
        }
    }
}
