
namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Result for a domain request.
    /// Incudes a basic error list and a success boolean.
    /// </summary>
    public class BasicResult : IBaseResult
    {
        private IList<BasicResultError> errors;

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IList<BasicResultError> Errors
        {
            get { return this.errors ?? (this.errors = new List<BasicResultError>()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
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
