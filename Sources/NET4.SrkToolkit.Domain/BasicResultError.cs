// -----------------------------------------------------------------------
// <copyright file="BasicResultError.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Result for a domain request.
    /// </summary>
    public class BasicResultError
    {
        public BasicResultError(string displayMessage)
        {
            this.DisplayMessage = displayMessage;
        }

        /// <summary>
        /// Gets or sets the display message.
        /// </summary>
        public string DisplayMessage { get; set; }
    }
}
