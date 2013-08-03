// -----------------------------------------------------------------------
// <copyright file="SrkDateTimeExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for the <see cref="DateTime"/> struct.
    /// </summary>
    public static class SrkDateTimeExtensions
    {
        /// <summary>
        /// Returns the specified date at 00:00:00.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>the specified date at 00:00:00</returns>
        public static DateTime GetDate(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, 0, value.Kind);
        }
    }
}
