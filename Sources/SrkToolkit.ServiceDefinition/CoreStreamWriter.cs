using System;
using System.IO;

namespace SrkToolkit.ServiceDefinition {
    public class CoreStreamWriter : IWriter {
        TextWriter textWriter;

        public CoreStreamWriter(TextWriter textWriter) {
            this.textWriter = textWriter ?? Console.Out;
        }

        public void Write(string content) {
            textWriter.WriteLine(content);
        }
    }
}
