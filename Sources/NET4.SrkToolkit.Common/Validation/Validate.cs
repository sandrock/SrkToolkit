// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

namespace SrkToolkit.Common.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using SrkToolkit.DataAnnotations;

    /// <summary>
    /// Validates various data.
    /// </summary>
    public static class Validate
    {
#if NET40
        private static readonly Regex emailRegex = new Regex(@"^" + EmailAddressExAttribute.EmailAddressRegex + "$", RegexOptions.IgnoreCase);
        private static readonly Regex emailsRegex = new Regex(EmailAddressExAttribute.EmailAddressRegex, RegexOptions.IgnoreCase);
#else
        private static readonly Regex emailRegex = new Regex(@"^" + EmailAddressExAttribute.EmailAddressRegex + "$", RegexOptions.IgnoreCase);
        private static readonly Regex emailsRegex = new Regex(EmailAddressExAttribute.EmailAddressRegex, RegexOptions.IgnoreCase);
#endif
        private static readonly Regex regexCaptureBracket = new Regex(@"\(.*\)", RegexOptions.Compiled);

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

        /// <summary>
        /// Internationalize and clean the phone number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns a clean phone number or null if it is not valid.</returns>
        public static string PhoneNumber(string value)
        {
            return PhoneNumber(value, false);
        }

        /// <summary>
        /// Internationalize and clean the phone number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="acceptNonIntlFormats">if set to <c>true</c> accept non international formats.</param>
        /// <returns>
        /// Returns a clean phone number or null if it is not valid.
        /// </returns>
        public static string PhoneNumber(string value, bool acceptNonIntlFormats)
        {
            string[] separators = new string[] { ".", "/", "-", };
            var result = value;

            // remove spaces & brackets
            result = result.RemoveSpaces();
            result = regexCaptureBracket.Replace(result, string.Empty);

            // remove separators
            foreach (var sep in separators)
            {
                result = result.Replace(sep, string.Empty);
            }

            // already international
            if (result.StartsWith("+"))
                return result;

            // international the other way: 0033
            if (result.StartsWith("00") && result.Length > 3 && result[2] != '0')
            {
                result = "+" + result.Substring(2);
                return result;
            }
            else if (acceptNonIntlFormats)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
