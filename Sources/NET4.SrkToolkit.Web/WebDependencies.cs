
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Manages web dependencies such as scripts and styles.
    /// </summary>
    public class WebDependencies
    {
        private List<Tuple<WebDependency, WebDependencyPosition>> includes;

        public WebDependencies()
        {
        }

        /// <summary>
        /// Renders the specified dependency.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>HTML</returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public MvcHtmlString Render(WebDependency value)
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

                return MvcHtmlString.Create(sb.ToString());
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Mark the specified dependency for inclusion.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public WebDependencies Include(WebDependency value, WebDependencyPosition position = WebDependencyPosition.EndOfPage)
        {
            if (this.includes == null)
            {
                this.includes = new List<Tuple<WebDependency, WebDependencyPosition>>();
            }

            if (!this.includes.Any(i => i.Item1.Name == value.Name))
            {
                this.includes.Add(new Tuple<WebDependency, WebDependencyPosition>(value, position));
            }

            return this;
        }

        /// <summary>
        /// Renders the dependencies marked for inclusion.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public MvcHtmlString RenderIncludes(WebDependencyPosition position)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!-- WebDependencies/" + position + " - start -->");

            if (this.includes != null && this.includes.Count > 0)
            {
                for (int i = 0; i < this.includes.Count; i++)
                {
                    if (this.includes[i].Item2 == position)
                    {
                        if (this.includes[i].Item1.Files != null)
                        {
                            for (int j = 0; j < this.includes[i].Item1.Files.Count; j++)
                            {
                                RenderDependency(this.includes[i].Item1.Files[j], sb);
                            }
                        }
                    }
                }
            }

            sb.AppendLine("<!-- WebDependencies/" + position + " - end -->");
            return MvcHtmlString.Create(sb.ToString());
        }

        private static void RenderDependency(WebDependencyFile value, StringBuilder sb)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (sb == null)
                throw new ArgumentNullException("sb");

            switch (value.Type)
            {
                case WebDependencyFileType.Javascript:
                    var tag = new TagBuilder("script");
                    tag.Attributes.Add("type", "text/javascript");
                    tag.Attributes.Add("src", value.Path);
                    if (value.Encoding != null)
                        tag.Attributes.Add("charset", value.Encoding.WebName);
                    sb.AppendLine(tag.ToString(TagRenderMode.Normal));
                    break;

                case WebDependencyFileType.Css:
                    tag = new TagBuilder("link");
                    tag.Attributes.Add("rel", "stylesheet");
                    tag.Attributes.Add("type", "text/css");
                    tag.Attributes.Add("href", value.Path);
                    if (value.Encoding != null)
                        tag.Attributes.Add("charset", value.Encoding.WebName);
                    
                    if (value.Media != null)
                    {
                        var medias = Enum.GetValues(typeof(WebDependencyMedia))
                            .Cast<WebDependencyMedia>()
                            .Where(v => (value.Media & v) == v)
                            .Select(v => v.ToString().ToLowerInvariant())
                            .ToArray();
                        tag.Attributes.Add("media", string.Join(", ", medias));
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
