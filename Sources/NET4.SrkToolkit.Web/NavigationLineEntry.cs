
namespace SrkToolkit.Web
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
