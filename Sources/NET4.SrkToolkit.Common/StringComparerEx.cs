
namespace SrkToolkit.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

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
