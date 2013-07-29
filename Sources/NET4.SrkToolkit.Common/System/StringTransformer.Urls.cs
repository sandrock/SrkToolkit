// -----------------------------------------------------------------------
// <copyright file="StringTransformer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace System
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Globalization;

    /// <summary>
    /// String-related operations.
    /// </summary>
    partial class SrkStringTransformer
    {
        const string linkFormat = "<a href=\"{0}\" title=\"{0}\" target=\"{2}\" class=\"{3}\">{1}</a>";

        private static Regex linksAsHtmlRegex;
        private static Regex replaceLinkRegex;
        private static Regex twitterHasLinkRegex = new Regex("\\#\\w+");
        private static Regex twitterUsernameLinkRegex = new Regex("@[A-Za-z0-9_]{1,20}");

        /// <summary>
        /// Replaces all URLs in a string with HTML links.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="linkClasses"></param>
        /// <param name="linkTarget"></param>
        /// <returns></returns>
        public static string LinksAsHtml(this string text, string linkClasses = "external", string linkTarget = "_self")
        {
            if (linksAsHtmlRegex == null)
            {
                var regEmailLocalPart = @"[a-z0-9][a-z0-9_\.+-]+@";
                var regProtocol = new Regex(@"(https?://|"+regEmailLocalPart+@")?");
                var regDomain = new Regex(@"((?:[-a-zA-Z0-9]{1,63}\.)+[-a-zA-Z0-9]{2,63}|(?:[0-9]{1,3}\.){3}[0-9]{1,3})");
                var regPort = new Regex(@"(:[0-9]{1,5})?");
                var regPath = new Regex(@"(/[!$-/0-9:;=@_\':;!a-zA-Z\x7f-\xff]*?)?");
                var regQuery = new Regex(@"(\?[!$-/0-9:;=@_\':;!a-zA-Z\x7f-\xff]+?)?");
                var regFragment = new Regex(@"(#[!$-/0-9:;=@_\':;!a-zA-Z\x7f-\xff]+?)?");
                linksAsHtmlRegex = new Regex("\\b" + regProtocol + regDomain + regPort + regPath + regQuery + regFragment + "(?=[?.!,;:\"]?([\\s]|$))");
            }

            var results = linksAsHtmlRegex.Matches(text);
            var list = new List<string>();
            var matches = new List<Match>();

            foreach (Match item in results)
            {
                matches.Add(item);
            }

            foreach (Match match in matches.OrderByDescending(m => m.Length))
            {
                if (list.Contains(match.Value) || list.Any(s => s.Contains(match.Value)))
                    continue;
                list.Add(match.Value);

                string full = match.Value, friendly = full;
                if (match.Groups[1].Value.Length > 1 && match.Groups[1].Value.EndsWith("@"))
                    full = "mailto:" + full;
                else if (string.IsNullOrEmpty(match.Groups[1].Value))
                    full = "http://" + full;
                if (friendly.Length > 50)
                    friendly = full.Substring(0, 25) + "..." + full.Substring(full.Length - 26);

                full = full.ProperHtmlAttributeEscape();
                friendly = friendly.ProperHtmlEscape();

                string link = string.Format(CultureInfo.InvariantCulture, linkFormat, full, friendly, linkTarget, linkClasses);

                text = text.Replace(match.Value, link);
            }

            return text;
        }

        public static string TwitterLinksAsHtml(this string text, string linkClasses = "external", string linkTarget = "_self")
        {
            var results = new List<Match>();
            var hashResults = twitterHasLinkRegex.Matches(text);
            var userResults = twitterUsernameLinkRegex.Matches(text);
            foreach (Match item in hashResults)
            {
                results.Add(item);
            }
            foreach (Match item in userResults)
            {
                results.Add(item);
            }

            var list = new List<string>();

            foreach (Match match in results.OrderByDescending(m => m.Length))
            {
                if (list.Contains(match.Value) || list.Any(s => s.Contains(match.Value)))
                    continue;
                list.Add(match.Value);

                string full = null;
                if (match.Groups[0].Value[0] == '#')
                {
                    full = "https://twitter.com/search/realtime?q=%23" + Uri.EscapeDataString(match.Groups[0].Value.Substring(1)) + "&src=hash";
                }
                else if (match.Groups[0].Value[0] == '@')
                {
                    full = "https://twitter.com/" + match.Groups[0].Value.Substring(1);
                }

                full = full.ProperHtmlEscape();

                string link = string.Format(CultureInfo.InvariantCulture, linkFormat, full, match.Groups[0].Value, linkTarget, linkClasses);

                text = text.Replace(match.Value, link);
            }

            return text;
        }
    }
}
