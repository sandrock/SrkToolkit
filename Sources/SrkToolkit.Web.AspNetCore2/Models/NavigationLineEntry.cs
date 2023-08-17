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
    /// An navigation entry in a navigation line.
    /// </summary>
    public class NavigationLineEntry
    {
        /// <summary>
        /// Gets or sets the name of the link.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL of the link.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Extra collection to store various data.
        /// </summary>
        public Dictionary<string, object> Items { get; set; }
    }
}
