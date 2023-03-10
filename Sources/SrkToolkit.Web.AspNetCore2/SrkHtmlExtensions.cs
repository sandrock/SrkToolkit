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

namespace SrkToolkit.Web
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
    using Microsoft.AspNetCore.Routing;
    using SrkToolkit.AspNetCore;
    using SrkToolkit.Web;
    using SrkToolkit.Web.Models;
    using SrkToolkit.Web.Open;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using System.Globalization;

    /// <summary>
    /// HTML extensions. 
    /// </summary>
    public static class SrkHtmlExtensions
    {
        internal const string defaultDateTimeFormatsKey = "SrkDisplayDateFormat";
        internal static readonly string[] defaultDateTimeFormats = new string[]
        {
            /* 0 => */ "D", // date
            /* 1 => */ "D zzz", // date + tz
            /* 2 => */ "T", // time
            /* 3 => */ "T zzz", // time + tz
            /* 4 => */ "c", // timespan
            /* 5 => */ "F", // datetime
            /* 6 => */ "t", // short time
            /* 7 => */ "g", // short timespan
        };

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        #region SetTimezone, GetTimezone, GetUserDate, GetUtcDate, SetCulture, GetCulture

        /// <summary>
        /// Sets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.TimeZoneNotFoundException"></exception>
        public static IHtmlHelper SetTimezone(this IHtmlHelper html, string timeZoneName)
        {
            if (string.IsNullOrEmpty(timeZoneName))
                throw new ArgumentException("The value cannot be empty", "timeZoneName");

            SrkHtmlExtensions.SetTimezone(html, TimeZoneInfo.FindSystemTimeZoneById(timeZoneName));
            return html;
        }

        /// <summary>
        /// Sets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IHtmlHelper SetTimezone(this IHtmlHelper html, TimeZoneInfo timeZone)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            if (html.ViewContext != null && html.ViewContext.HttpContext != null)
                html.ViewContext.HttpContext.Items["Timezone"] = timeZone;
            html.ViewData["Timezone"] = timeZone;
            return html;
        }

        /// <summary>
        /// Gets the timezone for displays of dates and times.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">html</exception>
        public static TimeZoneInfo GetTimezone(this IHtmlHelper html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            if (html.ViewContext != null && html.ViewContext.HttpContext != null)
                return (TimeZoneInfo)html.ViewData["Timezone"] ?? (TimeZoneInfo)html.ViewContext.HttpContext.Items["Timezone"] ?? TimeZoneInfo.Utc;
            return (TimeZoneInfo)html.ViewData["Timezone"] ?? TimeZoneInfo.Utc;
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> based on the user's <see cref="TimeZoneInfo"/> specified with <see cref="SetTimezone"/>.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date.</param>
        /// <param name="utcDate">The specified <see cref="DateTime"/> in the UTC time zone.</param>
        /// <returns>The specified <see cref="DateTime"/> in the user's time zone.</returns>
        public static DateTime GetUserDate(this IHtmlHelper html, DateTime date, out DateTime utcDate)
        {
            var tz = html.GetTimezone();
            if (date.Kind == DateTimeKind.Utc)
            {
                utcDate = date;
                return tz.ConvertFromUtc(date);
            }
            else if (date.Kind == DateTimeKind.Local)
            {
                utcDate = date.ToUniversalTime();
                return tz.ConvertFromUtc(utcDate);
            }
            else if (date.Kind == DateTimeKind.Unspecified)
            {
                utcDate = tz.ConvertToUtc(date);
                return date;
            }

            utcDate = DateTime.MinValue;
            return utcDate;
        }

        /// <summary>
        /// Gets the UTC <see cref="DateTime"/> based on the user's <see cref="TimeZoneInfo"/> specified with <see cref="SetTimezone"/>.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date.</param>
        /// <returns>The specified <see cref="DateTime"/> in the UTC time zone.</returns>
        /// <exception cref="System.NotImplementedException">DateTime.Kind ' + date.Kind + ' is not supported</exception>
        public static DateTime GetUtcDate(this IHtmlHelper html, DateTime date)
        {
            if (date.Kind == DateTimeKind.Utc)
            {
                return date;
            }
            else if (date.Kind == DateTimeKind.Local)
            {
                return date.ToUniversalTime();
            }
            else if (date.Kind == DateTimeKind.Unspecified)
            {
                var tz = html.GetTimezone();
                return tz.ConvertToUtc(date);
            }

            throw new NotSupportedException("DateTime.Kind '" + date.Kind + "' is not supported");
        }

        /// <summary>
        /// Sets the current culture.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="culture">Culture name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="CultureNotFoundException"></exception>
        public static IHtmlHelper SetCulture(this IHtmlHelper html, string culture)
        {
            if (string.IsNullOrEmpty(culture))
                throw new ArgumentException("The value cannot be empty", "culture");

            SrkHtmlExtensions.SetCulture(html, CultureInfo.GetCultureInfo(culture));
            return html;
        }

        /// <summary>
        /// Sets the current culture.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static IHtmlHelper SetCulture(this IHtmlHelper html, CultureInfo culture)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            if (html.ViewContext != null && html.ViewContext.HttpContext != null)
                html.ViewContext.HttpContext.Items["Culture"] = culture;
            html.ViewData["Culture"] = culture;
            return html;
        }

        /// <summary>
        /// Gets the current culture.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">html</exception>
        public static CultureInfo GetCulture(this IHtmlHelper html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            if (html.ViewContext != null && html.ViewContext.HttpContext != null)
                return (CultureInfo)html.ViewData["Culture"] ?? (CultureInfo)html.ViewContext.HttpContext.Items["Culture"] ?? CultureInfo.CurrentUICulture;
            return (CultureInfo)html.ViewData["Culture"] ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Sets the current UI culture.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="uICulture">Culture name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="CultureNotFoundException"></exception>
        public static IHtmlHelper SetUICulture(this IHtmlHelper html, string uICulture)
        {
            if (string.IsNullOrEmpty(uICulture))
                throw new ArgumentException("The value cannot be empty", "culture");

            SrkHtmlExtensions.SetUICulture(html, CultureInfo.GetCultureInfo(uICulture));
            return html;
        }

        /// <summary>
        /// Sets the current UI culture.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="uICulture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static IHtmlHelper SetUICulture(this IHtmlHelper html, CultureInfo uICulture)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            if (html.ViewContext != null && html.ViewContext.HttpContext != null)
                html.ViewContext.HttpContext.Items["UICulture"] = uICulture;
            html.ViewData["UICulture"] = uICulture;
            return html;
        }

        /// <summary>
        /// Gets the current UI culture.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">html</exception>
        public static CultureInfo GetUICulture(this IHtmlHelper html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            if (html.ViewContext != null && html.ViewContext.HttpContext != null)
                return (CultureInfo)html.ViewData["UICulture"] ?? (CultureInfo)html.ViewContext.HttpContext.Items["UICulture"] ?? CultureInfo.CurrentUICulture;
            return (CultureInfo)html.ViewData["UICulture"] ?? CultureInfo.CurrentUICulture;
        }

        #region Display date/time
