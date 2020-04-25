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
    using System.Web.Mvc;

    /// <summary>
    /// A HTML element that will represent a page information .
    /// </summary>
    public class PageInfoObject
    {
        private static readonly string[] singleTagNames = new string[] { "meta", "link", };
        private readonly Dictionary<string, string> attributes = new Dictionary<string, string>();
        private string tagName;
        private string tagValue;
        private string attributeValue;
        private OpenGraphTag openGraphTag;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfoObject"/> class.
        /// </summary>
        public PageInfoObject()
            : this(PageInfoObjectSection.Basic)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfoObject"/> class.
        /// </summary>
        /// <param name="section">The section.</param>
        public PageInfoObject(PageInfoObjectSection section)
        {
            this.Section = section;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfoObject"/> class.
        /// </summary>
        /// <param name="openGraphTag">The open graph tag.</param>
        public PageInfoObject(OpenGraphTag openGraphTag)
        {
            this.Section = PageInfoObjectSection.OpenGraph;
            this.openGraphTag = openGraphTag;
        }

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        public PageInfoObjectSection Section { get; set; }

        /// <summary>
        /// Gets the open graph tag.
        /// </summary>
        public OpenGraphTag OpenGraphTag
        {
            get { return this.openGraphTag; }
        }

#if !NSTD && !NET40
        private TagBuilder Tag
        {
            get
            {
                if (string.IsNullOrEmpty(this.tagName))
                    return null;

                var tag = new TagBuilder(this.tagName);
                foreach (var attr in this.attributes)
                {
                    tag.MergeAttribute(attr.Key, attr.Value);
                }

                if (this.tagValue != null)
                {
                    tag.SetInnerText(this.tagValue);
                }

                return tag;
            }
        } 
#endif

        /// <summary>
        /// Prepare a HTML element of the specified name to contain the item's value.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        public PageInfoObject TagWithValue(string tagName)
        {
            this.tagName = tagName;
            return this;
        }

        /// <summary>
        /// Prepare a meta HTML element of the specified name to contain the item's value.
        /// </summary>
        /// <param name="metaName">Name of the meta.</param>
        /// <returns></returns>
        public PageInfoObject MetaWithValue(string metaName)
        {
            this.MetaWithValue(metaName, null);
            return this;
        }

        /// <summary>
        /// Prepare a meta HTML element of the specified name to contain the specified value.
        /// </summary>
        /// <param name="metaName">Name of the meta.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public PageInfoObject MetaWithValue(string metaName, string value)
        {
            this.tagName = "meta";
            this.attributes.Add("name", metaName);

            if (value != null)
                this.attributes.Add("content", value);

            this.attributeValue = "content";
            return this;
        }

        /// <summary>
        /// Prepare a link HTML element of the specified name to contain the item's value.
        /// </summary>
        /// <param name="relName">Name of the relative.</param>
        /// <returns></returns>
        public PageInfoObject RelatedLink(string relName)
        {
            this.RelatedLink(relName, null, null, null);
            return this;
        }

        /// <summary>
        /// Prepare a link HTML element of the specified name to contain the item's value.
        /// </summary>
        /// <param name="relName">Name of the relative.</param>
        /// <param name="hrefLang">The href language.</param>
        /// <param name="href">The href.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public PageInfoObject RelatedLink(string relName, string hrefLang, string href, string type)
        {
            this.tagName = "link";
            this.attributes.Add("rel", relName);
            this.attributes.Add("href", href);
            this.attributeValue = "href";

            if (!string.IsNullOrEmpty(type))
                this.attributes.Add("type", type);

            if (!string.IsNullOrEmpty(hrefLang))
                this.attributes.Add("hrefLang", hrefLang);

            return this;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public PageInfoObject SetValue(string value)
        {
            if (this.openGraphTag != null)
            {
                if (value != null)
                    this.openGraphTag.Value = value;
            }
            else if (this.attributeValue != null)
            {
                if (value != null)
                    this.attributes[this.attributeValue] = value;
            }
            else
            {
                if (value != null)
                    this.tagValue = value;
            }

            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that contains the generated HTML tags.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that contains the generated HTML tags.
        /// </returns>
        public override string ToString()
        {
#if NSTD || NET40
            return "<!-- PageInfoObject: NOT IMPLEMENTED IN NETSTANDARD -->";
#else
            var tag = this.Tag;
            if (this.openGraphTag != null)
            {
                return this.openGraphTag.ToString();
            }
            else if (tag != null)
            {
                return tag.ToString(singleTagNames.Contains(this.tagName) ? TagRenderMode.SelfClosing : TagRenderMode.Normal);
            }
            else
            {
                return "<!-- PageInfoObject: no HTML to write -->";
            }
#endif
        } 
    }

    /// <summary>
    /// HTML object sections.
    /// </summary>
    [Flags]
    public enum PageInfoObjectSection
    {
        /// <summary>
        /// The basic elements (title, description, links, some metas).
        /// </summary>
        Basic      = 0x0001,

        /// <summary>
        /// The old meta tags (http-equiv, author, owner...).
        /// </summary>
        OldMeta    = 0x0002,

        /// <summary>
        /// The open graph tags. 
        /// </summary>
        OpenGraph  = 0x0004,

        /// <summary>
        /// The dublin core tags.
        /// </summary>
        DublinCore = 0x0008,
    }
}
