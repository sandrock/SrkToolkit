using System.Text;

namespace System.Web {

    /// <summary>
    /// Provides methods for encoding and decoding HTML and URL strings.
    /// </summary>
    public static class HttpUtility {

        /// <summary>
        /// Converts a text string into a URL-encoded string.
        /// </summary>
        /// <returns>A URL-encoded string.</returns>
        /// <param name="url">The text to URL-encode.</param>
        public static string UrlEncode(string url) {
            if (url == null) {
                return null;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(url);
            int num = 0;
            int num2 = 0;
            int length = bytes.Length;
            for (int i = 0; i < length; i++) {
                char ch = (char)bytes[i];
                if (ch == ' ') {
                    num++;
                } else if (!IsSafe(ch)) {
                    num2++;
                }
            }
            if ((num != 0) || (num2 != 0)) {
                byte[] buffer2 = new byte[length + (num2 * 2)];
                int num5 = 0;
                for (int j = 0; j < length; j++) {
                    byte num7 = bytes[j];
                    char ch2 = (char)num7;
                    if (IsSafe(ch2)) {
                        buffer2[num5++] = num7;
                    } else if (ch2 == ' ') {
                        buffer2[num5++] = 0x2b;
                    } else {
                        buffer2[num5++] = 0x25;
                        buffer2[num5++] = (byte)IntToHex((num7 >> 4) & 15);
                        buffer2[num5++] = (byte)IntToHex(num7 & 15);
                    }
                }
                bytes = buffer2;
            }
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        private static bool IsSafe(char ch) {
            if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9'))) {
                return true;
            }
            switch (ch) {
                case '\'':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
        }

        internal static char IntToHex(int n) {
            if (n <= 9) {
                return (char)(n + 0x30);
            }
            return (char)((n - 10) + 0x61);
        }

    }
}
