using System;
using System.Windows;
using System.Windows.Data;

namespace SrkToolkit.Xaml.Converters {

    public class BoolToVisibilityConverter : IValueConverter {

        #region IValueConverter Members

        /// <summary>
        /// Converts a boolean to a <see cref="Visibility"/>.
        /// If value is null, <see cref="Visibility.Collapsed"/> is returned.
        /// </summary>
        /// <param name="value">
        /// a boolean to convert (nullable or not)
        /// </param>
        /// <param name="targetType"></param>
        /// <param name="parameter">
        /// Reverse the behavior if begins with '!'.
        /// Will return <see cref="Visibility.Visible"/> for null value if it contains 'N'.
        /// Will return <see cref="Visibility.Hidden"/> instead of <see cref="Visibility.Collapsed"/> if it contains 'H'.
        /// </param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool reverse = false;
            bool val = false;
            bool showIfNull = false;
            bool hide = false;
            var vHide = Visibility.Collapsed;

            // parse parameter
            if (parameter is string) {
                string strparam = (string)parameter;
                if (strparam.Length > 0) {
                    reverse = strparam[0] == '!';
                    showIfNull = strparam.IndexOf('N') >= 0;
                    hide = strparam.IndexOf('H') >= 0;
                } else {
                    reverse = false;
                }
            } else if (parameter is bool)
                reverse = (bool)parameter;
            else if (parameter is bool?) {
                bool? p = ((bool?)parameter);
                reverse = p.HasValue && p.Value;
            }
            if (hide)
                vHide = Visibility.Hidden;

            // parse value
            if (value == null) {
                return showIfNull ? Visibility.Visible : vHide;
            } else if (value is string) {
                val = bool.Parse((string)value);
            } else if (value is bool) {
                val = (bool)value;
            } else if (value is bool?) {
                bool? p = (bool?)value;
                if (p.HasValue)
                    val = p.Value;
                else
                    return showIfNull ? Visibility.Visible : vHide;
            }

            if (reverse)
                return !val ? Visibility.Visible : vHide;
            else
                return val ? Visibility.Visible : vHide;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion

    }
}
