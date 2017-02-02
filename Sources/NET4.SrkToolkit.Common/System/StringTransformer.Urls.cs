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
        private static Regex twitterHasLinkRegex = new Regex("\\#\\w+;?");
        private static Regex twitterUsernameLinkRegex = new Regex("@[A-Za-z0-9_]{1,20}");

        /// <summary>
        /// Replaces all URLs in a string with HTML links.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="linkClasses">The class attribute to associate to &lt;a&gt; tags (defaults to "external").</param>
        /// <param name="linkTarget">The target attribute to associate to &lt;a&gt; tags (default to "_blank").</param>
        /// <param name="avoidDoubleEscape">if set to <c>true</c> [avoid double escape].</param>
        /// <returns></returns>
        public static string LinksAsHtml(this string text, string linkClasses = "external", string linkTarget = "_self", bool avoidDoubleEscape = false)
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
            var attrEscape = new Func<string, string>(val => avoidDoubleEscape ? val : val.ProperHtmlAttributeEscape());
            var htmlEscape = new Func<string, string>(val => avoidDoubleEscape ? val : val.ProperHtmlEscape());

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
                string cssClass = linkClasses;
                if (match.Groups[1].Value.Length > 1 && match.Groups[1].Value.EndsWith("@"))
                {
                    full = "mailto:" + full;
                    cssClass += " mailto";
                }
                else if (string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    full = "http://" + full;
                }

                if (friendly.Length > 50)
                {
                    friendly =
                        htmlEscape(full.Substring(0, 25)) +
                        "<span class=\"link-trim\">" + htmlEscape(full.Substring(25, full.Length - 26 - 25)) + "</span>" +
                        htmlEscape(full.Substring(full.Length - 26));
                }
                else
                {
                    friendly = htmlEscape(friendly);
                }

                full = attrEscape(full);

                string link = string.Format(CultureInfo.InvariantCulture, linkFormat, full, friendly, linkTarget, cssClass);

                text = text.Replace(match.Value, link);
            }

            return text;
        }

        /// <summary>
        /// Replaces all @mentions and #hashes in a string with HTML links to Twitter.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="linkClasses">The class attribute to associate to &lt;a&gt; tags (defaults to "external twitter").</param>
        /// <param name="linkTarget">The target attribute to associate to &lt;a&gt; tags (default to "_blank").</param>
        /// <returns></returns>
        public static string TwitterLinksAsHtml(this string text, string linkClasses = "external twitter", string linkTarget = "_self")
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
                string value = match.Groups[0].Value;
                string suffix = value[value.Length - 1] == ';' ? ";" : "";
                if (suffix == ";")
                    value = value.Substring(0, value.Length - 1);
                if (value[0] == '#')
                {
                    if (value.Length == 4 && (value[1] == 'x' || value[1] == 'X') && suffix == ";")
                    {
                        // this is a html entity like &#x27;
                        continue;
                    }
                    else
                    {
                        full = "https://twitter.com/search/realtime?q=%23" + Uri.EscapeDataString(value.Substring(1)) + "&src=hash";
                    }
                }
                else if (value[0] == '@')
                {
                    full = "https://twitter.com/" + value.Substring(1);
                }

                full = full.ProperHtmlEscape();

                string link = string.Format(CultureInfo.InvariantCulture, linkFormat, full, value, linkTarget, linkClasses);

                text = text.Replace(match.Value, link);
            }

            return text;
        }
    }
}
