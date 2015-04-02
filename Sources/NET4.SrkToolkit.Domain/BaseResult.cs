
namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Result for a domain request.
    /// Incudes a request store, a code-based error list and a success boolean.
    /// </summary>
    public class BaseResult<TRequest, TResultCode> : IBaseResult
        where TRequest : class
        where TResultCode : struct
    {
        private readonly TRequest request;

        private IList<ResultError<TResultCode>> errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{TRequest, TResultCode}"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public BaseResult(TRequest request)
        {
            this.request = request;
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public TRequest Request
        {
            get { return this.request; }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IList<ResultError<TResultCode>> Errors
        {
            get { return this.errors ?? (this.errors = new List<ResultError<TResultCode>>()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        public bool Succeed { get; set; }

        /// <summary>
        /// Gets a copy of the errors' collection.
        /// </summary>
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
