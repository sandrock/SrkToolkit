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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using SrkToolkit.Resources;

    // TODO: support for netstandard

    /// <summary>
    /// Validates the field is a valid <see cref="CultureInfo"/> name.
    /// </summary>
    public class CultureInfoAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CultureInfoAttribute"/> class.
        /// </summary>
        public CultureInfoAttribute()
        {
            this.ErrorMessageResourceName = "CultureInfoAttribute_ErrorMessage";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow neutral culture.
        /// </summary>
        public bool AllowNeutralCulture { get; set; }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false.
        /// </returns>
        public override bool IsValid(object value)
        {
            string val;
            if (value == null || string.IsNullOrEmpty(val = value.ToString()))
                return true;

            try
            {
                var culture = new CultureInfo(val);

                if (culture == null || culture.Equals(CultureInfo.InvariantCulture))
                    return false;

                if (this.AllowNeutralCulture)
                {
                    return true;
                }
                else
                {
                    return !culture.IsNeutralCulture;
                }
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