/*
        /// <summary>
        /// Sets the date and time formats.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="dateFormat">The date format for Html.DisplayDate(DateTime).</param>
        /// <param name="dateTzFormat">The date tz format for Html.DisplayDate(DateTimeOffset).</param>
        /// <param name="timeFormat">The time format for Html.DisplayTime(DateTime).</param>
        /// <param name="timeTzFormat">The time tz format for Html.DisplayDate(DateTimeOffset).</param>
        /// <param name="timespanFormat">The timespan format for Html.DisplayTime(TimeSpan).</param>
        /// <param name="dateTimeFormat">The date time format for Html.DisplayDateTime(DateTime).</param>
        /// <param name="shortTimeFormat">The short time format for Html.DisplayShortTime(DateTime).</param>
        /// <param name="shortTimespanFormat">The short timespan format for Html.DisplayShortTime(TimeSpan).</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">html
        /// or
        /// html.ViewContext
        /// or
        /// html.ViewContext.HttpContext</exception>
        public static IHtmlHelper SetDateTimeFormats(this IHtmlHelper html, string dateFormat = null, string dateTzFormat = null, string timeFormat = null, string timeTzFormat = null, string timespanFormat = null, string dateTimeFormat = null, string shortTimeFormat = null, string shortTimespanFormat = null)
        {
            if (html == null)
                throw new ArgumentNullException("html");
            if (html.ViewContext == null)
                throw new ArgumentNullException("html.ViewContext");
            if (html.ViewContext.HttpContext == null)
                throw new ArgumentNullException("html.ViewContext.HttpContext");

            html.ViewContext.HttpContext.SetDateTimeFormats(dateFormat, dateTzFormat, timeFormat, timeTzFormat, timespanFormat, dateTimeFormat);


            return html;
        }
*/
        /// <summary>
        /// Gets the date and time formats.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// html
        /// or
        /// html.ViewContext
        /// or
        /// html.ViewContext.HttpContext
        /// </exception>
        public static string[] GetDateTimeFormats(this IHtmlHelper html)
        {
            if (html == null)
                throw new ArgumentNullException("html");
            if (html.ViewContext == null)
                throw new ArgumentNullException("html.ViewContext");
            if (html.ViewContext.HttpContext == null)
                throw new ArgumentNullException("html.ViewContext.HttpContext");

            var values = (string[])html.ViewContext.HttpContext.Items[defaultDateTimeFormatsKey] ?? defaultDateTimeFormats.ToArray();

            return values;
        }

        /// <summary>
        /// Displays a date and a time.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <param name="displayDateFormat">The display date format (use to change the default display value format).</param>
        /// <returns></returns>
        public static HtmlString DisplayDateTime(this IHtmlHelper html, DateTime date, bool useTimeTag = true, string display = null, string displayDateFormat = null)
        {
            var defaultFormat = html.GetDateTimeFormats()[5];
            DateTime utc;
            DateTime userDate = html.GetUserDate(date, out utc);
            string displayTime = userDate.ToString(displayDateFormat ?? defaultFormat);
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\" class=\"{3}\">{0}</time>",
                    display,
                    utc.ToString("O"),
                    userDate.ToString("G"),
                    GetDateClasses(utc) + "display-datetime");
                return new HtmlString(tag);
            }
            else
            {
                return new HtmlString(display);
            }
        }

        private static string GetDateClasses(DateTime date)
        {
            string classes = "";
            if (date.Kind == DateTimeKind.Utc)
            {
                classes += date > DateTime.UtcNow ? "future " : "past ";
                classes += date.IsEqualTo(DateTime.UtcNow, DateTimePrecision.Day) ? "today " : "not-today ";
            }

            if (date.Kind == DateTimeKind.Local)
            {
                classes += date > DateTime.Now ? "future " : "past ";
                classes += date.IsEqualTo(DateTime.Now, DateTimePrecision.Day) ? "today " : "not-today ";
            }

            return classes;
        }

        private static string GetDateClasses(DateTimeOffset date)
        {
            return GetDateClasses(date.UtcDateTime);
        }

        /// <summary>
        /// Displays a date.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <param name="displayDateFormat">The display date format (use to change the default display value format).</param>
        /// <returns></returns>
        public static HtmlString DisplayDate(this IHtmlHelper html, DateTime date, bool useTimeTag = true, string display = null, string displayDateFormat = null)
        {
            DateTime utc;
            DateTime userDate = html.GetUserDate(date, out utc);
            var defaultFormat = html.GetDateTimeFormats()[0];
            string displayTime = userDate.ToString(displayDateFormat ?? defaultFormat);
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\" class=\"{3}\">{0}</time>",
                    display,
                    utc.ToString("O"),
                    userDate.ToString("D"),
                    GetDateClasses(utc) + "display-date");
                return new HtmlString(tag);
            }
            else
            {
                return new HtmlString(display);
            }
        }

        /// <summary>
        /// Displays a date.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <param name="displayDateFormat">The display date format (use to change the default display value format).</param>
        /// <returns></returns>
        public static HtmlString DisplayDate(this IHtmlHelper html, DateTimeOffset date, bool useTimeTag = true, string display = null, string displayDateFormat = null)
        {
            var defaultFormat = html.GetDateTimeFormats()[1];
            string displayTime = date.ToString(displayDateFormat ?? defaultFormat);
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\" class=\"{3}\">{0}</time>",
                    display,
                    date.ToUniversalTime().ToString("O"),
                    date.ToString("D zzz"),
                    GetDateClasses(date) + "display-date");
                return new HtmlString(tag);
            }
            else
            {
                return new HtmlString(display);
            }
        }

        /// <summary>
        /// Displays a time.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <param name="displayDateFormat">The display date format (use to change the default display value format).</param>
        /// <returns></returns>
        public static HtmlString DisplayTime(this IHtmlHelper html, DateTime date, bool useTimeTag = true, string display = null, string displayDateFormat = null)
        {
            DateTime utc;
            DateTime userDate = html.GetUserDate(date, out utc);
            var defaultFormat = html.GetDateTimeFormats()[2];
            string displayTime = userDate.ToString(displayDateFormat ?? defaultFormat);
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\" class=\"{3}\">{0}</time>",
                    display,
                    utc.ToString("O"),
                    userDate.ToString("G"),
                    GetDateClasses(utc) + "display-time");
                return new HtmlString(tag);
            }
            else
            {
                return new HtmlString(display);
            }
        }

        /// <summary>
        /// Displays a time.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <param name="displayDateFormat">The display date format (use to change the default display value format).</param>
        /// <returns></returns>
        public static HtmlString DisplayTime(this IHtmlHelper html, DateTimeOffset date, bool useTimeTag = true, string display = null, string displayDateFormat = null)
        {
            var defaultFormat = html.GetDateTimeFormats()[3];
            string displayTime = date.ToLocalTime().ToString(displayDateFormat ?? defaultFormat);
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\" class=\"{3}\">{0}</time>",
                    display,
                    date.ToUniversalTime().ToString("O"),
                    date.ToString("R"),
                    GetDateClasses(date) + "display-time");
                return new HtmlString(tag);
            }
            else
            {
                return new HtmlString(display);
            }
        }

        /// <summary>
        /// Displays a time.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <param name="displayDateFormat">The display date format (use to change the default display value format).</param>
        /// <returns></returns>
        public static HtmlString DisplayTime(this IHtmlHelper html, TimeSpan date, bool useTimeTag = true, string display = null, string displayDateFormat = null)
        {
            var defaultFormat = html.GetDateTimeFormats()[4];
            string displayTime = date.ToString(displayDateFormat ?? defaultFormat);
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\" class=\"{3}\">{0}</time>",
                    display,
                    date.ToString("c"),
                    date.ToString("c"),
                    "display-time");
                return new HtmlString(tag);
            }
            else
            {
                return new HtmlString(display);
            }
        }

        /// <summary>
        /// Returns the date (to UTC) in JavaScript format like 'new Date(123456789000)'.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date.</param>
        /// <param name="precision">The precision.</param>
        /// <returns></returns>
        public static HtmlString JsDate(this IHtmlHelper html, DateTime date, DateTimePrecision precision = DateTimePrecision.Second)
        {
            DateTime utc = SrkHtmlExtensions.GetUtcDate(html, date);
            /* https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date
             *   new Date(value); 
             *     value: Integer value representing the number of milliseconds since 1 January 1970 00:00:00 UTC (Unix Epoch)
             * 
             * I use this constructor because it accepts a UTC date.
             * The other constructors use a local date so we can't rely on them.
             * 
             * ToString("F0") is important to avoid the exponential notation.
             */
            string value = "new Date("
                + (utc.ToPrecision(precision).Subtract(UnixEpoch).TotalMilliseconds).ToString("F0")
                + ")";
            return new HtmlString(value);
        }

        /// <summary>
        /// Displays a time.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <returns></returns>
        public static HtmlString DisplayShortTime(this IHtmlHelper html, DateTime date, bool useTimeTag = true, string display = null)
        {
            var defaultFormat = html.GetDateTimeFormats()[6];
            return html.DisplayTime(date, useTimeTag, display, displayDateFormat: defaultFormat);
        }

        /// <summary>
        /// Displays a time.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <returns></returns>
        public static HtmlString DisplayShortTime(this IHtmlHelper html, TimeSpan date, bool useTimeTag = true, string display = null)
        {
            var defaultFormat = html.GetDateTimeFormats()[7];
            return html.DisplayTime(date, useTimeTag, display, displayDateFormat: defaultFormat);
        }

        #endregion

        #region Display text

        /// <summary>
        /// Replaces new lines by line breaks (&lt;br /&gt;)
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="content">The content.</param>
        /// <param name="makeLinks">if set to <c>true</c> make links from URLs and email addresses.</param>
        /// <param name="linkClasses">The class attribute to associate to &lt;a&gt; tags (defaults to "external").</param>
        /// <param name="linkTarget">The target attribute to associate to &lt;a&gt; tags (default to "_blank").</param>
        /// <returns></returns>
        public static HtmlString LineBreaks(this IHtmlHelper html, string content, bool makeLinks = false, string linkClasses = "external accentColor", string linkTarget = "_self")
        {
            if (content == null)
                return new HtmlString(string.Empty);

            content = content.ProperHtmlEscape();

            if (makeLinks)
            {
                content = content.LinksAsHtml(linkClasses: linkClasses, linkTarget: linkTarget);
            }

            content = content.AddHtmlLineBreaks();

            return new HtmlString(content);
        }

        /// <summary>
        /// Trims the text from the right, leaving the specified number of characters.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="text">The text.</param>
        /// <param name="chars">The number of characters desired.</param>
        /// <param name="ending">The ending string (used only if trim occured).</param>
        /// <returns></returns>
        public static string TrimText(this IHtmlHelper html, string text, int chars, string ending = "...")
        {
            return text.TrimTextRight(chars, ending);
        }

        /// <summary>
        /// Formats a user string as HTML;
        /// Replaces newlines with line breaks.
        /// Replaces URLs with links.
        /// Escapes HTML chars.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="content">the content to transform</param>
        /// <param name="makeLinks">if set to <c>true</c> make links from URLs and email addresses.</param>
        /// <param name="makeParagraphs">if set to <c>true</c> make paragraphs from groups of text (separated by multiple lines).</param>
        /// <param name="makeLineBreaks">if set to <c>true</c> make line breaks from new lines.</param>
        /// <param name="twitterLinks">if set to <c>true</c> make twitter links from @mentions and #hashes.</param>
        /// <param name="linksClass">The class attribute to associate to &lt;a&gt; tags (defaults to "external").</param>
        /// <param name="linksTarget">The target attribute to associate to &lt;a&gt; tags (default to "_blank").</param>
        /// <param name="wrapClass">Indicates wether to wrap the HTML into a div element.</param>
        /// <returns>
        /// an escaped HTML string
        /// </returns>
        public static HtmlString DisplayText(this IHtmlHelper html, string content, bool makeLinks = true, bool makeParagraphs = true, bool makeLineBreaks = true, bool twitterLinks = false, string linksClass = "external", string linksTarget = "_blank", string wrapClass = null)
        {
            if (content == null)
                return new HtmlString(string.Empty);

            content = content.ProperHtmlEscape();

            if (makeLinks)
                content = content.LinksAsHtml(linkClasses: linksClass, linkTarget: linksTarget, avoidDoubleEscape: true);

            if (twitterLinks)
                content = content.TwitterLinksAsHtml(linkClasses: linksClass + " twitter", linkTarget: linksTarget);

            if (makeParagraphs)
                content = content.HtmlParagraphizify(makeLineBreaks: makeLineBreaks);
            else if (makeLineBreaks)
                content = content.AddHtmlLineBreaks();

            if (wrapClass == null)
                return new HtmlString(content);
            else
                return new HtmlString("<div class=\"" + wrapClass.ProperHtmlAttributeEscape() + "\">" + content + "</div>");
        }

        #endregion

        #endregion
