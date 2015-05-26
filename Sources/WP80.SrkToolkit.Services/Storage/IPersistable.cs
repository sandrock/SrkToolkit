
namespace SrkToolkit.Services.Storage
{
    using System;

    /// <summary>
    /// Provides a data persistence method.
    /// </summary>
    public interface IPersistable
    {
        /// <summary>
        /// Persists this instance using the registered <see cref="ILocalStorageService"/>.
        /// </summary>
        void Persist();
    }
}
