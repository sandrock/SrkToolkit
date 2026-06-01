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

namespace SrkToolkit.Web.Open
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A tag will be rendred as a meta HTML element.
    /// &lt;meta property="og:key" content="value" /&gt;
    /// </summary>
    public class OpenGraphTag
    {
        private OpenGraphName key;
        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphTag"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public OpenGraphTag(OpenGraphName key, string value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            this.key = key;
            this.value = value;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return this.key.Name; }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public OpenGraphName Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "<meta property=\"{0}\" content=\"{1}\" />",
                this.key.ToString().ProperHtmlAttributeEscape(),
                this.value.ProperHtmlAttributeEscape());
        }
    }
}
