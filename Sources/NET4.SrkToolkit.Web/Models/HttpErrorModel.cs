
namespace SrkToolkit.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class HttpErrorModel
    {
        public HttpErrorModel()
        {
        }

        public HttpErrorModel(string title, string message)
        {
            this.Title = title;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the main page title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the main message.
        /// </summary>
        public string Message { get; set; }

        public int Code { get; set; }
        public string UrlPath { get; set; }

        public string TopLinkTitle { get; set; }
        
        public string TopLinkHref { get; set; }

        public string ErrorAction { get; set; }

        public Exception Exception { get; set; }
    }
}
