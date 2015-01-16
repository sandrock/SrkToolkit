
namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Result for a domain request.
    /// Incudes a code-based error list and a success boolean.
    /// </summary>
    public class BasicResult<TResultCode, TData> : BasicResult<TResultCode>
        where TResultCode : struct
    {
        private IList<ResultError<TResultCode>> errors;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{TRequest, TResultCode}"/> class.
        /// </summary>
        public BasicResult()
        {
        }

        public BasicResult(TData data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IList<ResultError<TResultCode>> Errors
        {
            get { return this.errors ?? (this.errors = new List<ResultError<TResultCode>>()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the operation succeeded; otherwise, <c>false</c>.
        /// </value>
        public bool Succeed { get; set; }

        public TData Data { get; set; }
    }
}