/*
        /// <summary>
        /// Returns many HTML radio buttons for the specified list and selected value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The model expression.</param>
        /// <param name="listOfValues">The list of values.</param>
        /// <returns></returns>
        public static HtmlString RadioButtonSelectList<TModel, TProperty>(
            this IHtmlHelper<TModel> IHtmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, IHtmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                // Create a radio button for each item in the list
                foreach (SelectListItem item in listOfValues)
                {
                    // Generate an id to be given to the radio button field
                    ////var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);
                    var id = metaData.PropertyName + "_" + item.Value;

                    // Create and populate a radio button using the existing html helpers
                    var label = IHtmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
                    var radio = IHtmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();

                    // Create the html string that will be returned to the client
                    // e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" /><label for="TestRadio_1">Line1</label>
                    sb.AppendFormat("<div class=\"RadioButton\">{0}{1}</div>", radio, label);
                }
            }

            return new HtmlString(sb.ToString());
        }
*/
        #region GetFullHtmlFieldName

        public static string GetFullHtmlFieldName<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return helper.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
        }
/*
        public static string GetFullHtmlFieldId<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return helper.ViewData.TemplateInfo.GetFullHtmlFieldId(expression);
        }
*/
        public static string GetFullHtmlFieldName<TModel, TProperty>(this TemplateInfo templateInfo, Expression<Func<TModel, TProperty>> expression)
        {
            return templateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }
