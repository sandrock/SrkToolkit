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
    using SrkToolkit.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.RegularExpressions;

    // TODO: support for netstandard

    /// <summary>
    /// Validates a twitter username with some tolerance. Use the static method to clean the username from extra characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TwitterUsernameAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// The regew that validates a twitter username with some tolerance for known formats.
        /// </summary>
        public const string TwitterRegex = @"^(?:(?:https?:\/\/)?(?:www\.)?twitter(?:\.com)?\/)?@?([a-zA-Z0-9_]{1,15})$";

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterUsernameAttribute"/> class.
        /// </summary>
        public TwitterUsernameAttribute()
            : base(TwitterRegex)
        {
            this.ErrorMessageResourceName = "TwitterUsernameAttribute_ErrorMessage";
            this.ErrorMessageResourceType = typeof(Strings);
        }

        /// <summary>
        /// Gets a cleaned username from many known username patterns.
        /// </summary>
        /// <param name="value">some kind of twitter username</param>
        /// <param name="username">a cleaned username</param>
        /// <returns>true if the value is a valid twitter username; otherwise, false</returns>
        public static bool GetUsername(string value, out string username)
        {
            username = null;

            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("The value cannot be null", "value");

            Match match = null;
            if ((match = Regex.Match(value, TwitterRegex)) != null
                && match.Success
                && match.Groups.Count > 1
                && match.Groups[1].Success)
            {
                username = match.Groups[1].Value;
                return true;
            }

            return false;
        }
    }
}
