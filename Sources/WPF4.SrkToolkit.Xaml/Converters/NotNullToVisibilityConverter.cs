using System;
using System.Windows;
using System.Windows.Data;

namespace SrkToolkit.Xaml.Converters {

    public class NotNullToVisibilityConverter : IValueConverter {

        #region IValueConverter Members

        /// <summary>
        /// Converts a value to a <see cref="Visibility"/> depending on it's existence (null or not).
        /// If value is null, <see cref="Visibility.Collapsed"/> is returned.
        /// If value is not null, <see cref="Visibility.Visible"/> is returned.
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="targetType"></param>
        /// <param name="parameter">
        /// Reverse the behavior if begins with '!'.
        /// Will return <see cref="Visibility.Hidden"/> instead of <see cref="Visibility.Collapsed"/> if it contains 'H'.
        /// </param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool reverse = false;
            bool isNull = false;
            bool hide = false;
            var vHide = Visibility.Collapsed;

            // parse parameter
            if (parameter is string) {
                string strparam = (string)parameter;
                if (strparam.Length > 0) {
                    reverse = strparam[0] == '!';
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
            if (value is string) {
                if (reverse)
                    return string.IsNullOrEmpty((string)value) ? Visibility.Visible : vHide;
                else
                    return !string.IsNullOrEmpty((string)value) ? Visibility.Visible : vHide;
            } else {
                if (reverse)
                    return value == null ? Visibility.Visible : vHide;
                else
                    return value != null ? Visibility.Visible : vHide;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion

    }
}
