
namespace SrkToolkit.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using SrkToolkit.Resources;
    using System.Reflection;

    /// <summary>
    /// Validates an email address field (one or multiple addresses).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
#if NET45
    public class EmailAddressExAttribute : ValidationAttribute
#else
    public class EmailAddressAttribute : ValidationAttribute
#endif
    {
        /// <summary>
        /// The regex pattern to match an email address.
        /// </summary>
        ////public   const string SingleRegexPattern = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";
        public const string EmailAddressRegex = @"[a-z0-9\u007F-\uFFFF][a-z0-9\u007F-\uFFFF_\.+-]+@[a-z0-9\.-]+\.[a-z0-9]+";
        private Func<string>[] errorMessageAccessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressAttribute"/> class.
        /// </summary>
#if NET45
        public EmailAddressExAttribute()
#else
        public EmailAddressAttribute()
#endif
        {
            this.ErrorMessageResourceName = "EmailAddressAttribute_ErrorMessage";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow multiple addresses in a single field. Default is false. Use <see cref="SrkToolkit.Common.Validation.Validate.ManyEmailAddresses"/> to parse addresses.
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of addresses (when AllowMultiple is true).
        /// </summary>
        public int MinimumAddresses { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of addresses (when AllowMultiple is true).
        /// </summary>
        public int MaximumAddresses { get; set; }

        /// <summary>
        /// Gets or sets the regex.
        /// </summary>
        /// <value>
        /// The regex.
        /// </value>
        protected Regex Regex { get; set; }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            this.SetupRegex();
            this.SetResourceAccessorByPropertyLookup();

            string text = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (this.AllowMultiple)
            {
                var addresses = SrkToolkit.Common.Validation.Validate.ManyEmailAddresses(text);
                int count = 0;
                foreach (var address in addresses)
                {
                    count++;
                }

                if (count < this.MinimumAddresses)
                {
                    return new ValidationResult(string.Format(
                        this.errorMessageAccessors[1](),
                        count,
                        this.MinimumAddresses));
                }

                if (this.MaximumAddresses > 0 && count > this.MaximumAddresses)
                {
                    return new ValidationResult(string.Format(
                        this.errorMessageAccessors[2](),
                        count,
                        this.MaximumAddresses));
                }

                return null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return null;
                }

                Match match = this.Regex.Match(text);
                if (match.Success && match.Index == 0 && match.Length == text.Length)
                {
                    return null;
                }
                else
                {
                    return new ValidationResult(this.ErrorMessageString);
                }
            }
        }

        private void SetupRegex()
        {
            if (this.Regex == null)
            {
                this.Regex = new Regex("^" + EmailAddressRegex + "$", RegexOptions.IgnoreCase);
            }
        }

        private void SetResourceAccessorByPropertyLookup()
        {
            // this attribute may give 3 different error messages
            // this method sets up 3 lookups for resources
            // inspired from ValidationAttribute's SetResourceAccessorByPropertyLookup method

            if (!(this.ErrorMessageResourceType != null) || string.IsNullOrEmpty(this.ErrorMessageResourceName))
            {
                throw new InvalidOperationException(Strings.ValidationAttribute_NeedBothResourceTypeAndResourceName);
            }

            string[] properties = new string[]
            {
                this.ErrorMessageResourceName, // single mode, invalid address
                this.ErrorMessageResourceName + "_MultipleMinimumAddresses", // multi mode, not enough addresses
                this.ErrorMessageResourceName + "_MultipleMaximumAddresses", // multi mode, too many addresses
            };

            var accessors = new Func<string>[3];

            for (int i = 0; i < properties.Length; i++)
            {
                string propertyName = properties[i];
                PropertyInfo property = this.ErrorMessageResourceType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                if (property != null)
                {
                    MethodInfo getMethod = property.GetGetMethod(true);
                    if (getMethod == null || (!getMethod.IsAssembly && !getMethod.IsPublic))
                    {
                        property = null;
                    }
                }

                if (property == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.ValidationAttribute_ResourceTypeDoesNotHaveProperty, new object[]
                    {
                        this.ErrorMessageResourceType.FullName,
                        propertyName
                    }));
                }

                if (property.PropertyType != typeof(string))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.ValidationAttribute_ResourcePropertyNotStringType, new object[]
                        {
                            property.Name,
                            this.ErrorMessageResourceType.FullName
                        }));
                }

                accessors[i] = new Func<string>(() => (string)property.GetValue(null, null));
            }

            this.errorMessageAccessors = accessors;
        }

        /// <summary>
        /// Gets the is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public ValidationResult GetIsValid(object value, ValidationContext context)
        {
            return this.IsValid(value, context);
        }
    }
}
