// -----------------------------------------------------------------------
// <copyright file="DateTimePrecision.cs" company="">
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
    /// THe precision of a <see cref="DateTime" />.
    /// </summary>
    public enum DateTimePrecision
    {
        /// <summary>
        /// The year
        /// </summary>
        Year,

        /// <summary>
        /// The month
        /// </summary>
        Month,

        /// <summary>
        /// The day
        /// </summary>
        Day,

        /// <summary>
        /// The hour
        /// </summary>
        Hour,

        /// <summary>
        /// The minute
        /// </summary>
        Minute,

        /// <summary>
        /// The second
        /// </summary>
        Second,

        /// <summary>
        /// The millisecond
        /// </summary>
        Millisecond,
    }
}
