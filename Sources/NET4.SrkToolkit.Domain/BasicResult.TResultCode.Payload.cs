
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

        public TData Data { get; set; }
    }
}