/*
        public static string GetFullHtmlFieldId<TModel, TProperty>(this TemplateInfo templateInfo, Expression<Func<TModel, TProperty>> expression)
        {
            return templateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
        }
*/
        public static string GetFullHtmlFieldDisplayName<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var propertyName = helper.GetFullHtmlFieldName(expression);

            return helper.ViewData.ModelMetadata.Properties.Single(p => p.PropertyName == propertyName).DisplayName;
        }

        #endregion

        #region DescriptionFor

/*
        /// <summary>
        /// Returns an HTML span element and the property name of the property that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the property to display.</param>
        /// <returns>An HTML span element and the property name of the property that is represented by the specified expression</returns>
        public static HtmlString DescriptionFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return DescriptionFor(helper, expression, null, null);
        }
        /// <summary>
        /// Returns an HTML span element and the property name of the property that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the property to display.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>
        /// An HTML span element and the property name of the property that is represented by the specified expression
        /// </returns>
        public static HtmlString DescriptionFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            return DescriptionFor(helper, expression, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Returns an HTML span element and the property name of the property that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the property to display.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>
        /// An HTML span element and the property name of the property that is represented by the specified expression
        /// </returns>
        public static HtmlString DescriptionFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IDictionary<string, object> htmlAttributes)
        {
            return DescriptionFor(helper, expression, null, htmlAttributes);
        }
*//*
        /// <summary>
        /// Returns an HTML span element and the property name of the property that is represented by the specified expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the property to display.</param>
        /// <param name="descriptionText">The description text to display.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>
        /// An HTML span element and the property name of the property that is represented by the specified expression
        /// </returns>
        public static HtmlString DescriptionFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string descriptionText,
            IDictionary<string, object> htmlAttributes)
        {
            var meta = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var value = descriptionText.NullIfEmpty() ?? meta.Description.NullIfEmpty();

            if (string.IsNullOrEmpty(value))
                return HtmlString.Empty;

            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var id = TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName));
            var tag = new TagBuilder("span");
            tag.Attributes.Add("data-for", id);
            tag.MergeAttributes<string, object>(htmlAttributes, true);
            ////tag.SetInnerText(value); // AspNet
            tag.InnerHtml.Append(value); // AspNetCore?
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }
*/
        #endregion

        #region BeginFormEx
