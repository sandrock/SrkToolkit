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

    // TODO: support for netstandard

    /// <summary>
    /// A string comparer class that accepts a <see cref="CompareOptions"/> argument.
    /// </summary>
    /// <example>
    /// // this comparer will ignore non-spacing characters (diacritics and co)
    /// var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
    /// </example>
    public class StringComparerEx : IEqualityComparer<string>
    {
        private readonly CultureInfo culture;
        private readonly CompareOptions options;

        public StringComparerEx(CultureInfo culture, CompareOptions options)
        {
            this.culture = culture;
            this.options = options;
        }

        public bool Equals(string x, string y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Length != y.Length)
            {
                return false;
            }

            return string.Compare(x, y, this.culture, this.options) == 0;
        }

        public int GetHashCode(string obj)
        {
            return 0; // force usage of the Equals method
        }
    }
}
