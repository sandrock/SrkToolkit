using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrkToolkit.ServiceDefinition {
    public class WcfContactGenerator : GeneratorBase {

        public WcfContactGenerator() {

        }

        public override void Run(Generation gen, IWriter writer) {
            var methods = new List<MethodParam>();

            ////foreach (var region in gen.Contract.Regions) {
            ////    foreach (var method in region.Methods) {
            ////        var newMethod = method.Clone();
            ////    }
            ////}

            this.Run(gen, writer, methods);
        }

        private void Run(Generation gen, IWriter writer, IList<MethodParam> methods) {
            //
            // namespace
            //

            WriteBeginNamespace(gen.Namespace);

            //
            // usings
            //

            foreach (var item in gen.Usings) {
                WriteUsing(item);
            }
            foreach (var item in gen.Contract.Usings) {
                if (!gen.Usings.Contains(item))
                    WriteUsing(item);
            }
            foreach (var item in gen.Set.DefaultUsings) {
                if (!gen.Usings.Contains(item))
                    if (!gen.Contract.Usings.Contains(item))
                        WriteUsing(item);
            }

            //
            // type comment, attributes, main line
            //

            WriteTypeCommentSummary(gen.Contract.Description);
            WriteTypeAttribute(
                "System.ServiceModel.ServiceContract",
                "Namespace", gen.Contract.Namespace,
                "Name", gen.Contract.Name ?? gen.TypeName ?? "\" #warning no name specified");
            WriteBeginType(gen.Accessibility, "interface", gen.TypeName ?? gen.Contract.Name);

            //
            // regions
            //

            foreach (var region in gen.Contract.Regions) {
                WriteBeginRegion(region.Name);

                //
                // methods
                //
                foreach (var method in region.Methods) {
                    //
                    // method: comments
                    //
                    WriteMemberCommentSummary(method.Description);
                    WriteMethodHeader(access);

#warning code being written // TODO: code being written (SandRock)

                }

                WriteEndRegion(region.Name);
            }

            WriteEndType();
            WriteEndNamespace();
        }
    }
}
