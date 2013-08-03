
namespace SrkToolkit.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Default implementation of <see cref="ISessionServiceSource"/> using <see cref="HttpSessionStateBase"/> as provider.
    /// </summary>
    public class HttpBaseSessionServiceSource : ISessionServiceSource
    {
        private readonly HttpSessionStateBase source;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBaseSessionServiceSource"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public HttpBaseSessionServiceSource(HttpSessionStateBase source)
        {
            this.source = source;
        }

        /// <summary>
        /// Gets on object by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (this.source == null)
                return null;

            return this.source[key];
        }

        /// <summary>
        /// Clears an object from session.
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            if (this.source == null)
                return;

            this.source.Remove(key);
        }

        /// <summary>
        /// Set an object in session.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (this.source == null)
                return;

            this.source[key] = value;
        }

        /// <summary>
        /// Clears the session.
        /// </summary>
        public void Clear()
        {
            if (this.source == null)
                return;

            this.source.Clear();
        }
    }
}
