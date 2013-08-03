
namespace SrkToolkit.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using SrkToolkit.Resources;

    /// <summary>
    /// Validates an email address.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailAddressAttribute : ValidationAttribute
    {
        /// <summary>
        /// The regex pattern to match an email address.
        /// </summary>
        public const string RegexPattern = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressAttribute"/> class.
        /// </summary>
        public EmailAddressAttribute()
        {
            this.ErrorMessageResourceName = "EmailAddressAttribute_ErrorMessage";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        /// <summary>
        /// Gets or sets the regex.
        /// </summary>
        /// <value>
        /// The regex.
        /// </value>
        protected Regex Regex { get; set; }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false.
        /// </returns>
        public override bool IsValid(object value)
        {
            this.SetupRegex();

            string text = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            Match match = this.Regex.Match(text);
            return match.Success && match.Index == 0 && match.Length == text.Length;
        }

        private void SetupRegex()
        {
            if (this.Regex == null)
            {
                this.Regex = new Regex(RegexPattern, RegexOptions.IgnoreCase);
            }
        }
    }
}
