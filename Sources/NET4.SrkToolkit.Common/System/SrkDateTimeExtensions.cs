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

        /// <summary>
        /// Returns a <see cref="DateTime"/> with the specified <see cref="DateTimePrecision"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="precision">The precision.</param>
        /// <returns></returns>
        public static DateTime ToPrecision(this DateTime value, DateTimePrecision precision)
        {
            switch (precision)
            {
                case DateTimePrecision.Year:
                    return new DateTime(value.Year, 1, 1, 0, 0, 0, value.Kind);

                case DateTimePrecision.Month:
                    return new DateTime(value.Year, value.Month, 1, 0, 0, 0, value.Kind);

                case DateTimePrecision.Day:
                    return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, value.Kind);

                case DateTimePrecision.Hour:
                    return new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0, value.Kind);

                case DateTimePrecision.Minute:
                    return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0, value.Kind);

                case DateTimePrecision.Second:
                    return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Kind);

                case DateTimePrecision.Millisecond:
                    return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Kind);

                default:
                    return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Kind);
            }
        }

        /// <summary>
        /// Determines whether a <see cref="DateTime"/> is equal to the specified <see cref="DateTime"/> base on the desired <see cref="DateTimePrecision"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dateTime">The date time.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>
        ///   <c>true</c> if a <see cref="DateTime"/> is equal to the specified <see cref="DateTime"/> base on the desired <see cref="DateTimePrecision"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEqualTo(this DateTime value, DateTime dateTime, DateTimePrecision precision)
        {
            return value.ToPrecision(precision) == dateTime.ToPrecision(precision);
        }

        /// <summary>
        /// Returns the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Local"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Local"/></returns>
        public static DateTime AsLocal(this DateTime value)
        {
            return new DateTime(value.Ticks, DateTimeKind.Local);
        }

        /// <summary>
        /// Returns the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Utc"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Utc"/></returns>
        public static DateTime AsUtc(this DateTime value)
        {
            return new DateTime(value.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Utc"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Utc"/></returns>
        public static DateTime? AsUtc(this DateTime? value)
        {
            return value != null ? value.Value.AsUtc() : default(DateTime?);
        }

        /// <summary>
        /// Returns the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Unspecified"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Unspecified"/></returns>
        public static DateTime AsUnspecified(this DateTime value)
        {
            return new DateTime(value.Ticks, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Returns the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Unspecified"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>the date with the same values and a <see cref="DateTime.Kind"/> set to <see cref="DateTimeKind.Unspecified"/></returns>
        public static DateTime? AsUnspecified(this DateTime? value)
        {
            return value != null ? value.Value.AsUnspecified() : default(DateTime?);
        }

        /// <summary>
        /// Get the unix time from a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The unix time</returns>
        public static long ToUnixTime(this DateTime value)
        {
            return (long)value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
