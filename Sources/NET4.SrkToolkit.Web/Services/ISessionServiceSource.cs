
namespace SrkToolkit.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Text;

    /// <summary>
    /// Abstraction of <see cref="HttpSessionStateBase"/>.
    /// </summary>
    public interface ISessionServiceSource
    {
        /// <summary>
        /// Gets on object by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Clears an object from session.
        /// </summary>
        /// <param name="key"></param>
        void Clear(string key);

        /// <summary>
        /// Set an object in session.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, object value);

        /// <summary>
        /// Clears the session.
        /// </summary>
        void Clear();
    }
}
