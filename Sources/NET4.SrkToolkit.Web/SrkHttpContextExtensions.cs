
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Extension methods for the <see cref="HttpContext"/> and <see cref="HttpContextBase"/> classes.
    /// </summary>
    public static class SrkHttpContextExtensions
    {
        /// <summary>
        /// Fast access to HttpContext.User.Identity.Name.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">httpContext</exception>
        public static string GetUserIdentityName(this HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            return httpContext.User != null && httpContext.User.Identity != null ? httpContext.User.Identity.Name.NullIfEmptyOrWhitespace() : null;
        }

        /// <summary>
        /// Fast access to HttpContext.User.Identity.Name.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">httpContext</exception>
        public static string GetUserIdentityName(this HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            return httpContext.User != null && httpContext.User.Identity != null ? httpContext.User.Identity.Name.NullIfEmptyOrWhitespace() : null;
        }

        /// <summary>
        /// Sets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="http">The HTML.</param>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.TimeZoneNotFoundException"></exception>
        public static HttpContextBase SetTimezone(this HttpContextBase http, string timeZoneName)
        {
            if (string.IsNullOrEmpty(timeZoneName))
                throw new ArgumentException("The value cannot be empty", "timeZoneName");

            SrkHttpContextExtensions.SetTimezone(http, TimeZoneInfo.FindSystemTimeZoneById(timeZoneName));
            return http;
        }

        /// <summary>
        /// Sets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="http">The HTML.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static HttpContextBase SetTimezone(this HttpContextBase http, TimeZoneInfo timeZone)
        {
            if (http == null)
                throw new ArgumentNullException("http");

            http.Items["Timezone"] = timeZone;
            return http;
        }

        /// <summary>
        /// Sets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="http">The HTML.</param>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.TimeZoneNotFoundException"></exception>
        public static HttpContext SetTimezone(this HttpContext http, string timeZoneName)
        {
            if (string.IsNullOrEmpty(timeZoneName))
                throw new ArgumentException("The value cannot be empty", "timeZoneName");

            SrkHttpContextExtensions.SetTimezone(http, TimeZoneInfo.FindSystemTimeZoneById(timeZoneName));
            return http;
        }

        /// <summary>
        /// Sets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="http">The HTML.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static HttpContext SetTimezone(this HttpContext http, TimeZoneInfo timeZone)
        {
            if (http == null)
                throw new ArgumentNullException("http");

            http.Items["Timezone"] = timeZone;
            return http;
        }

        /// <summary>
        /// Gets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="http">The HTML.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">html</exception>
        public static TimeZoneInfo GetTimezone(this HttpContextBase http)
        {
            if (http == null)
                throw new ArgumentNullException("http");

            return (TimeZoneInfo)http.Items["Timezone"] ?? TimeZoneInfo.Utc;
        }

        /// <summary>
        /// Gets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="http">The HTML.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">html</exception>
        public static TimeZoneInfo GetTimezone(this HttpContext http)
        {
            if (http == null)
                throw new ArgumentNullException("http");

            return (TimeZoneInfo)http.Items["Timezone"] ?? TimeZoneInfo.Utc;
        }

        /// <summary>
        /// Sets the date and time formats.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="dateFormat">The date format for Html.DisplayDate(DateTime).</param>
        /// <param name="dateTzFormat">The date tz format for Html.DisplayDate(DateTimeOffset).</param>
        /// <param name="timeFormat">The time format for Html.DisplayTime(DateTime).</param>
        /// <param name="timeTzFormat">The time tz format for Html.DisplayDate(DateTimeOffset).</param>
        /// <param name="timespanFormat">The timespan format for Html.DisplayTime(TimeSpan).</param>
        /// <param name="dateTimeFormat">The date time format for Html.DisplayDateTime(DateTime).</param>
        /// <param name="shortTimeFormat">The short time format for Html.DisplayShortTime(DateTime).</param>
        /// <param name="shortTimespanFormat">The short timespan format for Html.DisplayShortTime(TimeSpan).</param>
        /// <exception cref="System.ArgumentNullException">httpContext</exception>
        public static void SetDateTimeFormats(this HttpContextBase httpContext, string dateFormat = null, string dateTzFormat = null, string timeFormat = null, string timeTzFormat = null, string timespanFormat = null, string dateTimeFormat = null, string shortTimeFormat = null, string shortTimespanFormat = null)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var now = DateTime.UtcNow;
            var values = (string[])httpContext.Items[System.Web.Mvc.SrkHtmlExtensions.defaultDateTimeFormatsKey]
                ?? System.Web.Mvc.SrkHtmlExtensions.defaultDateTimeFormats.ToArray();

            if (!string.IsNullOrEmpty(dateFormat))
            {
                now.ToString(dateFormat); // crash now for incorrect format
                values[0] = dateFormat;
            }

            if (!string.IsNullOrEmpty(dateTzFormat))
            {
                now.ToString(dateTzFormat); // crash now for incorrect format
                values[1] = dateTzFormat;
            }

            if (!string.IsNullOrEmpty(timeFormat))
            {
                now.ToString(timeFormat); // crash now for incorrect format
                values[2] = timeFormat;
            }

            if (!string.IsNullOrEmpty(timeTzFormat))
            {
                now.ToString(timeTzFormat); // crash now for incorrect format
                values[3] = timeTzFormat;
            }

            if (!string.IsNullOrEmpty(timespanFormat))
            {
                TimeSpan.FromMinutes(2D).ToString(timespanFormat); // crash now for incorrect format
                values[4] = timespanFormat;
            }

            if (!string.IsNullOrEmpty(dateTimeFormat))
            {
                now.ToString(dateTimeFormat); // crash now for incorrect format
                values[5] = dateTimeFormat;
            }

            if (!string.IsNullOrEmpty(shortTimeFormat))
            {
                now.ToString(shortTimeFormat); // crash now for incorrect format
                values[6] = shortTimeFormat;
            }

            if (!string.IsNullOrEmpty(shortTimespanFormat))
            {
                TimeSpan.FromMinutes(2D).ToString(shortTimespanFormat); // crash now for incorrect format
                values[7] = shortTimespanFormat;
            }

            httpContext.Items[System.Web.Mvc.SrkHtmlExtensions.defaultDateTimeFormatsKey] = values;
        }

        /// <summary>
        /// Gets the date and time formats.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">httpContext</exception>
        public static string[] GetDateTimeFormats(this HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var values = (string[])httpContext.Items[System.Web.Mvc.SrkHtmlExtensions.defaultDateTimeFormatsKey]
                ?? System.Web.Mvc.SrkHtmlExtensions.defaultDateTimeFormats.ToArray();

            return values;
        }
    }
}
