
namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class SrkStringExtensions
    {
        /// <summary>
        /// Returns null is the string is empty.
        /// </summary>
        /// <param name="value">The value.</param>
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
        /// Returns null is the string is empty or contains only whispaces.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string NullIfEmptyOrWhitespace(this string value, bool trim = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            if (trim)
                return value.Trim();
            return value;
        }
    }
}
