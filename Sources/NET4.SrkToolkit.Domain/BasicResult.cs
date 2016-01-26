
namespace SrkToolkit.Domain
{
    using SrkToolkit.Domain.Internals;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Result for a domain request.
    /// Incudes a basic error list and a success boolean.
    /// </summary>
    [DataContract(Namespace = Names.DataContractNamespace)]
    public class BasicResult : IBaseResult
    {
        private IList<BasicResultError> errors;

        /// <summary>
        /// Gets the errors.
        /// </summary>
        [DataMember(IsRequired = false, Order = 1)]
        public IList<BasicResultError> Errors
        {
            get { return this.errors ?? (this.errors = new List<BasicResultError>()); }
            set { this.errors = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        [DataMember(IsRequired = false, Order = 0)]
        public bool Succeed { get; set; }

        [IgnoreDataMember]
        IList<IResultError> IBaseResult.Errors
        {
            get
            {
                if (this.errors != null)
                    return new List<IResultError>(this.errors);
                else
                    return new List<IResultError>(0);
            }
        }
    }
}
