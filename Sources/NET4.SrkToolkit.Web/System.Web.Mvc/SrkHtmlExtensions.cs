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

    /// <summary>
    /// HTML extensions. 
    /// </summary>
    public static class SrkHtmlExtensions
    {
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
            string displayTime = date.ToLocalTime().ToString(displayDateFormat ?? "F");
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\">{0}</time>",
                    display,
                    date.ToString("O"),
                    date.ToUniversalTime().ToString("R"));
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
        public static MvcHtmlString DisplayDate(this HtmlHelper html, DateTime date, bool useTimeTag = true, string display = null, string displayDateFormat = "D")
        {
            string displayTime = date.ToLocalTime().ToString(displayDateFormat ?? "D");
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\">{0}</time>",
                    display,
                    date.ToString("D"),
                    date.ToUniversalTime().ToString("D"));
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
            string displayTime = date.ToLocalTime().ToString(displayDateFormat ?? "T");
            if (display == null)
                display = displayTime;

            if (useTimeTag)
            {
                string tag = string.Format(
                    "<time datetime=\"{1}\" title=\"{2}\">{0}</time>",
                    display,
                    date.ToString("O"),
                    date.ToUniversalTime().ToString("R"));
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
                    "<time datetime=\"{1}\" title=\"{2}\">{0}</time>",
                    display,
                    date.ToString("c"),
                    date.ToString("c"));
                return MvcHtmlString.Create(tag);
            }
            else
            {
                return MvcHtmlString.Create(display);
            }
        }

        #endregion

        #region Display text

        /// <summary>
        /// Replaces new lines by line breaks (<&lt;br /&gt;)
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static MvcHtmlString LineBreaks(this HtmlHelper html, string content)
        {
            if (content == null)
                return MvcHtmlString.Create(string.Empty);

            content = html.Encode(content);

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
        /// <returns>
        /// an escaped HTML string
        /// </returns>
        public static MvcHtmlString DisplayText(this HtmlHelper html, string content, bool makeLinks = true, bool makeParagraphs = true, bool makeLineBreaks = true, bool twitterLinks = false, string linksClass = "external", string linksTarget = "_blank")
        {
            if (content == null)
                return MvcHtmlString.Create(string.Empty);

            ////content = html.Encode(content);
            content = content.ProperHtmlEscape();

            if (makeLinks)
                content = content.LinksAsHtml(linkClasses: linksClass, linkTarget: linksTarget);

            if (twitterLinks)
                content = content.TwitterLinksAsHtml(linkClasses: linksClass, linkTarget: linksTarget);

            if (makeParagraphs)
                content = content.HtmlParagraphizify(makeLineBreaks: makeLineBreaks);
            else if (makeLineBreaks)
                content = content.AddHtmlLineBreaks();

            return MvcHtmlString.Create(content);
        }

        #endregion

        #region Forms

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

        /// <summary>
        /// Writes an opening &lt;form&gt; tag to the response. When the user submits the form, the request will be processed by an action method.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An opening <form> tag.</returns>
        public static MvcForm BeginFormEx(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            string rawUrl = htmlHelper.ViewContext.HttpContext.Request.RawUrl;
            return htmlHelper.BeginForm(null, null, htmlHelper.ViewContext.RouteData.Values, FormMethod.Post, new RouteValueDictionary(htmlAttributes) as IDictionary<string, object>);
        }

        #endregion
    }
}
