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
    /// Descriptors to a page in order to generate a meta/link tag.
    /// </summary>
    public class PageInfoItem : IEnumerable<PageInfoObject>
    {
        private readonly List<PageInfoObject> objects = new List<PageInfoObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfoItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public PageInfoItem(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfoItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public PageInfoItem(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Adds an HTML element.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public PageInfoItem Add(PageInfoObject obj)
        {
            this.objects.Add(obj);
            return this;
        }

        /// <summary>
        /// Adds an HTML element based on a OpenGraph tag.
        /// </summary>
        /// <param name="openGraphTag">The object.</param>
        /// <returns></returns>
        public PageInfoItem Add(OpenGraphTag openGraphTag)
        {
            var obj = new PageInfoObject(openGraphTag);
            this.objects.Add(obj);
            return this;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetValue(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<PageInfoObject> GetEnumerator()
        {
            return this.objects.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.objects.GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that contains the generated HTML tags.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that contains the generated HTML tags.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            this.ToString(sb, PageInfo.defaultSections, false);
            return sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that contains the generated HTML tags.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to write to.</param>
        /// <param name="sections">The sections to use.</param>
        /// <exception cref="System.ArgumentNullException">sb</exception>
        public void ToString(StringBuilder sb, PageInfoObjectSection sections, bool indented)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            foreach (var obj in this.objects)
            {
                if ((obj.Section & sections) != 0)
                {
                    obj.SetValue(this.Value);
                    if (indented)
                        sb.AppendLine(obj.ToString());
                    else
                        sb.Append(obj.ToString());
                }
            }
        }
    }
}
