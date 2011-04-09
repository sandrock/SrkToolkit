using System;
using System.Windows;
using System.Windows.Data;

namespace SrkToolkit.Xaml.Converters {

    public class OppositeBoolConverter : IValueConverter {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion

    }
}
