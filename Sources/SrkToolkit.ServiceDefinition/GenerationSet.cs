using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrkToolkit.ServiceDefinition {
    public class GenerationSet : List<Generation> {

        public GenerationSet() {
        }
        
        public IList<string> DefaultUsings { get; set; }

    }
}
