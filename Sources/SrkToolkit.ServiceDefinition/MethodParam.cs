

namespace SrkToolkit.ServiceDefinition {
    public class MethodParam {

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is persistent.
        /// </summary>
        public bool IsPersistent { get; set; }

        internal MethodParam Clone() {
            return new MethodParam {
                Name = this.Name,
                Type = this.Type,
                Description = this.Description,
                IsPersistent = this.IsPersistent
            };
        }
    }
}