/*
        /// <summary>
        /// Writes an opening &lt;form&gt; tag to the response. When the user submits the form, the request will be processed by an action method.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An opening &lt;form&gt; tag.</returns>
        public static MvcForm BeginFormEx(this IHtmlHelper IHtmlHelper, object htmlAttributes)
        {
            string rawUrl = IHtmlHelper.ViewContext.HttpContext.Request.RawUrl;
            return IHtmlHelper.BeginForm(null, null, IHtmlHelper.ViewContext.RouteData.Values, FormMethod.Post, new RouteValueDictionary(htmlAttributes) as IDictionary<string, object>);
        }

        /// <summary>
        /// Writes an opening &lt;form&gt; tag to the response. When the user submits the form, the request will be processed by an action method.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. This object is typically created by using object initializer syntax.</param>
        /// <returns>An opening &lt;form&gt; tag.</returns>
        public static MvcForm BeginFormEx(this IHtmlHelper IHtmlHelper, object htmlAttributes, object routeValues)
        {
            string rawUrl = IHtmlHelper.ViewContext.HttpContext.Request.RawUrl;
            return IHtmlHelper.BeginForm(null, null, routeValues, FormMethod.Post, htmlAttributes);
        }
*/
        #endregion

        #region Submit
/*
        /// <summary>
        /// Returns a submit input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="value">The value.</param>
        /// <returns>An input element whose type attribute is set to "submit".</returns>
        public static HtmlString Submit(this IHtmlHelper html, string value)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "submit");
            builder.MergeAttribute("value", value);
            return new HtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Returns a submit input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <returns>An input element whose type attribute is set to "submit".</returns>
        public static HtmlString Submit(this IHtmlHelper html, string value, string name)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "submit");
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("value", value);
            return new HtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Returns a submit input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An input element whose type attribute is set to "submit".</returns>
        public static HtmlString Submit(this IHtmlHelper html, string value, string name, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var builder = new TagBuilder("input");
            builder.MergeAttributes<string, object>(attributes);
            builder.MergeAttribute("type", "submit");
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("value", value);
            return new HtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }
*/
        #endregion

        #region File
