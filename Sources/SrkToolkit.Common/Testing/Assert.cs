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

namespace SrkToolkit.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Allows to make assertions in unit tests.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Compares two strings to and throws an exception is their value differs.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <exception cref="System.ArgumentException">
        /// The actual string was expected null
        /// or
        /// The actual string is null
        /// or
        /// Both strings are different.
        /// </exception>
        public static void AreEqual(string expected, string actual)
        {
            if (expected == null && actual != null)
                throw new ArgumentException("The actual string was expected null instead of <" + actual + ">");
            if (expected != null && actual == null)
                throw new ArgumentException("The actual string is null");
            if (expected == actual)
                return;

            int lengthDiffIndex = Math.Max(expected.Length, actual.Length);
            for (int i = 0; i < lengthDiffIndex; i++)
            {
                if (expected.Length == i)
                {
                    string msg = "Strings differ at index " + i + ": actual is longer than expected\r\n";
                    int quoteAt = Math.Max(0, i - 10);
                    msg += "> " + expected.Substring(quoteAt, expected.Length - quoteAt) + "\r\n";
                    msg += "> " + actual.Substring(quoteAt, actual.Length - quoteAt) + "\r\n";
                    msg += "--" + string.Concat(Enumerable.Repeat("-", i - quoteAt)) + "^ ";
                    throw new ArgumentException(msg);
                }

                if (actual.Length == i)
                {
                    string msg = "Strings differ at index " + i + ": actual is shorter than expected\r\n";
                    int quoteAt = Math.Max(0, i - 10);
                    msg += "> " + expected.Substring(quoteAt, expected.Length - quoteAt) + "\r\n";
                    msg += "> " + actual.Substring(quoteAt, actual.Length - quoteAt) + "\r\n";
                    msg += "--" + string.Concat(Enumerable.Repeat("-", i - quoteAt)) + "^ ";
                    throw new ArgumentException(msg);
                }

                char e = expected[i];
                char a = actual[i];

                if (e != a)
                {
                    string msg = "Strings differ at index " + i + "\r\n";
                    int quoteAt = Math.Max(0, i - 10);
                    msg += "> " + expected.Substring(quoteAt, expected.Length - quoteAt) + "\r\n";
                    msg += "> " + actual.Substring(quoteAt, actual.Length - quoteAt) + "\r\n";
                    msg += "--" + string.Concat(Enumerable.Repeat("-", i - quoteAt)) + "^ ";
                    throw new ArgumentException(msg);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified string contains the specified searached string.
        /// </summary>
        /// <param name="search">The string to find.</param>
        /// <param name="value">The string to search in.</param>
        /// <exception cref="System.ArgumentException">
        /// The value cannot be empty;search
        /// or
        /// The value cannot be empty;value
        /// or
        /// The searched string does not exist in the value.
        /// </exception>
        public static void Contains(string search, string value)
        {
            if (string.IsNullOrEmpty(search))
                throw new ArgumentException("The value cannot be empty", "search");
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("The value cannot be empty", "value");

            var matches = new List<ContainsMatch>();
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == search[0])
                {
                    // begining of a match
                    var match = new ContainsMatch
                    {
                        ValueIndex = i,
                    };
                    matches.Add(match);
                }

                // iterate matches
                for (int m = 0; m < matches.Count; m++)
                {
                    var match = matches[m];
                    int index = i - match.ValueIndex;
                    if (index < search.Length)
                    {
                        if (value[i] == search[index])
                        {
                            match.Length = index + 1;
                        }
                    }
                    else
                    {
                        match.IsValid = match.Length == search.Length;

                        if (match.IsValid)
                            return;
                    }
                }
            }

            for (int m = 0; m < matches.Count; m++)
            {
                var match = matches[m];
                if (match.Length == search.Length)
                {
                    match.IsValid = true;
                    return;
                }
                else if ((match.ValueIndex + search.Length) > (value.Length))
                {

                }
            }

            var bestMatch = matches.OrderByDescending(m => m.Length).FirstOrDefault();

            if (bestMatch != null)
            {
                string msg = "Searched string was not found. Best partial match:\r\n";
                int quoteAt1 = Math.Max(0, bestMatch.ValueIndex + bestMatch.Length - 10);
                int quoteLength1 = Math.Min(value.Length - quoteAt1, quoteAt1 + 20);
                int quoteAt2 = Math.Max(0, bestMatch.Length - 10);
                int quoteLength2 = Math.Min(search.Length - quoteAt2, quoteAt2 + 20);
                int quoteDiff = Math.Max(0, bestMatch.ValueIndex - quoteAt1);
                msg += "V> " + value.Substring(quoteAt1, quoteLength1) + "\r\n";
                msg += "S> " + string.Concat(Enumerable.Repeat("-", quoteDiff)) + search.Substring(quoteAt2, quoteLength2) + "\r\n";
                msg += "   " + string.Concat(Enumerable.Repeat(" ", quoteDiff)) + "^" + (bestMatch.Length > 2 ? (string.Concat(Enumerable.Repeat("-", quoteLength2 - 2)) + "^") : bestMatch.Length == 2 ? "^" : "");
                throw new ArgumentException(msg);
            }
            else
            {
                string msg = "Searched string <" + search.TrimToLength(80) + "> was not found.";
                throw new ArgumentException(msg);
            }
        }

        internal class ContainsMatch
        {
            public int ValueIndex { get; set; }

            public int Length { get; set; }

            public bool IsValid { get; set; }
        }
    }
}
