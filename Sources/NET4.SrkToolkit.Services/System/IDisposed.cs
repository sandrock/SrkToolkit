
namespace System
{
    /// <summary>
    /// Defines a property to indicate whether the <see cref="System.IDisposable.Dispose"/> method was called.
    /// </summary>
    public interface IDisposed : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        bool IsDisposed { get; }
    }
}
