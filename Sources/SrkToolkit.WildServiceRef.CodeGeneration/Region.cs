using System.Collections.Generic;

namespace SrkToolkit.WildServiceRef.CodeGeneration {
    public class Region {
        public string Name { get; set; }
        public IList<Method> Methods { get; set; }
    }
}
