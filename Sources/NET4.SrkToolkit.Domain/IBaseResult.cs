
namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Result for a domain request.
    /// </summary>
    public interface IBaseResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        bool Succeed { get; }

        /// <summary>
        /// Gets a copy of the errors' collection.
        /// </summary>
        IList<IResultError> Errors { get; }
    }
}
