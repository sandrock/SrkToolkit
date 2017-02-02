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

namespace SrkToolkit.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Basic variable replacer.
    /// </summary>
    public class StringReplacer
    {
        internal const string replaceRegexString = "{([a-zA-Z0-9_\\.]+)(?: ([ /,\"a-zA-Z0-9]+))?}";
        private static readonly Regex replaceRegex = new Regex(replaceRegexString, System.Text.RegularExpressions.RegexOptions.Compiled);

        private Dictionary<string, Func<StringReplacement<object>, string>> replacements = new Dictionary<string, Func<StringReplacement<object>, string>>();
        ////private StringReplacement defaultReplacement = StringReplacement.Empty;
        private Func<StringReplacement<object>, string> defaultReplacement;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReplacer"/> class.
        /// </summary>
        public StringReplacer()
        {
        }

        /// <summary>
        /// Setups a replacement for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public StringReplacer Setup(string key, Func<StringReplacement<object>, string> replace)
        {
            this.replacements.Add(key, replace);
            return this;
        }

        /// <summary>
        /// Setups the default replacement.
        /// </summary>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public StringReplacer SetupDefault(Func<StringReplacement<object>, string> replace)
        {
            this.defaultReplacement = replace;
            return this;
        }

        /// <summary>
        /// Replaces the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>the modified text</returns>
        public string Replace(string text)
        {
            return this.Replace(text, CultureInfo.CurrentCulture, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Replaces the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="culture">the culture to use</param>
        /// <param name="timezone">the timezone to use</param>
        /// <returns>the modified text</returns>
        public string Replace(string text, IFormatProvider culture, TimeZoneInfo timezone)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (timezone == null)
                throw new ArgumentNullException("timezone");

            return replaceRegex.Replace(text, x =>
            {
                if (x.Groups.Count > 1)
                {
                    var key = x.Groups[1].Value;
                    var param = x.Groups.Count > 2 ? x.Groups[2].Value : "";

                    var rpl = new StringReplacement<object>(key, null, param, culture, timezone);
                    if (this.replacements.ContainsKey(key))
                    {
                        return this.replacements[key](rpl);
                    }
                    else if (this.defaultReplacement != null)
                    {
                        return this.defaultReplacement(rpl);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                return x.Groups[0].Value;
            });
        }

        /// <summary>
        /// Gets the replacement keys.
        /// </summary>
        /// <returns></returns>
        public string[] GetKeys()
        {
            return this.replacements.Keys.ToArray();
        }
    }

    /// <summary>
    /// Model-based basic variable replacer.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class StringReplacer<TModel>
    {
        private static readonly Regex replaceRegex = new Regex(StringReplacer.replaceRegexString, System.Text.RegularExpressions.RegexOptions.Compiled);

        private Dictionary<string, Func<StringReplacement<TModel>, string>> replacements = new Dictionary<string, Func<StringReplacement<TModel>, string>>();
        private Func<StringReplacement<TModel>, string> defaultReplacement;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReplacer{TModel}"/> class.
        /// </summary>
        public StringReplacer()
        {
        }

        /// <summary>
        /// Setups a replacement for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public StringReplacer<TModel> Setup(string key, Func<StringReplacement<TModel>, string> replace)
        {
            this.replacements.Add(key, replace);
            return this;
        }

        /// <summary>
        /// Setups the default replacement.
        /// </summary>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public StringReplacer<TModel> SetupDefault(Func<StringReplacement<TModel>, string> replace)
        {
            this.defaultReplacement = replace;
            return this;
        }

        /// <summary>
        /// Gets the replacement keys.
        /// </summary>
        /// <returns></returns>
        public string[] GetKeys()
        {
            return this.replacements.Keys.ToArray();
        }

        /// <summary>
        /// Replaces the specified text with the specified model.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public string Replace(string text, TModel model)
        {
            return this.Replace(text, model, CultureInfo.CurrentCulture, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Replaces the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="model">The model.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="timezone">The timezone.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// culture
        /// or
        /// timezone
        /// </exception>
        public string Replace(string text, TModel model, IFormatProvider culture, TimeZoneInfo timezone)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (timezone == null)
                throw new ArgumentNullException("timezone");

            return replaceRegex.Replace(text, x =>
            {
                if (x.Groups.Count > 1)
                {
                    var key = x.Groups[1].Value;
                    var param = x.Groups.Count > 2 ? x.Groups[2].Value : "";

                    var rpl = new StringReplacement<TModel>(key, model, param, culture, timezone);
                    if (this.replacements.ContainsKey(key))
                    {
                        return this.replacements[key](rpl);
                    }
                    else if (this.defaultReplacement != null)
                    {
                        return this.defaultReplacement(rpl);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                return x.Groups[0].Value;
            });
        }
    }

    public class StringReplacement<TModel>
    {
        private readonly string key;
        private readonly TModel model;
        private readonly string parameter;
        private readonly IFormatProvider culture;
        private TimeZoneInfo timezone;

        internal StringReplacement(string key, TModel model, string parameter, IFormatProvider culture, TimeZoneInfo timezone)
        {
            this.key = key;
            this.model = model;
            this.parameter = parameter;
            this.culture = culture;
            this.timezone = timezone;
        }

        /// <summary>
        /// Gets or sets the replace key.
        /// </summary>
        public string Key
        {
            get { return this.key; }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public TModel Model
        {
            get { return this.model; }
        }

        /// <summary>
        /// Gets or sets the optional replace parameter.
        /// </summary>
        public string Parameter
        {
            get { return this.parameter; }
        }

        public IFormatProvider Culture
        {
            get { return this.culture; }
        }

        public TimeZoneInfo Timezone
        {
            get { return this.timezone; }
        }
    }
}
