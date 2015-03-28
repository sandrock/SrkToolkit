
namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Result for a domain request.
    /// </summary>
    public class BasicResultError : IResultError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicResultError"/> class.
        /// </summary>
        /// <param name="displayMessage">The display message.</param>
        public BasicResultError(string displayMessage)
        {
            this.DisplayMessage = displayMessage;
        }

        /// <summary>
        /// Gets or sets the display message.
        /// </summary>
        public string DisplayMessage { get; set; }

        string IResultError.Code
        {
            get { return this.DisplayMessage; }
        }
    }
}
