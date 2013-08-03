
namespace SrkToolkit.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// ViewModel representing a HTTP error with details and description.
    /// </summary>
    public class HttpErrorModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpErrorModel"/> class.
        /// </summary>
        public HttpErrorModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpErrorModel"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
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

        /// <summary>
        /// Gets or sets the HTTP code.
        /// </summary>
        /// <value>
        /// The HTTP code.
        /// </value>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the URL path.
        /// </summary>
        /// <value>
        /// The URL path.
        /// </value>
        public string UrlPath { get; set; }

        /// <summary>
        /// Gets or sets the top link title.
        /// </summary>
        /// <value>
        /// The top link title.
        /// </value>
        public string TopLinkTitle { get; set; }

        /// <summary>
        /// Gets or sets the top link href.
        /// </summary>
        /// <value>
        /// The top link href.
        /// </value>
        public string TopLinkHref { get; set; }

        /// <summary>
        /// Gets or sets the error action.
        /// </summary>
        /// <value>
        /// The error action.
        /// </value>
        public string ErrorAction { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }
    }
}
