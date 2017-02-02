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

namespace SrkToolkit.DataAnnotations
{
    using SrkToolkit.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Validates a international phone number.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneNumberAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// The regex used to validate a phone number.
        /// </summary>
        public const string PhoneRegex = @"^(\+|00)[0-9\(\)\-\. ]+$";

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumberAttribute"/> class.
        /// </summary>
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
        /// <param name="result">the converted number</param>
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
