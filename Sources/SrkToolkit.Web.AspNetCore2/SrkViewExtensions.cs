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
    using Microsoft.AspNetCore.Mvc.Razor;
    using SrkToolkit.Web.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for <see cref="IRazorPage"/>.
    /// </summary>
    public static class SrkViewExtensions
    {
        /// <summary>
        /// Returns all temp messages, clearing them from session.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static IList<TempMessage> TempMessages(this IRazorPage view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            var data = view.ViewContext.TempData;
            var list = data[TempMessage.TempDataKey] as IList<TempMessage>;
            return list ?? new List<TempMessage>();
        }
    }
}
