
namespace SrkToolkit.Domain
{
    using System;

    /// <summary>
    /// Represents some kind of domain error.
    /// </summary>
    public interface IResultError
    {
        /// <summary>
        /// Gets the error display message.
        /// </summary>
        string DisplayMessage { get; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        string Code { get; }
    }
}
