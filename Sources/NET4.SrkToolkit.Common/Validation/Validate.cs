
namespace SrkToolkit.Common.Validation
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using SrkToolkit.DataAnnotations;

    /// <summary>
    /// Validates various data.
    /// </summary>
    public static class Validate
    {
#if NET45
        private static readonly Regex emailRegex = new Regex(@"^" + EmailAddressExAttribute.EmailAddressRegex + "$", RegexOptions.IgnoreCase);
        private static readonly Regex emailsRegex = new Regex(EmailAddressExAttribute.EmailAddressRegex, RegexOptions.IgnoreCase);
#else
        private static readonly Regex emailRegex = new Regex(@"^" + EmailAddressAttribute.EmailAddressRegex + "$", RegexOptions.IgnoreCase);
        private static readonly Regex emailsRegex = new Regex(EmailAddressAttribute.EmailAddressRegex, RegexOptions.IgnoreCase);
#endif

        /// <summary>
        /// Validates and lower-ify an email address;
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns a clean email address or null if it is not valid.</returns>
        public static string EmailAddress(string value)
        {
            if (value == null)
                return null;

            var better = value.ToLowerInvariant().Trim();
            if (emailRegex.IsMatch(better))
                return better;
            return null;
        }

        /// <summary>
        /// Extracts many email addresses from a string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IEnumerable<string> ManyEmailAddresses(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                yield break;

            var matches = emailsRegex.Matches(input);

            foreach (Match match in matches)
            {
                yield return match.Value.ToLowerInvariant();
            }
        }
    }
}
