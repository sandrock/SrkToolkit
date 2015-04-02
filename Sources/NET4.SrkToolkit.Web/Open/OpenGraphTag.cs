
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
