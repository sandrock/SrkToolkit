using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrkToolkit.ServiceDefinition {
    public class GeneratorBase {

        IWriter writer;

        string indentUnit = "    ";
        string line = Environment.NewLine;
        int namespaceIndentLevel = 0;
        int typeIndentLevel = 1;
        int memberIndentLevel = 2;
        int memberCodeIndentLevel = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorBase"/> class.
        /// </summary>
        public GeneratorBase() {
        }

        public virtual void Initialize(IWriter writer) {
            this.writer = writer;
        }

        public virtual void Run(Generation gen, IWriter writer) {
            throw new NotSupportedException();
        }

        #region High-level write methods



        #endregion

        #region Mid-level write methods

        protected void WriteBeginNamespace(string @namespace) {
            WriteLine(namespaceIndentLevel, "namespace " + @namespace);
            WriteLine(namespaceIndentLevel, "{");
        }

        protected void WriteUsing(string name) {
            WriteLine(typeIndentLevel, "using " + name + ";");
        }

        protected void WriteTypeAttribute(string attributeType, params string[] keyValuePairs) {
            Indent(typeIndentLevel);
            Write("[");
            Write(attributeType);
            Write("(");

            if (keyValuePairs != null) {
                bool isKey = true;
                string separator = string.Empty;
                foreach (var item in keyValuePairs) {
                    if (isKey) {
                        Write(separator + item);
                    } else {
                        Write(" = ");
                        if (item.StartsWith("http://"))
                            Write("\"" + item + "\"");
                    }

                    separator = ", ";
                    isKey = !isKey;
                } 
            }
            WriteLine();
        }

        protected void WriteTypeCommentSummary(string description) {
            WriteLine(typeIndentLevel, "/// <summary>");
            WriteLine(typeIndentLevel, "/// " + description);
            WriteLine(typeIndentLevel, "/// </summary>");
        }

        protected void WriteBeginType(TypeAccessibility accessibility, string kind, string name) {
            switch (accessibility) {
                case TypeAccessibility.Private:
                    Write("private ");
                    break;
                case TypeAccessibility.Protected:
                    Write("protected ");
                    break;
                case TypeAccessibility.ProtectedInternal:
                    Write("protected internal ");
                    break;
                case TypeAccessibility.Internal:
                    Write("internal ");
                    break;
                case TypeAccessibility.Public:
                    Write("public ");
                    break;
                default:
                    break;
            }

            Write("partial " + kind + " " + name);
            WriteLine();
            WriteLine(typeIndentLevel, "{");
        }

        protected void WriteBeginRegion(string regionName) {
            WriteLine(memberIndentLevel, "#region " + regionName);
        }

        protected void WriteEndRegion(string regionName) {
            WriteLine(memberIndentLevel, "#endregion " + regionName);
        }

        protected void WriteMemberCommentSummary(string description) {
            WriteLine(memberIndentLevel, "/// <summary>");
            WriteLine(memberIndentLevel, "/// " + description);
            WriteLine(memberIndentLevel, "/// </summary>");
        }

        protected void WriteMemberCommentParam(string name, string description) {
            WriteLine(memberIndentLevel, "/// <param name=\"" + name + "\">");
            WriteLine(memberIndentLevel, "/// " + description);
            WriteLine(memberIndentLevel, "/// </param>");
        }

        protected void WriteMemberCommentReturns(string description) {
            WriteLine(memberIndentLevel, "/// <returns>");
            WriteLine(memberIndentLevel, "/// " + description);
            WriteLine(memberIndentLevel, "/// </returns>");
        }

        protected void WriteEndType() {
            WriteLine(typeIndentLevel, "}");
        }

        protected void WriteEndNamespace() {
            WriteLine(namespaceIndentLevel, "}");
        }

        #endregion

        #region Low-level write methods

        private void WriteLine(int indentLevel, string content) {
            for (int i = 0; i < indentLevel; i++) {
                writer.Write(indentUnit);
            }
            writer.Write(content);
            writer.Write(line);
        }

        private void Indent(int indentLevel) {
            for (int i = 0; i < indentLevel; i++) {
                writer.Write(indentUnit);
            }
        }

        private void Write(string content) {
            writer.Write(content);
        }

        private void WriteLine() {
            writer.Write(line);
        }

        #endregion
    }
}
