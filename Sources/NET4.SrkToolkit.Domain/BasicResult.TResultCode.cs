
namespace SrkToolkit.Domain
{
    using SrkToolkit.Domain.Internals;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Result for a domain request.
    /// Incudes a code-based error list and a success boolean.
    /// </summary>
    [DataContract(Namespace = Names.DataContractNamespace)]
    public class BasicResult<TResultCode> : IBaseResult
        where TResultCode : struct
    {
        private IList<ResultError<TResultCode>> errors;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{TRequest, TResultCode}"/> class.
        /// </summary>
        public BasicResult()
        {
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [DataMember(IsRequired = false, Order = 1)]
        public IList<ResultError<TResultCode>> Errors
        {
            get { return this.errors ?? (this.errors = new List<ResultError<TResultCode>>()); }
            set { this.errors = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the operation succeeded; otherwise, <c>false</c>.
        /// </value>
        [DataMember(IsRequired = false, Order = 0)]
        public bool Succeed { get; set; }

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
