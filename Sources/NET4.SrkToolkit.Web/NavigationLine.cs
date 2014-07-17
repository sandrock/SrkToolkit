
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NavigationLine : List<NavigationLineEntry>
    {
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
