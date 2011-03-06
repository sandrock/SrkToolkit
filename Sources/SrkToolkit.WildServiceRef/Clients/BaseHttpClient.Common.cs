using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrkToolkit.WildServiceRef.Clients {

    /// <summary>
    /// Base class for API implementation
    /// </summary>
    public partial class BaseHttpClient {

        #region Properties

        /// <summary>
        /// Base HTTP url for queries. 
        /// This will permit to use a different base adresse (for HTTPS, different port or domain name...).
        /// Default is http://api.betaseries.com/.
        /// </summary>
        /// <remarks>
        /// Value must be setted from .ctor.
        /// </remarks>
        protected readonly string BaseUrl;

        /// <summary>
        /// Formating string for query string.
        /// Must be set from sub-class.
        /// </summary>
        protected readonly string UrlFormat;

        /// <summary>
        /// Recommended field for application tracking.
        /// </summary>
        public string UserAgent {
            get { return _userAgent; }
            set { _userAgent = value; }
        }
        private string _userAgent;

        private string RealUserAgent {
            //VERSION: Assembly version is hard-coded
            get { return string.Format("{0} {1}", "Srk.BetaSeriesApi/0.2.1.0", UserAgent); }
        }

        /// <summary>
        /// This is a http query wrapper. Use for unit testing only.
        /// </summary>
        protected virtual IHttpWrapper http {
            get { return _http ?? (_http = new HttpWrapper(RealUserAgent)); }
            set { _http = value; }
        }
        private IHttpWrapper _http;

        #endregion

        #region .ctor

        /// <summary>
        /// Default .ctor.
        /// </summary>
        /// <param name="userAgent"></param>
        /// <param name="baseUrl"></param>
        /// <param name="urlFormat"></param>
        protected BaseHttpClient(string userAgent, string baseUrl, string urlFormat) {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentException("A base url is required", "baseUrl");

            UserAgent = userAgent ?? "unknown-client";
            BaseUrl = baseUrl;
            UrlFormat = urlFormat;
        }

        #endregion


    }
}
