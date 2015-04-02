
namespace SrkToolkit.DataAnnotations
{
    using SrkToolkit.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

    [AttributeUsage(AttributeTargets.Property)]
    public class TwitterUsernameAttribute : RegularExpressionAttribute
    {
        public const string TwitterRegex = @"^(?:(?:https?:\/\/)?(?:www\.)?twitter(?:\.com)?\/)?@?([a-zA-Z0-9_]{1,15})$";

        public TwitterUsernameAttribute()
            : base(TwitterRegex)
        {
            this.ErrorMessageResourceName = "TwitterUsernameAttribute_ErrorMessage";
            this.ErrorMessageResourceType = typeof(Strings);
        }

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
