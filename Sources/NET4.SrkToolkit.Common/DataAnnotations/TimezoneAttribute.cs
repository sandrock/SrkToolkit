
namespace SrkToolkit.DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using SrkToolkit.Resources;

    /// <summary>
    /// Validates the field is a valid <see cref="TimeZoneInfo"/> ID.
    /// </summary>
    public class TimezoneAttribute : ValidationAttribute
    {
        public TimezoneAttribute()
        {
            this.ErrorMessageResourceName = "TimezoneAttribute_ErrorMessage";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        public override bool IsValid(object value)
        {
            string val;
            if (value == null || string.IsNullOrEmpty(val = value.ToString()))
                return false;

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(val);
                return tz != null;
            }
            catch (InvalidTimeZoneException)
            {
                return false;
            }
            catch (TimeZoneNotFoundException)
            {
                return false;
            }
        }
    }
}
