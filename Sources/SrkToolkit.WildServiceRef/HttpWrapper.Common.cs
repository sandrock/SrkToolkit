using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace SrkToolkit.WildServiceRef {

    /// <summary>
    /// Default wrapper to execute HTTP requests.
    /// </summary>
    public partial class HttpWrapper {

        /// <summary>
        /// Formating string for query string.
        /// Must be set from sub-class.
        /// </summary>
        protected readonly string UserAgent;

        /// <summary>
        /// You have to specify a user-agent.
        /// </summary>
        /// <param name="userAgent"></param>
        public HttpWrapper(string userAgent) {
            UserAgent = userAgent;
        }

        /// <summary>
        /// Created as <see cref="HttpStatusCodeHandler"/> if not set.
        /// </summary>
        public IHttpStatusCodeHandler HttpStatusCodeHandler {
            get { return _httpStatusCodeHandler ?? (_httpStatusCodeHandler = new HttpStatusCodeHandler()); }
            set { _httpStatusCodeHandler = value; }
        }
        private IHttpStatusCodeHandler _httpStatusCodeHandler;

        private void HandleHttpCodes(HttpStatusCode httpStatusCode) {
            HttpStatusCodeHandler.Handle((int)httpStatusCode);
        }

    }
}
