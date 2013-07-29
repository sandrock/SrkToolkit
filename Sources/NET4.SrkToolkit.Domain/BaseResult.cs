// -----------------------------------------------------------------------
// <copyright file="BaseResult.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Result for a domain request.
    /// </summary>
    public class BaseResult<TRequest, TResultCode>
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
        /// <value>
        /// The request.
        /// </value>
        public TRequest Request
        {
            get { return this.request; }
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
    }
}
