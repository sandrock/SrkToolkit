﻿// 
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
    /// Manages web dependencies such as scripts and styles.
    /// </summary>
    public class WebDependencies
    {
        protected Dictionary<string, Tuple<WebDependency, WebDependencyPosition>> includes;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDependencies"/> class.
        /// </summary>
        public WebDependencies()
        {
        }

        /// <summary>
        /// Renders the specified dependency.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>HTML</returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public HtmlString Render(WebDependency value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Files != null)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < value.Files.Count; i++)
                {
                    RenderDependency(value.Files[i], sb);
                }

                return new HtmlString(sb.ToString());
            }

            return HtmlString.Empty;
        }

        /// <summary>
        /// Mark the specified dependency for inclusion.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
#pragma warning disable CS0618 // Type or member is obsolete
        public WebDependencies Include(WebDependency value, WebDependencyPosition position = WebDependencyPosition.Default)
        {
            if (this.includes == null)
            {
                this.includes = new Dictionary<string, Tuple<WebDependency, WebDependencyPosition>>();
            }
            
            Tuple<WebDependency, WebDependencyPosition> includeCandidate;
            if (!this.includes.TryGetValue(value.Name, out includeCandidate))
            {
                // not included yet
                this.includes.Add(value.Name, new Tuple<WebDependency, WebDependencyPosition>(value, position));
            }
            else
            {
                // already included. Take the highest position
                var replace = false;
                if (position != WebDependencyPosition.Default)
                {
                    if (includeCandidate.Item2 == WebDependencyPosition.Default)
                    {
                        replace = true;
                    }
                    else if (position < includeCandidate.Item2)
                    {
                        replace = true;
                    }
                    else
                    {
                        // do not replace
                    }
                }
                else
                {
                    // position is default, do not replace
                }

                if (replace)
                {
                    this.includes[value.Name] = new Tuple<WebDependency, WebDependencyPosition>(value, position);
                }
            }

            return this;
        }
#pragma warning restore CS0618 // Type or member is obsolete

        /// <summary>
        /// Renders the dependencies marked for inclusion.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public HtmlString RenderIncludes(WebDependencyPosition position)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!-- WebDependencies/" + position + " - start -->");

            if (this.includes != null && this.includes.Count > 0)
            {
                var sorted = this.includes.Values.OrderBy(i => i.Item1.Order).ToArray();
                for (int i = 0; i < sorted.Length; i++)
                {
                    var item = sorted[i].Item1;
                    var itemPosition = sorted[i].Item2;
#pragma warning disable CS0618 // Type or member is obsolete
                    if (itemPosition == position || itemPosition == WebDependencyPosition.Default && item.DefaultPosition == position)
                    {
                        if (item.Files != null)
                        {
                            for (int j = 0; j < item.Files.Count; j++)
                            {
                                RenderDependency(item.Files[j], sb);
                            }
                        }
                    }
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }

            sb.AppendLine("<!-- WebDependencies/" + position + " - end -->");
            return new HtmlString(sb.ToString());
        }

        protected static void RenderDependency(WebDependencyFile value, StringBuilder sb)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (sb == null)
                throw new ArgumentNullException("sb");

            var webPath = value.Path;
            if (webPath != null && webPath.Length >= 2)
            {
                if (webPath[0].Equals('~') && webPath[1].Equals('/'))
                {
                    var virtualPath = HostingEnvironment.ApplicationVirtualPath ?? "/"; // Root:"/", Subfolder:"/a/b"
                    webPath = virtualPath + webPath.Substring(1); // Root:"//Content/Site.css", Subfolder:"/a/b/Content/Site.css"
                    webPath = "/" + webPath.TrimStart('/'); // Root:"/Content/Site.css", Subfolder:"/a/b/Content/Site.css"
                }
            }

            switch (value.Type)
            {
                case WebDependencyFileType.Javascript:
                    var tag = new TagBuilder("script");
                    tag.Attributes.Add("type", "text/javascript");
                    tag.Attributes.Add("src", webPath);
                    if (value.Encoding != null)
                    {
                        tag.Attributes.Add("charset", value.Encoding.WebName);
                    }

                    if (value.Attributes != null)
                    {
                        tag.MergeAttributes(value.Attributes); 
                    }

                    sb.AppendLine(tag.ToString(TagRenderMode.Normal));
                    break;

                case WebDependencyFileType.Css:
                    tag = new TagBuilder("link");
                    tag.Attributes.Add("rel", "stylesheet");
                    tag.Attributes.Add("type", "text/css");
                    tag.Attributes.Add("href", webPath);
                    if (value.Encoding != null)
                    {
                        tag.Attributes.Add("charset", value.Encoding.WebName);
                    }

                    if (value.Media != null)
                    {
                        var medias = Enum.GetValues(typeof(WebDependencyMedia))
                            .Cast<WebDependencyMedia>()
                            .Where(v => (value.Media & v) == v)
                            .Select(v => v.ToString().ToLowerInvariant())
                            .ToArray();
                        tag.Attributes.Add("media", string.Join(", ", medias));
                    }

                    if (value.Attributes != null)
                    {
                        tag.MergeAttributes(value.Attributes); 
                    }

                    sb.AppendLine(tag.ToString(TagRenderMode.SelfClosing));
                    break;

                default:
                    sb.AppendLine("<!-- ERROR: unknown web dependency file type '" + value.Type + "' -->");
                    break;
            }
        }
    }
}
