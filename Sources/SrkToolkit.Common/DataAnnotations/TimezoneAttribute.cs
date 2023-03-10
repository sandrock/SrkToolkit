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

#if NETFRAMEWORK

namespace SrkToolkit.DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using SrkToolkit.Resources;

    // TODO: support for netstandard

    /// <summary>
    /// Validates the field is a valid <see cref="TimeZoneInfo"/> ID.
    /// </summary>
    public class TimezoneAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneAttribute"/> class.
        /// </summary>
        public TimezoneAttribute()
        {
            this.ErrorMessageResourceName = "TimezoneAttribute_ErrorMessage";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        /// <summary>
        /// Determines whether the specified value is a valid time zone identifier.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// true if the specified value is a valid time zone identifier; otherwise, false.
        /// </returns>
        public override bool IsValid(object value)
        {
            string val;
            if (value == null || string.IsNullOrEmpty(val = value.ToString()))
            {
                return true;
            }

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
#endif