/*
        /// <summary>
        /// Returns a file input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="name">The name.</param>
        /// <returns>An input element whose type attribute is set to "file".</returns>
        public static HtmlString File(this IHtmlHelper html, string name)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "file");
            builder.MergeAttribute("name", name);
            return new HtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Returns a file input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="name">The name.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An input element whose type attribute is set to "file".</returns>
        public static HtmlString File(this IHtmlHelper html, string name, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var builder = new TagBuilder("input");
            builder.MergeAttributes<string, object>(attributes);
            builder.MergeAttribute("type", "file");
            builder.MergeAttribute("name", name);
            return new HtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }
*/
        #endregion

        #region OpenGraph
/*
        /// <summary>
        /// Gets the opengraph object associated with the current HTTP request.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static SrkOpenGraphHtmlExtensions OpenGraph(this IHtmlHelper html)
        {
            return new SrkOpenGraphHtmlExtensions(html);
        }
*/
        #endregion

        #region ActionLink
/*
        /// <summary>
        /// Returns an anchor element (a element) that contains the virtual path of the specified action.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName"></param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <returns></returns>
        public static HtmlString ActionLink(this IHtmlHelper IHtmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes = null, string fragment = null, string controllerName = null, string protocol = null, string hostName = null)
        {
            return IHtmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }
*/
        #endregion

        /// <summary>
        /// Helps write a CSS class base on a condition.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="condition">A display condition.</param>
        /// <param name="classTrue">The class to display if the condition is true.</param>
        /// <param name="classFalse">The class to display if the condition is false.</param>
        /// <returns>The CSS class corresponding to the condition</returns>
        public static string CssClass(this IHtmlHelper html, bool condition, string classTrue, string classFalse = null)
        {
            return condition ? classTrue : classFalse;
        }

        /// <summary>
        /// Helps write a CSS class base on a condition.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="condition">A display condition.</param>
        /// <param name="classTrue">The class to display if the condition is true.</param>
        /// <param name="classFalse">The class to display if the condition is false.</param>
        /// <param name="classNull">The class to display if the condition is null.</param>
        /// <returns>The CSS class corresponding to the condition</returns>
        public static string CssClass(this IHtmlHelper html, bool? condition, string classTrue, string classFalse = null, string classNull = null)
        {
            if (condition != null)
                return condition.Value ? classTrue : classFalse;
            return classNull;
        }

        /// <summary>
        /// Determines whether the model state contains other validation errors.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">html</exception>
        public static bool HasOtherValidationErrors(this IHtmlHelper html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            if (html.ViewData.ModelState.IsValid)
                return false;

            if (!html.ViewData.ModelState.ContainsKey(string.Empty))
                return false;

            if (html.ViewData.ModelState[string.Empty] == null)
                return true;

            if (html.ViewData.ModelState[string.Empty].Errors == null)
                return true;

            return html.ViewData.ModelState[string.Empty].Errors.Count > 0;
        }
