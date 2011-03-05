using System.Collections.Generic;

namespace SrkToolkit.WildServiceRef.CodeGeneration {
    public class Method {
        public string Name { get; set; }
        public MethodParam Return { get; set; }
        public IList<MethodParam> Params { get; set; }
    }
}
