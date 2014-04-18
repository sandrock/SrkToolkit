// -----------------------------------------------------------------------
// <copyright file="SrkHtmlExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Linq.Expressions;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using SrkToolkit.Web.Open;

    /// <summary>
    /// HTML extensions. 
    /// </summary>
    public static class SrkHtmlExtensions
    {
        public static HtmlHelper SetTimezone(this HtmlHelper html, string timeZoneName)
        {
            SrkHtmlExtensions.SetTimezone(html, TimeZoneInfo.FindSystemTimeZoneById(timeZoneName));
            return html;
        }

        public static HtmlHelper SetTimezone(this HtmlHelper html, TimeZoneInfo timeZone)
        {
            html.ViewData["Timezone"] = timeZone;
            return html;
        }

        public static TimeZoneInfo GetTimezone(this HtmlHelper html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            return (TimeZoneInfo)html.ViewData["Timezone"] ?? TimeZoneInfo.Utc;
        }

        public static DateTime GetUserDate(this HtmlHelper html, DateTime date, out DateTime utcDate)
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

        public static DateTime GetUtcDate(this HtmlHelper html, DateTime date)
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

            throw new NotImplementedException("DateTime.Kind '" + date.Kind + "' is not supported");
        }

        #region Display date/time

        /// <summary>
        /// Displays a date and a time.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date value.</param>
        /// <param name="useTimeTag">if set to <c>true</c> the date will be enclsoed in a &lt;time&gt; tag.</param>
        /// <param name="display">The display value (use to manualy set the display value).</param>
        /// <param name="displayDateFormat">The display date format (use to change the default display value format).</param>
        /// <returns></returns>
        public static MvcHtmlString DisplayDateTime(this HtmlHelper html, DateTime date, bool useTimeTag = true, string display = null, string displayDateFormat = "F")
        {
            DateTime utc;
            DateTime userDate = html.GetUserDate(date, out utc);
            string displayTime = userDate.ToString(displayDateFormat ?? "F");
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
                return MvcHtmlString.Create(tag);
            }
            else
            {
                return MvcHtmlString.Create(display);
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
        public static MvcHtmlString DisplayDate(this HtmlHelper html, DateTime date, bool useTimeTag = true, string display = null, string displayDateFormat = "D")
        {
            DateTime utc;
            DateTime userDate = html.GetUserDate(date, out utc);
            string displayTime = userDate.ToString(displayDateFormat ?? "D");
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
                return MvcHtmlString.Create(tag);
            }
            else
            {
                return MvcHtmlString.Create(display);
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
        public static MvcHtmlString DisplayDate(this HtmlHelper html, DateTimeOffset date, bool useTimeTag = true, string display = null, string displayDateFormat = "D zzz")
        {
            string displayTime = date.ToString(displayDateFormat ?? "D zzz");
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
                return MvcHtmlString.Create(tag);
            }
            else
            {
                return MvcHtmlString.Create(display);
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
        public static MvcHtmlString DisplayTime(this HtmlHelper html, DateTime date, bool useTimeTag = true, string display = null, string displayDateFormat = "T")
        {
            DateTime utc;
            DateTime userDate = html.GetUserDate(date, out utc);
            string displayTime = userDate.ToString(displayDateFormat ?? "T");
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
                return MvcHtmlString.Create(tag);
            }
            else
            {
                return MvcHtmlString.Create(display);
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
        public static MvcHtmlString DisplayTime(this HtmlHelper html, DateTimeOffset date, bool useTimeTag = true, string display = null, string displayDateFormat = "T zzz")
        {
            string displayTime = date.ToLocalTime().ToString(displayDateFormat ?? "T zzz");
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
                return MvcHtmlString.Create(tag);
            }
            else
            {
                return MvcHtmlString.Create(display);
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
        public static MvcHtmlString DisplayTime(this HtmlHelper html, TimeSpan date, bool useTimeTag = true, string display = null, string displayDateFormat = "c")
        {
            string displayTime = date.ToString(displayDateFormat ?? "c");
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
                return MvcHtmlString.Create(tag);
            }
            else
            {
                return MvcHtmlString.Create(display);
            }
        }

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Returns the date (to UTC) in JavaScript format like 'new Date(123456789000)'.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="date">The date.</param>
        /// <param name="precision">The precision.</param>
        /// <returns></returns>
        public static MvcHtmlString JsDate(this HtmlHelper html, DateTime date, DateTimePrecision precision = DateTimePrecision.Second)
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
            return MvcHtmlString.Create(value);
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
        public static MvcHtmlString DisplayShortTime(this HtmlHelper html, DateTime date, bool useTimeTag = true, string display = null)
        {
            return html.DisplayTime(date, useTimeTag, display, displayDateFormat: "t");
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
        public static MvcHtmlString DisplayShortTime(this HtmlHelper html, TimeSpan date, bool useTimeTag = true, string display = null)
        {
            return html.DisplayTime(date, useTimeTag, display, displayDateFormat: "t");
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
        public static MvcHtmlString LineBreaks(this HtmlHelper html, string content, bool makeLinks = false, string linkClasses = "external accentColor", string linkTarget = "_self")
        {
            if (content == null)
                return MvcHtmlString.Create(string.Empty);

            content = content.ProperHtmlEscape();

            if (makeLinks)
            {
                content = content.LinksAsHtml(linkClasses: linkClasses, linkTarget: linkTarget);
            }

            content = content.AddHtmlLineBreaks();

            return MvcHtmlString.Create(content);
        }

        /// <summary>
        /// Trims the text from the right, leaving the specified number of characters.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="text">The text.</param>
        /// <param name="chars">The number of characters desired.</param>
        /// <param name="ending">The ending string (used only if trim occured).</param>
        /// <returns></returns>
        public static string TrimText(this HtmlHelper html, string text, int chars, string ending = "...")
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
        /// <returns>
        /// an escaped HTML string
        /// </returns>
        public static MvcHtmlString DisplayText(this HtmlHelper html, string content, bool makeLinks = true, bool makeParagraphs = true, bool makeLineBreaks = true, bool twitterLinks = false, string linksClass = "external", string linksTarget = "_blank")
        {
            if (content == null)
                return MvcHtmlString.Create(string.Empty);

            content = content.ProperHtmlEscape();

            if (makeLinks)
                content = content.LinksAsHtml(linkClasses: linksClass, linkTarget: linksTarget, avoidDoubleEscape: true);

            if (twitterLinks)
                content = content.TwitterLinksAsHtml(linkClasses: linksClass + " twitter", linkTarget: linksTarget);

            if (makeParagraphs)
                content = content.HtmlParagraphizify(makeLineBreaks: makeLineBreaks);
            else if (makeLineBreaks)
                content = content.AddHtmlLineBreaks();

            return MvcHtmlString.Create(content);
        }

        #endregion

        #region Forms

        /// <summary>
        /// Returns many HTML radio buttons for the specified list and selected value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The model expression.</param>
        /// <param name="listOfValues">The list of values.</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
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
                    var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();

                    // Create the html string that will be returned to the client
                    // e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" /><label for="TestRadio_1">Line1</label>
                    sb.AppendFormat("<div class=\"RadioButton\">{0}{1}</div>", radio, label);
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static string GetFullHtmlFieldName<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return helper.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
        }

        public static string GetFullHtmlFieldId<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return helper.ViewData.TemplateInfo.GetFullHtmlFieldId(expression);
        }

        public static string GetFullHtmlFieldName<TModel, TProperty>(this TemplateInfo templateInfo, Expression<Func<TModel, TProperty>> expression)
        {
            return templateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        public static string GetFullHtmlFieldId<TModel, TProperty>(this TemplateInfo templateInfo, Expression<Func<TModel, TProperty>> expression)
        {
            return templateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
        }

        public static string GetFullHtmlFieldDisplayName<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var propertyName = helper.GetFullHtmlFieldName(expression);

            return helper.ViewData.ModelMetadata.Properties.Single(p => p.PropertyName == propertyName).DisplayName;
        }

        public static string DescriptionFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var meta = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            return meta.Description;
        }

        /// <summary>
        /// Writes an opening &lt;form&gt; tag to the response. When the user submits the form, the request will be processed by an action method.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An opening &lt;form&gt; tag.</returns>
        public static MvcForm BeginFormEx(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            string rawUrl = htmlHelper.ViewContext.HttpContext.Request.RawUrl;
            return htmlHelper.BeginForm(null, null, htmlHelper.ViewContext.RouteData.Values, FormMethod.Post, new RouteValueDictionary(htmlAttributes) as IDictionary<string, object>);
        }

        /// <summary>
        /// Writes an opening &lt;form&gt; tag to the response. When the user submits the form, the request will be processed by an action method.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. This object is typically created by using object initializer syntax.</param>
        /// <returns>An opening &lt;form&gt; tag.</returns>
        public static MvcForm BeginFormEx(this HtmlHelper htmlHelper, object htmlAttributes, object routeValues)
        {
            string rawUrl = htmlHelper.ViewContext.HttpContext.Request.RawUrl;
            return htmlHelper.BeginForm(null, null, routeValues, FormMethod.Post, htmlAttributes);
        }

        #endregion

        #region Submit

        /// <summary>
        /// Returns a submit input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="value">The value.</param>
        /// <returns>An input element whose type attribute is set to "submit".</returns>
        public static MvcHtmlString Submit(this HtmlHelper html, string value)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "submit");
            builder.MergeAttribute("value", value);
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Returns a submit input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <returns>An input element whose type attribute is set to "submit".</returns>
        public static MvcHtmlString Submit(this HtmlHelper html, string value, string name)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "submit");
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("value", value);
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Returns a submit input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An input element whose type attribute is set to "submit".</returns>
        public static MvcHtmlString Submit(this HtmlHelper html, string value, string name, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var builder = new TagBuilder("input");
            builder.MergeAttributes<string, object>(attributes);
            builder.MergeAttribute("type", "submit");
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("value", value);
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        #endregion

        #region File

        /// <summary>
        /// Returns a file input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <returns>An input element whose type attribute is set to "file".</returns>
        public static MvcHtmlString File(this HtmlHelper html, string name)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "file");
            builder.MergeAttribute("name", name);
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Returns a file input element by using the specified HTML helper and the name of the form field.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An input element whose type attribute is set to "file".</returns>
        public static MvcHtmlString File(this HtmlHelper html, string name, object htmlAttributes)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var builder = new TagBuilder("input");
            builder.MergeAttributes<string, object>(attributes);
            builder.MergeAttribute("type", "file");
            builder.MergeAttribute("name", name);
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        #endregion

        #region OpenGraph

        public static SrkOpenGraphHtmlExtensions OpenGraph(this HtmlHelper html)
        {
            return new SrkOpenGraphHtmlExtensions(html);
    }



        #endregion

        #region ActionLink

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
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes = null, string fragment = null, string controllerName = null, string protocol = null, string hostName = null)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        #endregion
    }
}
