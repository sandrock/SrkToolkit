
namespace SrkToolkit.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// A message to display at the next loaded web page.
    /// </summary>
    public class TempMessage
    {
        /// <summary>
        /// The key to use to store temp messages.
        /// </summary>
        public const string TempDataKey = "TempMessages";

        public TempMessage()
        {
        }

        public TempMessage(TempMessageKind kind, string message, bool isMarkup)
        {
            this.Kind = kind;
            this.Message = message;
            this.IsMarkup = isMarkup;
        }

        /// <summary>
        /// Gets or sets the message (text or markup).
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the kind (Error, Warning, Info).
        /// </summary>
        public TempMessageKind Kind { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Message"/> contains markup.
        /// </summary>
        public bool IsMarkup { get; set; }
    }

    /// <summary>
    /// The type of message.
    /// </summary>
    public enum TempMessageKind
    {
        /// <summary>
        /// A generic information that is not important.
        /// </summary>
        Information,

        /// <summary>
        /// An error that prevented something to happen.
        /// </summary>
        Error,

        /// <summary>
        /// A generic confirmation that an action successfully completed.
        /// </summary>
        Confirmation,

        /// <summary>
        /// A warning that may be important to the user.
        /// </summary>
        Warning,
    }
}