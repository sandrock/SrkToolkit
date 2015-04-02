
namespace SrkToolkit.Web
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
