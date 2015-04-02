﻿
namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class SrkStringExtensions
    {
        /// <summary>
        /// Returns null is the string is empty or null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="trim">if set to <c>true</c> [trim].</param>
        /// <returns></returns>
        public static string NullIfEmpty(this string value, bool trim = false)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            if (trim)
                return value.Trim();
            return value;
        }

        /// <summary>
        /// Returns null is the string is empty, null or contains only whispaces.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="trim">if set to <c>true</c> [trim].</param>
        /// <returns></returns>
        public static string NullIfEmptyOrWhitespace(this string value, bool trim = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            if (trim)
                return value.Trim();
            return value;
        }

        /// <summary>
        /// Trims a string to the specified length.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string TrimToLength(this string value, int length)
        {
            if (value == null)
                return null;

            if (value.Length <= length)
                return value;

            return value.Substring(0, length);
        }

        /// <summary>
        /// Determines whether the specified source contains any of the specified string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="values">The values to find.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>true if any value is found; otherwise, false</returns>
        /// <exception cref="System.ArgumentNullException">
        /// value
        /// or
        /// values
        /// </exception>
        /// <exception cref="System.ArgumentException">String comparison type is not supported</exception>
        public static bool ContainsAny(this string source, IEnumerable<string> values, StringComparison comparisonType)
        {
            if (source == null)
                throw new ArgumentNullException("value");
            if (values == null)
                throw new ArgumentNullException("values");

            if (string.IsNullOrEmpty(source))
                return false;

            var values1 = values.ToArray();
            CultureInfo culture;
            CompareOptions options = CompareOptions.None;
            switch (comparisonType)
            {
                case StringComparison.CurrentCulture:
                    culture = CultureInfo.CurrentCulture;
                    break;

                case StringComparison.CurrentCultureIgnoreCase:
                    culture = CultureInfo.CurrentCulture;
                    options = CompareOptions.IgnoreCase;
                    break;

                case StringComparison.InvariantCulture:
                    culture = CultureInfo.InvariantCulture;
                    break;

                case StringComparison.InvariantCultureIgnoreCase:
                    culture = CultureInfo.InvariantCulture;
                    options = CompareOptions.IgnoreCase;
                    break;

                case StringComparison.Ordinal:
                    culture = CultureInfo.InvariantCulture;
                    options = CompareOptions.Ordinal;
                    break;

                case StringComparison.OrdinalIgnoreCase:
                default:
                    throw new ArgumentException("String comparison " + comparisonType + " is not supported");
            }

            for (int i = 0; i < values1.Length; i++)
            {
                var value = values1[i];

                var position = culture.CompareInfo.IndexOf(source, value, options);
                if (position > -1)
                    return true;
            }

            return false;
        }
    }
}