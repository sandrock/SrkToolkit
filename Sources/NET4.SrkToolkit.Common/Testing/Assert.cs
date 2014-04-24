
namespace SrkToolkit.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Assert
    {
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
    }
}
