using System.Collections.Generic;
using System.Xml.Linq;

namespace SrkToolkit.ServiceDefinition {
    public class Contract {

        /// <summary>
        /// Gets or sets the contract SOAP name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the contract SOAP namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the regions (they contain methods).
        /// </summary>
        public IList<Region> Regions { get; set; }

        /// <summary>
        /// Gets or sets the using directives.
        /// </summary>
        public IList<string> Usings { get; set; }

        /// <summary>
        /// Gets or sets the faults.
        /// </summary>
        public IList<MethodParam> Faults { get; set; }

        internal XElement XElement { get; set; }
    }
}
