using System.Collections.Generic;

namespace SrkToolkit.ServiceDefinition {
    public class Region {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the methods.
        /// </summary>
        internal IList<Method> Methods { get; set; }
    }
}
