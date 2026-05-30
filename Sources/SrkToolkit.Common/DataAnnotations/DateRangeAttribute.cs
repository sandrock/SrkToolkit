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
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using SrkToolkit.Resources;

    /// <summary>
    /// Validates a <see cref="DateTime"/> property by specifying a lower and upper bounds.
    /// Minimum and Maximum must be ISO 8601 strings (e.g. "2015-01-01T00:00:00" or "2015-01-01 00:00").
    /// </summary>
    public class DateRangeAttribute : ValidationAttribute
    {
        private static readonly string[] iso8601Formats = new[]
        {
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm",
            "yyyy-MM-dd HH:mm",
            "yyyy-MM-dd",
        };

        private const string ErrorDisplayFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeAttribute"/> class.
        /// </summary>
        /// <param name="Minimum">The minimum.</param>
        /// <param name="Maximum">The maximum.</param>
        public DateRangeAttribute(string Minimum = null, string Maximum = null)
        {
            this.Minimum = Minimum;
            this.Maximum = Maximum;
            this.ErrorMessageResourceType = typeof(Strings);
        }

        /// <summary>
        /// Gets or sets the minimum date (ISO 8601 string).
        /// </summary>
        public string Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum date (ISO 8601 string).
        /// </summary>
        public string Maximum { get; set; }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false.
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is string)
            {
                var stringValue = (string)value;
                if (!string.IsNullOrEmpty(stringValue))
                {
                    if (TryParseIso(stringValue, out DateTime date))
                    {
                        value = date;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (value is DateTime)
            {
                var val = (DateTime)value;

                if (this.Minimum != null)
                {
                    if (!TryParseIso(this.Minimum, out DateTime minDate))
                    {
                        throw new InvalidOperationException("DateRangeAttribute.Minimum is not a valid ISO 8601 date string: " + this.Minimum);
                    }

                    if (val < minDate)
                    {
                        return false;
                    }
                }

                if (this.Maximum != null)
                {
                    if (!TryParseIso(this.Maximum, out DateTime maxDate))
                    {
                        throw new InvalidOperationException("DateRangeAttribute.Maximum is not a valid ISO 8601 date string: " + this.Maximum);
                    }

                    if (maxDate < val)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>
        /// An instance of the formatted error message.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            if (this.Minimum != null && this.Maximum != null)
            {
                if (!TryParseIso(this.Minimum, out DateTime minDate))
                {
                    throw new InvalidOperationException("DateRangeAttribute.Minimum is not a valid ISO 8601 date string: " + this.Minimum);
                }

                if (!TryParseIso(this.Maximum, out DateTime maxDate))
                {
                    throw new InvalidOperationException("DateRangeAttribute.Maximum is not a valid ISO 8601 date string: " + this.Maximum);
                }

                return string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.DateRangeAttribute_ErrorMessage_MinMax,
                    name,
                    minDate.ToString(ErrorDisplayFormat, CultureInfo.InvariantCulture),
                    maxDate.ToString(ErrorDisplayFormat, CultureInfo.InvariantCulture));
            }
            else if (this.Minimum != null)
            {
                if (!TryParseIso(this.Minimum, out DateTime minDate))
                {
                    throw new InvalidOperationException("DateRangeAttribute.Minimum is not a valid ISO 8601 date string: " + this.Minimum);
                }

                return string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.DateRangeAttribute_ErrorMessage_Min,
                    name,
                    minDate.ToString(ErrorDisplayFormat, CultureInfo.InvariantCulture));
            }
            else if (this.Maximum != null)
            {
                if (!TryParseIso(this.Maximum, out DateTime maxDate))
                {
                    throw new InvalidOperationException("DateRangeAttribute.Maximum is not a valid ISO 8601 date string: " + this.Maximum);
                }

                return string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.DateRangeAttribute_ErrorMessage_Max,
                    name,
                    maxDate.ToString(ErrorDisplayFormat, CultureInfo.InvariantCulture));
            }
            else
            {
                return string.Format(CultureInfo.CurrentCulture, Strings.DateRangeAttribute_ErrorMessage_WTF, name);
            }
        }

        private static bool TryParseIso(string value, out DateTime result)
        {
            return DateTime.TryParseExact(value, iso8601Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }
    }
}
