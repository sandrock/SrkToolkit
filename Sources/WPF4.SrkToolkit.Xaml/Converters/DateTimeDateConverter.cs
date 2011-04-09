using System;
using System.Windows;
using System.Windows.Data;

namespace SrkToolkit.Xaml.Converters {

    public class DateTimeDateConverter : IValueConverter {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            DateTime? val = null;
            if (value is DateTime?) {
                val = (DateTime?)value;
            }
            if (value is DateTime) {
                DateTime val2 = (DateTime)value;
                val = val2;
            }
            if (val.HasValue) {
                return val.Value.ToLongDateString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion

    }
}
