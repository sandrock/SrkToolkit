
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

        public HttpBaseSessionServiceSource(HttpSessionStateBase source)
        {
            this.source = source;
        }

        public object Get(string key)
        {
            if (this.source == null)
                return null;

            return this.source[key];
        }

        public void Clear(string key)
        {
            if (this.source == null)
                return;

            this.source.Remove(key);
        }

        public void Set(string key, object value)
        {
            if (this.source == null)
                return;

            this.source[key] = value;
        }

        public void Clear()
        {
            if (this.source == null)
                return;

            this.source.Clear();
        }
    }
}
