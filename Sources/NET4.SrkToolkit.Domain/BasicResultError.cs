
namespace SrkToolkit.Domain
{
    using SrkToolkit.Domain.Internals;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Result for a domain request.
    /// </summary>
    [DataContract(Namespace = Names.DataContractNamespace)]
    public class BasicResultError : IResultError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicResultError"/> class.
        /// </summary>
        public BasicResultError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicResultError"/> class.
        /// </summary>
        /// <param name="displayMessage">The display message.</param>
        public BasicResultError(string displayMessage)
        {
            this.DisplayMessage = displayMessage;
        }

        /// <summary>
        /// Gets or sets the display message.
        /// </summary>
        [DataMember(IsRequired = false, Order = 1)]
        public string DisplayMessage { get; set; }

        [DataMember(IsRequired = false, Order = 2)]
        public string Detail { get; set; }

        [DataMember(IsRequired = false, Order = 0)]
        string IResultError.Code
        {
            get { return this.DisplayMessage; }
        }
    }
}
