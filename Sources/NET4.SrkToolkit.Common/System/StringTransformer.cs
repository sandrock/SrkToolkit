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
    using System.Globalization;

    /// <summary>
    /// String-related operations.
    /// </summary>
    public static partial class SrkStringTransformer
    {
        #region Statics

        /// <summary>
        /// Hand-made list of ligatures (result of two chars merged in one) and other 2-letter chars.
        /// </summary>
        private static readonly Dictionary<char, string> diacriticsReplacements = new Dictionary<char, string>
        {
            { '\u0132', "IJ" }, // Ĳ
            { '\u0133', "ij" }, // ĳ
            { '\u00C6', "Ae" }, // Æ
            { '\u00E6', "ae" }, // æ
            { '\u0153', "oe" }, // œ
            { '\u0152', "Oe" }, // Œ
            { '\u01C4', "DZ" }, // Ǆ
            { '\u01C5', "Dz" }, // ǅ
            { '\u01C6', "dz" }, // ǆ
            { '\u01C7', "LJ" }, // Ǉ
            { '\u01C8', "Lj" }, // ǈ
            { '\u01C9', "lj" }, // ǉ
            { '\u01CA', "NJ" }, // Ǌ
            { '\u01CB', "Nj" }, // ǋ
            { '\u01CC', "nj" }, // ǌ
            { '\u01E2', "AE" }, // Ǣ
            { '\u01E3', "ae" }, // ǣ
            { '\u01F1', "DZ" }, // Ǳ
            { '\u01F2', "Dz" }, // ǲ
            { '\u01F3', "dz" }, // ǳ
            { '\u01F6', "Hu" }, // Ƕ
            { '\u0238', "db" }, // ȸ
            { '\u0239', "qp" }, // ȹ
            { '\u026E', "lz" }, // ɮ
            { '\u0276', "OE" }, // ɶ
            { '\u02A3', "dz" }, // ʣ
            { '\u02A4', "dz" }, // ʤ
            { '\u02A5', "dz" }, // ʥ
            { '\u02A6', "ts" }, // ʦ
            { '\u02A7', "tf" }, // ʧ
            { '\u02A8', "tc" }, // ʨ
            { '\u02A9', "fn" }, // ʩ
            { '\u02AA', "ls" }, // ʪ
            { '\u02AB', "lz" }, // ʫ
            { '\u04D4', "AE" }, // Ӕ
            { '\u04D5', "ae" }, // ӕ
            { '\u00DF', "ss" }, // ß
        };

        #endregion

        /// <summary>
        /// Inserts HTML line breaks (&lt;br /&gt;) before all newlines, keeping line breaks in the string.
        /// </summary>
        /// <param name="text">text containing lines</param>
        /// <returns></returns>
        public static string AddHtmlLineBreaks(this string text)
        {
            if (text == null)
                return null;

            // windows mode
            if (text.Contains("\r\n"))
                return text.Replace("\r\n", "<br />\r\n");

            // unix mode
            else if (text.Contains("\n"))
                return text.Replace("\n", "<br />\n");

            // mac mode
            else if (text.Contains("\r"))
                return text.Replace("\r", "<br />\r");

            // no new lines ^^
            else
                return text;
        }

        /// <summary>
        /// Trims the text from the right, leaving the specified number of characters.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="chars">The chars.</param>
        /// <param name="ending">The ending.</param>
        /// <returns></returns>
        public static string TrimTextRight(this string text, int chars, string ending = "...")
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            if (text.Length <= chars)
                return text;

            text = text.Trim();
            if (text.Length <= (chars))
                return text;

            var trimmed = text.Substring(0, chars - ending.Length);
            return TrimTextToWord(ending, true, ref trimmed);
        }

        /// <summary>
        /// Trims the text from the left, leaving the specified number of characters.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="chars">The chars.</param>
        /// <param name="ending">The ending.</param>
        /// <returns></returns>
        public static string TrimTextLeft(this string text, int chars, string ending = "...")
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            
            if (text.Length <= chars)
                return text;

            text = text.Trim();
            if (text.Length <= (chars))
                return text;

            var trimmed = text.Substring(text.Length - chars + ending.Length);
            return TrimTextToWord(ending, false, ref trimmed);
        }

        private static string TrimTextToWord(string ending, bool trimFromRight, ref string trimmed)
        {
            var white = new char[] { ' ', '\t', '\n', '\r', };

            char last;
            while (trimmed.Length > 1)
            {
                if (trimFromRight)
                    last = trimmed[trimmed.Length - 1];
                else
                    last = trimmed[0];

                switch (CharUnicodeInfo.GetUnicodeCategory(last))
                {
                    case UnicodeCategory.Control:
                    case UnicodeCategory.CurrencySymbol:
                    case UnicodeCategory.DecimalDigitNumber:
                    case UnicodeCategory.EnclosingMark:
                    case UnicodeCategory.FinalQuotePunctuation:
                    case UnicodeCategory.Format:
                    case UnicodeCategory.InitialQuotePunctuation:
                    case UnicodeCategory.LetterNumber:
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.MathSymbol:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.ModifierSymbol:
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.OtherLetter:
                    case UnicodeCategory.OtherNotAssigned:
                    case UnicodeCategory.OtherNumber:
                    case UnicodeCategory.OtherSymbol:
                    case UnicodeCategory.PrivateUse:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.Surrogate:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                        return (trimFromRight ? "" : ending) + trimmed + (trimFromRight ? ending : "");

                    default:
                        if (trimFromRight)
                            trimmed = trimmed.Substring(0, trimmed.Length - 1);
                        else
                            trimmed = trimmed.Substring(1);
                        break;
                }
            }

            return trimmed;
        }

        /// <summary>
        /// Inserts HTML paragraphs between grouped newlines.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="makeLineBreaks">if set to <c>true</c> inserts HTML line breaks (&lt;br /&gt;) before all newlines.</param>
        /// <returns></returns>
        public static string HtmlParagraphizify(this string text, bool makeLineBreaks = false)
        {
            if (text == null)
                return string.Empty;

            // windows mode
            else if (text.Contains("\r\n"))
                return HtmlParagraphizifyImpl("\r\n", text, makeLineBreaks);

            // unix mode
            else if (text.Contains("\n"))
                return HtmlParagraphizifyImpl("\n", text, makeLineBreaks);

            // mac mode
            else if (text.Contains("\r"))
                return HtmlParagraphizifyImpl("\r", text, makeLineBreaks);

            // no lines ^^
            else
                return "<p>" + text + "</p>";
        }

        /// <summary>
        /// Uppercases the first letter of all words.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ToUpperFirstLetters(this string text)
        {
            char[] array = text.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(array[i - 1]))
                {
                    case UnicodeCategory.CurrencySymbol:
                    case UnicodeCategory.LetterNumber:
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.MathSymbol:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.ModifierSymbol:
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.OtherLetter:
                    case UnicodeCategory.OtherNotAssigned:
                    case UnicodeCategory.OtherNumber:
                    case UnicodeCategory.OtherSymbol:
                    case UnicodeCategory.PrivateUse:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                        break;

                    default:
                        array[i] = char.ToUpper(array[i]);
                        break;
                }
            }

            return new string(array);
        }

        /// <summary>
        /// Removes the diacritics (accents and other forms) from the specified text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string input)
        {
            if (input == null)
                return null;

            string stFormD = input.Normalize(NormalizationForm.FormD);
            int len = stFormD.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                char c = stFormD[i];
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    if (diacriticsReplacements.ContainsKey(c))
                    {
                        sb.Append(diacriticsReplacements[c]);
                    }
                    else
                    {
                        sb.Append(stFormD[i]);
                    }
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// UNTESTED! Removes the empty characters.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string RemoveSpaces(this string input)
        {
            if (input == null)
                return null;

            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                switch (CharUnicodeInfo.GetUnicodeCategory(input[i]))
                {
                    case UnicodeCategory.LineSeparator:
                    case UnicodeCategory.ParagraphSeparator:
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.Control:
                        break;

                    default:
                        sb.Append(input[i]);
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Escapes XML/HTML characters that needs to be escaped when outputing HTML.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <remarks>
        /// Code based on http://wonko.com/post/html-escaping and https://www.owasp.org/index.php/XSS_%28Cross_Site_Scripting%29_Prevention_Cheat_Sheet.
        /// </remarks>
        public static string ProperHtmlEscape(this string content)
        {
            if (content == null)
                return null;

            var builder = new StringBuilder();
            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];
                switch (c)
                {
                    case '<':
                        builder.Append("&lt;");
                        break;

                    case '>':
                        builder.Append("&gt;");
                        break;

                    case '&':
                        builder.Append("&amp;");
                        break;

                    case '"':
                        builder.Append("&quot;");
                        break;

                    case '\'':
                        builder.Append("&#x27;");
                        break;

                    ////case '/':
                    ////    builder.Append("&#x2F;");
                    ////    break;

                    default:
                        builder.Append(content[i]);
                        break;
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Escapes XML/HTML characters that needs to be escaped for an attribute (&lt;, &gt;, &amp;, &quot;).
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <remarks>
        /// Code based on http://wonko.com/post/html-escaping and https://www.owasp.org/index.php/XSS_%28Cross_Site_Scripting%29_Prevention_Cheat_Sheet.
        /// </remarks>
        public static string ProperHtmlAttributeEscape(this string content)
        {
            if (content == null)
                return null;

            var builder = new StringBuilder();
            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];
                switch (c)
                {
                    case '<':
                        builder.Append("&lt;");
                        break;

                    case '>':
                        builder.Append("&gt;");
                        break;

                    case '&':
                        builder.Append("&amp;");
                        break;

                    case '"':
                        builder.Append("&quot;");
                        break;

                    case '\'':
                        builder.Append("&#x27;");
                        break;

                    case '/':
                        builder.Append("&#x2F;");
                        break;

                    case ' ':
                        builder.Append("&nbsp;");
                        break;

                    case '`':
                        builder.Append("&#x60;");
                        break;

                    case '!':
                        builder.Append("&#x21;");
                        break;

                    case '@':
                        builder.Append("&#x40;");
                        break;
                        
                    case '$':
                        builder.Append("&#x24;");
                        break;

                    case '%':
                        builder.Append("&#x25;");
                        break;

                    case '(':
                        builder.Append("&#x28;");
                        break;

                    case ')':
                        builder.Append("&#x29;");
                        break;

                    case '=':
                        builder.Append("&#x3D;");
                        break;

                    case '+':
                        builder.Append("&#x2B;");
                        break;
                        
                    case '{':
                        builder.Append("&#x7B;");
                        break;

                    case '}':
                        builder.Append("&#x7D;");
                        break;

                    case '[':
                        builder.Append("&#x5B;");
                        break;

                    case ']':
                        builder.Append("&#x5D;");
                        break;

                    default:
                        builder.Append(content[i]);
                        break;
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Transforms and strips characters to make a URL-friendly string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="preserveCase">if set to <c>true</c> [preserve case].</param>
        /// <param name="preserveChars">Characters to preserve.</param>
        /// <returns></returns>
        public static string MakeUrlFriendly(this string input, bool preserveCase, char[] preserveChars = null)
        {
            var val = input.RemoveDiacritics();
            preserveChars = preserveChars ?? new char[0];

            var sb = new StringBuilder();

            // inspired from 
            // http://stackoverflow.com/a/17092315/282105
            for (int i = 0; i < val.Length; i++)
            {
                char ch = val[i];
                if (preserveChars.Contains(ch))
                {
                    sb.Append(ch);
                    continue;
                }

                var charInfo = CharUnicodeInfo.GetUnicodeCategory(ch);
                switch (charInfo)
                {
                    // Keep these as they are
                    case UnicodeCategory.DecimalDigitNumber:
                    case UnicodeCategory.LetterNumber:
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.CurrencySymbol:
                    case UnicodeCategory.OtherLetter:
                    case UnicodeCategory.OtherNumber:
                        sb.Append(ch);
                        break;

                    // Convert these to dashes
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.MathSymbol:
                    case UnicodeCategory.ModifierSymbol:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.OtherSymbol:
                    case UnicodeCategory.SpaceSeparator:
                        sb.Append('-');
                        break;

                    // Convert to lower-case
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                        if (preserveCase)
                            sb.Append(ch);
                        else
                            sb.Append(char.ToLowerInvariant(ch));
                        break;

                    // Ignore certain types of characters
                    case UnicodeCategory.OpenPunctuation:
                    case UnicodeCategory.ClosePunctuation:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.Control:
                    case UnicodeCategory.EnclosingMark:
                    case UnicodeCategory.FinalQuotePunctuation:
                    case UnicodeCategory.Format:
                    case UnicodeCategory.InitialQuotePunctuation:
                    case UnicodeCategory.LineSeparator:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.OtherNotAssigned:
                    case UnicodeCategory.ParagraphSeparator:
                    case UnicodeCategory.PrivateUse:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.Surrogate:
                        break;
                }
            }

            var built = sb.ToString();

            while (built.Contains("--"))
                built = built.Replace("--", "-");

            while (built.EndsWith("-"))
                built = built.Substring(0, built.Length - 1);

            while (built.StartsWith("-"))
                built = built.Substring(1, built.Length - 1);

            return built;
        }

        /// <summary>
        /// Gets the incremented string (blah, blah-1, blah-2...)
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="uniquenessCheck">The uniqueness check.</param>
        /// <param name="incrementSeparator">The increment separator.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static string GetIncrementedString(this string input, Func<string, bool> uniquenessCheck = null, char incrementSeparator = '-', int startIndex = 1)
        {
            // inspired from http://stackoverflow.com/a/17092315/282105
            var value = input;
            startIndex -= 1;
            int numToInc;
            do
            {
                var parts = value.Split(incrementSeparator);
                var lastPortion = parts.LastOrDefault();
                bool incExisting;
                if (lastPortion == null)
                {
                    numToInc = startIndex;
                    incExisting = false;
                }
                else
                {
                    if (int.TryParse(lastPortion, out numToInc))
                    {
                        incExisting = true;
                        if (numToInc <= startIndex)
                            numToInc = startIndex;
                    }
                    else
                    {
                        incExisting = false;
                        numToInc = startIndex;
                    }
                }

                var fragToKeep = incExisting
                    ? string.Join(incrementSeparator.ToString(), parts.Take(parts.Length - 1).ToArray())
                    : value;
                value = fragToKeep + incrementSeparator + (++numToInc).ToString();
            } while (uniquenessCheck != null && !uniquenessCheck(value));

            return value;
        }

        private static string HtmlParagraphizifyImpl(string separator, string text, bool makeLineBreaks)
        {
            return
                "<p>" +
                text.Split(new string[] { separator + separator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => makeLineBreaks ? s.AddHtmlLineBreaks() : s)
                    .Aggregate((s1, s2) => s1 + "</p>" + Environment.NewLine + "<p>" + s2)
                + "</p>";
        }

        /// <summary>
        /// Unescapes the unicode sequences (\x00-\xFF) in the given string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string UnescapeUnicodeSequences(this string input)
        {
            var sb = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\\')
                {
                    if (input.Length > i + 3
                     && (input[i + 1] == 'x' || input[i + 1] == 'X')
                     && IsHex(input[i + 2])
                     && IsHex(input[i + 3]))
                    {
                        ushort number;
                        if (ushort.TryParse(input.Substring(i + 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number))
                        {
                            var bytes = BitConverter.GetBytes(number);
                            var str = Encoding.Unicode.GetString(bytes);
                            sb.Append(str);
                            i += 3;
                            continue;
                        }
                    }
                }

                sb.Append(input[i]);
            }

            return sb.ToString();
        }
        /*
         * this is wrong
         * https://en.wikipedia.org/wiki/UTF8#Description
         * 
        public static string UnescapeUTF8Sequences(this string input)
        {
            var sb = new StringBuilder();

            bool backSlash = false, havex = false, havea = false, haveb = false;
            char a = ' ', b;
            char[] chars = new char[4];
            byte nchars = 0;
            bool ready = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\\')
                {
                    nchars = 0;
                    havex = false;
                    ready = false;
                    backSlash = true;
                }
                else
                {
                    if (backSlash)
                    {
                        if (input[i] == 'x' || input[i] == 'X')
                        {
                            havex = true;
                            chars = new char[4];
                        }
                        else
                        {
                            if (havex)
                            {
                                if (input[i] >= 0x30 && input[i] <= 0x39 || input[i] >= 0x41 && input[i] <= 0x5A && nchars < 4)
                                {
                                    if (nchars == 0)
                                    {
                                        nchars++;
                                        chars[0] = input[i];
                                    }
                                    else if (nchars == 1)
                                    {
                                        nchars++;
                                        chars[1] = input[i];
                                        ready = !IsHex(input[i + 1]) || !IsHex(input[i + 2]);
                                    }
                                    else if (nchars == 2)
                                    {
                                        nchars++;
                                        chars[2] = input[i];
                                    }
                                    else if (nchars == 3)
                                    {
                                        nchars++;
                                        chars[3] = input[i];
                                        ready = true;
                                    }
                                }
                                
                                if (ready)
                                {
                                    uint number;
                                    if (uint.TryParse(new string(chars, 0, nchars), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out number))
                                    {
                                        sb.Remove(sb.Length + 1 - nchars - 2, 2 + nchars - 1);
                                        var uintAsBytes = BitConverter.GetBytes(number);
                                        byte size = checked((byte)uintAsBytes.Length);
                                        for (byte j = 0; j < uintAsBytes.Length; j++)
                                        {
                                            if (uintAsBytes[uintAsBytes.Length-j-1] != 0)
                                            {
                                                size = checked((byte)(uintAsBytes.Length - j));
                                                break;
                                            }
                                        }
                                        var bytes = new byte[size];
                                        for (byte j = 0; j < size; j++)
                                        {
                                            bytes[j] = uintAsBytes[j];
                                        }

                                        var str = Encoding.UTF8.GetString(bytes);
                                        sb.Append(str);
                                        backSlash = havex = false;
                                        nchars = 0;
                                        ready = false;
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                havex = false;
                            }
                        }
                    }
                    else
                    {
                        backSlash = false;
                    }
                }

                sb.Append(input[i]);
            }

            return sb.ToString();
        }
        */
        private static bool IsHex(char value)
        {
            return value >= 0x30 && value <= 0x39 || value >= 0x41 && value <= 0x5A;
        }
    }
}
