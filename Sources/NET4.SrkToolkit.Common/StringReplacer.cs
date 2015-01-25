
namespace SrkToolkit.Common
{
    using System;
    using System.Collections.Generic;
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

        private Dictionary<string, Func<StringReplacement, string>> replacements = new Dictionary<string, Func<StringReplacement, string>>();
        ////private StringReplacement defaultReplacement = StringReplacement.Empty;
        private Func<StringReplacement, string> defaultReplacement;

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
        public StringReplacer Setup(string key, Func<StringReplacement, string> replace)
        {
            this.replacements.Add(key, replace);
            return this;
        }

        /// <summary>
        /// Setups the default replacement.
        /// </summary>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public StringReplacer SetupDefault(Func<StringReplacement, string> replace)
        {
            this.defaultReplacement = replace;
            return this;
        }

        /// <summary>
        /// Replaces the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string Replace(string text)
        {
            return replaceRegex.Replace(text, x =>
            {
                if (x.Groups.Count > 1)
                {
                    var key = x.Groups[1].Value;
                    var param = x.Groups.Count > 2 ? x.Groups[2].Value : "";

                    if (this.replacements.ContainsKey(key))
                    {
                        return this.replacements[key](new StringReplacement(key));
                    }
                    else if (this.defaultReplacement != null)
                    {
                        return this.defaultReplacement(new StringReplacement(key));
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

        public class StringReplacement
        {
            private Func<string, string> keyToValue;

            internal StringReplacement(string key)
            {
                this.Key = key;
            }

            /// <summary>
            /// Gets or sets the replace key.
            /// </summary>
            public string Key { get; set; }
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
            return replaceRegex.Replace(text, x =>
            {
                if (x.Groups.Count > 1)
                {
                    var key = x.Groups[1].Value;
                    var param = x.Groups.Count > 2 ? x.Groups[2].Value : "";

                    var rpl = new StringReplacement<TModel>(key, model);
                    rpl.Parameter = param;
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

        public class StringReplacement<TModel>
        {
            internal StringReplacement(string key, TModel model)
            {
                this.Key = key;
                this.Model = model;
            }

            /// <summary>
            /// Gets or sets the replace key.
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Gets or sets the model.
            /// </summary>
            public TModel Model { get; set; }

            /// <summary>
            /// Gets or sets the optional replace parameter.
            /// </summary>
            public string Parameter { get; set; }
        }
    }
}