/*
        /// <summary>
        /// Enhancement of <see cref="System.Web.Mvc.Html.ValidationExtensions.ValidationSummary"/> that shows no HTML when there are no errors to display.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        public static HtmlString ValidationSummaryEx(this IHtmlHelper html)
        {
            if (SrkHtmlExtensions.HasOtherValidationErrors(html))
            {
                return html.ValidationSummary(true);
            }

            return null;
        }
*/
        /// <summary>
        /// Gets the <see cref="NavigationLine"/> associated to the request.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static NavigationLine NavigationLine(this IHtmlHelper html)
        {
            if (html.ViewContext == null)
                throw new ArgumentNullException("ViewContext is not set", "ctrl");

            if (html.ViewContext.HttpContext == null)
                throw new ArgumentNullException("HttpContext is not set", "ctrl");

            var line = html.ViewContext.HttpContext.Items[SrkControllerExtensions.NavigationLineKey] as NavigationLine;
            if (line == null)
            {
                line = new NavigationLine();
                html.ViewContext.HttpContext.Items[SrkControllerExtensions.NavigationLineKey] = line;
            }

            return line;
        }
/*
        /// <summary>
        /// Returns an anchor element (a element) that contains a phone call url.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        public static HtmlString CallLink(this IHtmlHelper html, string phoneNumber)
        {
            return SrkHtmlExtensions.CallLink(html, phoneNumber, new { @class = "tel", });
        }
*//*
        /// <summary>
        /// Returns an anchor element (a element) that contains a phone call url.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="attributes">An object that contains the HTML attributes for the element.</param>
        /// <returns></returns>
        public static HtmlString CallLink(this IHtmlHelper html, string phoneNumber, object attributes)
        {
            var tag = new TagBuilder("a");
            tag.Attributes.Add("href", "tel:" + phoneNumber);
            var attrCollection = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            tag.MergeAttributes<string, object>(attrCollection, true);
            ////tag.SetInnerText(phoneNumber); // AspNet
            tag.InnerHtml.Append(phoneNumber); // AspNetCore?
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }
*/
        /// <summary>
        /// Helps attach descriptors to a page in order to generate meta/link tags.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>The <see cref="PageInfo"/> for the current request.</returns>
        /// <exception cref="System.ArgumentNullException">html or html.ViewContext or html.ViewContext.HttpContext</exception>
        public static PageInfo GetPageInfo(this IHtmlHelper html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            if (html.ViewContext == null)
                throw new ArgumentNullException("html.ViewContext");

            if (html.ViewContext.HttpContext == null)
                throw new ArgumentNullException("html.ViewContext.HttpContext");

            var httpContext = html.ViewContext.HttpContext;
            var item = httpContext.Items[SrkControllerExtensions.PageInfoKey] as PageInfo;
            if (item == null)
            {
                item = new PageInfo();
                httpContext.Items[SrkControllerExtensions.PageInfoKey] = item;
            }

            return item;
        }
    }
}
