
namespace SrkToolkit.DataAnnotations
{
    using SrkToolkit.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneNumberAttribute : RegularExpressionAttribute
    {
        public const string PhoneRegex = @"^(\+|00)[0-9\(\)\-\. ]+$";

        public PhoneNumberAttribute()
            : base(PhoneRegex)
        {
            this.ErrorMessageResourceName = "PhoneNumberAttribute_ErrorMessage";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        /// <summary>
        /// Converts a national phone number to the international format;
        /// </summary>
        /// <param name="value">a supported national phone number</param>
        /// <param name="cultureInfo"></param>
        /// <returns>an international phone number</returns>
        /// <exception cref="InvalidOperationException">the national phone number is not supported</exception>
        public static bool ConvertNationalToInternational(string value, CultureInfo cultureInfo, out string result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                throw new ArgumentException("Empty phone number", "value");
            }

            if (Regex.IsMatch(value, PhoneRegex))
            {
                result = value;
                return true;
            }

            value = value.Trim();
            if (cultureInfo.Name == "fr-FR")
            {
                if (value.StartsWith("0"))
                {
                    result = "+33" + value.Substring(1);
                    return true;
                }
                else
                {
                    throw new InvalidOperationException("National phone number '" + value + "' is invalid for culture " + cultureInfo.NativeName);
                }
            }
            else
            {
                throw new InvalidOperationException("National phone number '" + value + "' is not supported for culture " + cultureInfo.NativeName);
            }
        }
    }
}
