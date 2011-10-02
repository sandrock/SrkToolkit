using System.Collections.Generic;
using System.Linq;

namespace SrkToolkit.ServiceDefinition {
    public class Method {
        public string Name { get; set; }
        public string Description { get; set; }	
        public MethodParam Return { get; set; }
        public IList<MethodParam> Params { get; set; }
        public IList<MethodParam> Faults { get; set; }

        internal Method Clone() {
            return new Method {
                Name = this.Name,
                Description = this.Description,
                Return = this.Return.Clone(),
                Params = this.Params.Select(p => p.Clone()).ToList(),
                Faults = this.Faults.Select(f => f.Clone()).ToList(),
            };
        }
    }
}
