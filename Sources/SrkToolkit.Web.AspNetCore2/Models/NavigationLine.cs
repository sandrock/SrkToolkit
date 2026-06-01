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

namespace SrkToolkit.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The navigation line contains links from the root of the website up to the current page.
    /// It helps users navigate in the site and understand its structure.
    /// </summary>
    public class NavigationLine : List<NavigationLineEntry>
    {
        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public NavigationLine Add(string name, string url)
        {
            base.Add(new NavigationLineEntry
            {
                Url = url,
                Name = name,
            });
            return this;
        }
    }
}
