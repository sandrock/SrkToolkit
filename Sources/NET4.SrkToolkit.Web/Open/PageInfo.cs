
namespace SrkToolkit.Web.Open
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Helps attach descriptors to a page in order to generate meta/link tags.
    /// </summary>
    public class PageInfo
    {
        internal static readonly PageInfoObjectSection defaultSections = PageInfoObjectSection.Basic | PageInfoObjectSection.OldMeta;
        private readonly List<PageInfoItem> items = new List<PageInfoItem>();
        private OpenGraphObject openGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageInfo"/> class.
        /// </summary>
        public PageInfo()
        {
        }

        /// <summary>
        /// Descriptor for the title of your page.
        /// </summary>
        public static PageInfoItem Title
        {
            get
            {
                return new PageInfoItem("title")
                {
                    new PageInfoObject().TagWithValue("title"),
                    new OpenGraphTag(OpenGraphName.KnownNames.OgTitle, null),
                };
            }
        }

        /// <summary>
        /// Descriptor for the description of your page.
        /// </summary>
        public static PageInfoItem Description
        {
            get
            {
                return new PageInfoItem("description")
                {
                    new PageInfoObject().MetaWithValue("description"),
                    new OpenGraphTag(OpenGraphName.KnownNames.OgDescription, null),
                };
            }
        }

        /// <summary>
        /// Descriptor for the language/locale of your page.
        /// </summary>
        public static PageInfoItem Language
        {
            get
            {
                return new PageInfoItem("language", CultureInfo.CurrentCulture.Name)
                {
                    new PageInfoObject().MetaWithValue("language", CultureInfo.CurrentCulture.Name),
                    new OpenGraphTag(OpenGraphName.KnownNames.OgLocale, CultureInfo.CurrentCulture.Name.Replace("-", "_")),
                };
            }
        }

        /// <summary>
        /// Descriptor for the keywords of your page.
        /// </summary>
        public static PageInfoItem Keywords
        {
            get
            {
                return new PageInfoItem("keywords")
                {
                    new PageInfoObject().MetaWithValue("keywords"),
                };
            }
        }

        /// <summary>
        /// Descriptor for the author of your page.
        /// </summary>
        public static PageInfoItem Author
        {
            get
            {
                return new PageInfoItem("author")
                {
                    new PageInfoObject().MetaWithValue("author"),
                };
            }
        }

        /// <summary>
        /// Descriptor for the owner of your page.
        /// </summary>
        public static PageInfoItem Owner
        {
            get
            {
                return new PageInfoItem("owner")
                {
                    new PageInfoObject().MetaWithValue("owner"),
                };
            }
        }

        /// <summary>
        /// Descriptor for the canonical url of your page.
        /// </summary>
        public static PageInfoItem CanonicalUrl
        {
            get
            {
                return new PageInfoItem("canonical")
                {
                    new PageInfoObject().RelatedLink("canonical"),
                    new OpenGraphTag(OpenGraphName.KnownNames.OgUrl, null),
                };
            }
        }

        /// <summary>
        /// Descriptor for the alternate language url of your page.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="href">The href.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public static PageInfoItem AlternateLanguage(CultureInfo culture, string href, string contentType)
        {
            return AlternateLanguage(culture.Name, href, contentType);
        }

        /// <summary>
        /// Descriptor for the alternate language url of your page.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        /// <param name="href">The href.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public static PageInfoItem AlternateLanguage(string cultureName, string href, string contentType)
        {
            return new PageInfoItem("alternate")
            {
                new PageInfoObject().RelatedLink("alternate", hrefLang: cultureName, href: href, type: contentType),
                new OpenGraphTag(OpenGraphName.KnownNames.OgLocaleAlternate, cultureName.Replace("-", "_")),
            };
        }

        /// <summary>
        /// Descriptor for the name of the site.
        /// </summary>
        public static PageInfoItem SiteName
        {
            get
            {
                return new PageInfoItem("sitename")
                {
                    new OpenGraphTag(OpenGraphName.KnownNames.OgSiteName, null),
                };
            }
        }

        /// <summary>
        /// Descriptor for the site URL.
        /// </summary>
        public static PageInfoItem SiteUrl
        {
            get
            {
                // <meta name="identifier-url" content="http://mywebsite.com/" />
                return new PageInfoItem("siteurl")
                {
                    new PageInfoObject(PageInfoObjectSection.OldMeta).MetaWithValue("identifier-url"),
                };
            }
        }

        /// <summary>
        /// Descriptor for the favicon location.
        /// </summary>
        public static PageInfoItem Favicon
        {
            get
            {
                // <link href="/favicon.ico" rel="shortcut icon" type="image/x-icon" />
                return new PageInfoItem("favicon")
                {
                    new PageInfoObject(PageInfoObjectSection.Basic).RelatedLink("shortcut icon", null, "/favicon.ico", "image"),
                };
            }
        }

        /// <summary>
        /// Gets the associated open graph tree.
        /// </summary>
        public OpenGraphObject OpenGraph
        {
            get
            {
                if (this.openGraph != null)
                {
                    foreach (var item in this.items)
                    {
                        foreach (var obj in item)
                        {
                            if (obj.OpenGraphTag != null)
                            {
                                if (obj.OpenGraphTag.Value == null)
                                {
                                    obj.OpenGraphTag.Value = item.Value;
                                }
                            }
                        }
                    }
                }

                return this.openGraph;
            }
        }

        public static PageInfoItem RevisitAfter(int days)
        {
            return new PageInfoItem("revisit-after")
            {
                new PageInfoObject().MetaWithValue("revisit-after", days.ToString("D") + " days"),
            };
        }

        /// <summary>
        /// Sets the specified page information item.
        /// </summary>
        /// <param name="pageInfoItem">The page information item.</param>
        /// <returns></returns>
        public PageInfo Set(PageInfoItem pageInfoItem)
        {
            this.Set(pageInfoItem, null);
            return this;
        }

        /// <summary>
        /// Sets the specified page information item.
        /// </summary>
        /// <param name="pageInfoItem">The page information item.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public PageInfo Set(PageInfoItem pageInfoItem, string value)
        {
            if (pageInfoItem.Value == null)
            {
                pageInfoItem.SetValue(value);
            }

            this.items.Add(pageInfoItem);

            var ogItems = pageInfoItem.Where(o => o.Section == PageInfoObjectSection.OpenGraph).ToArray();
            foreach(var ogItem in ogItems)
            {
                var openGraph = this.openGraph ?? (this.openGraph = new OpenGraphObject());
                openGraph.Add(ogItem.OpenGraphTag);
            }

            return this;
        }

        /// <summary>
        /// Overwrites the specified page information item.
        /// </summary>
        /// <param name="pageInfoItem">The page information item.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public PageInfo Overwrite(PageInfoItem pageInfoItem, string value)
        {
            var removes = this.items.Where(i => i.Name == pageInfoItem.Name).ToArray();
            foreach (var remove in removes)
            {
                if (this.items.Contains(remove))
                    this.items.Remove(remove);
            }

            this.Set(pageInfoItem, value);
            return this;
        }

        public bool Contains(string name)
        {
            return this.items.Any(item => item.Name == name);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that contains the generated HTML tags.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that contains the generated HTML tags.
        /// </returns>
        public override string ToString()
        {
            return this.ToString(defaultSections, false);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="sections">The sections.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(PageInfoObjectSection sections, bool indented)
        {
            var sb = new StringBuilder();
            this.Write(sb, sections, indented);
            return sb.ToString();
        }

        /// <summary>
        /// Generates HTML tags into the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The sb.</param>
        public void Write(StringBuilder sb, PageInfoObjectSection sections, bool indented)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            foreach (var item in this.items)
            {
                item.ToString(sb, sections, indented);
            }
        }
    }
}
