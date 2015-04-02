using System.Collections.Generic;

namespace SrkToolkit.ServiceDefinition {

    /// <summary>
    /// Configuration for a code generation.
    /// </summary>
    public class Generation {
        /// <summary>
        /// Gets or sets the name of the contract.
        /// It must match a contract name in the service definition file.
        /// </summary>
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets the XML namespace URI.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the name of the type to generate.
        /// </summary>
        public string TypeName { get; set; }

        public TypeAccessibility Accessibility { get; set; }

        ////public GenerationKind Kind { get; set; }           // old params from wild T4
        ////public GenerationAsyncMode AsyncMode { get; set; } // old params from wild T4

        /// <summary>
        /// Gets or sets the using directives.
        /// </summary>
        public IList<string> Usings { get; set; }

        /// <summary>
        /// Gets or sets the parent classes and interfaces.
        /// </summary>
        public IList<string> Parents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to suppress persistent params from methods.
        /// </summary>
        public bool SuppressPersistentParams { get; set; }

        /// <summary>
        /// Gets or sets the generator class type name.
        /// </summary>
        public string GeneratorClass { get; set; }

        /// <summary>
        /// Gets or sets the generator class assembly name (for an external type).
        /// </summary>
        public string GeneratorAssembly { get; set; }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        internal GeneratorBase Generator { get; set; }

        /// <summary>
        /// Gets or sets the contract.
        /// </summary>
        internal Contract Contract { get; set; }

        internal GenerationSet Set { get; set; }
    }

    public enum FieldAccessibility {
        Private, Protected, ProtectedInternal, Internal, Public
    }

    public enum TypeAccessibility {
        Private, Protected, ProtectedInternal, Internal, Public
    }

    public enum GenerationKind {
        Interface,
        Client,
        SilverlightClient
    }

    public enum GenerationAsyncMode {
        Sync,
        AsyncWithEvents,
        AsyncWithCallbacks,
        WcfAsyncPattern,
        AsyncPattern,
    }
}
