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
    /// TODO: Update summary.
    /// </summary>
    public class DateRangeAttribute : ValidationAttribute
    {
        public DateRangeAttribute(string Minimum = null, string Maximum = null)
        {
            this.Minimum = Minimum;
            this.Maximum = Maximum;
            this.ErrorMessageResourceName = "DateRangeAttribute_ErrorMessage_WTF";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        public string Minimum { get; set; }
        public string Maximum { get; set; }

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
