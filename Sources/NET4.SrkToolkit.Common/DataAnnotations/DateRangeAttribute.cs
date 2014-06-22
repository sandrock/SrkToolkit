// -----------------------------------------------------------------------
// <copyright file="DateRangeAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using SrkToolkit.Resources;

    /// <summary>
    /// Validates a <see cref="DateTime"/> property by specifying a lower and upper bounds.
    /// </summary>
    public class DateRangeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeAttribute"/> class.
        /// </summary>
        /// <param name="Minimum">The minimum.</param>
        /// <param name="Maximum">The maximum.</param>
        public DateRangeAttribute(string Minimum = null, string Maximum = null)
        {
            this.Minimum = Minimum;
            this.Maximum = Maximum;
            this.ErrorMessageResourceName = "DateRangeAttribute_ErrorMessage_WTF";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        /// <summary>
        /// Gets or sets the minimum date.
        /// </summary>
        public string Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum date.
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
                return true;

            if (value is DateTime || value is DateTime?)
            {
                DateTime? val = (DateTime?)value;

                if (val != null)
                {
                    if (this.Minimum != null)
                    {
                        var minDate = DateTime.Parse(this.Minimum);
                        if (val < minDate)
                        {
                            return false;
                        }
                    }

                    if (this.Maximum != null)
                    {
                        var maxDate = DateTime.Parse(this.Maximum);
                        if (maxDate < val)
                        {
                            return false;
                        }
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
                var minDate = DateTime.Parse(this.Minimum);
                var maxDate = DateTime.Parse(this.Maximum);
                this.ErrorMessageResourceName = "DateRangeAttribute_ErrorMessage_MinMax";
                return string.Format(
                    CultureInfo.CurrentCulture,
                    base.ErrorMessageString,
                    name,
                    minDate.ToString(CultureInfo.CurrentCulture),
                    maxDate.ToString(CultureInfo.CurrentCulture));
            }
            else if (this.Minimum != null)
            {
                var minDate = DateTime.Parse(this.Minimum);
                this.ErrorMessageResourceName = "DateRangeAttribute_ErrorMessage_Min";
                return string.Format(
                    CultureInfo.CurrentCulture,
                    base.ErrorMessageString,
                    name,
                    minDate.ToString(CultureInfo.CurrentCulture));
            }
            else if (this.Maximum != null)
            {
                var maxDate = DateTime.Parse(this.Maximum);
                this.ErrorMessageResourceName = "DateRangeAttribute_ErrorMessage_Max";
                return string.Format(
                    CultureInfo.CurrentCulture,
                    base.ErrorMessageString,
                    name,
                    maxDate.ToString(CultureInfo.CurrentCulture));
            }
            else
            {
                this.ErrorMessageResourceName = "DateRangeAttribute_ErrorMessage_WTF";
                return base.ErrorMessageString;
            }
        }
    }
}
