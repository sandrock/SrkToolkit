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

namespace SrkToolkit.Xaml.Converters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts a boolean to a <see cref="Visibility"/>.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
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
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "CA is wrong")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool reverse = false;
            bool val = false;
            bool showIfNull = false;
            bool hide = false;
            var valueForHide = Visibility.Collapsed;

            // parse parameter
            if (parameter is string)
            {
                string strparam = (string)parameter;
                if (strparam.Length > 0)
                {
                    reverse = strparam[0] == '!';
                    showIfNull = strparam.IndexOf('N') >= 0;
                    hide = strparam.IndexOf('H') >= 0;
                }
                else
                {
                    reverse = false;
                }
            }
            else if (parameter is bool)
            {
                reverse = (bool)parameter;
            }
            else if (parameter is bool?)
            {
                bool? p = ((bool?)parameter);
                reverse = p.HasValue && p.Value;
            }

#if !SILVERLIGHT
            // Hidden instead of Collapsed?
            // Silverlight doesn't support this :)
            if (hide)
                valueForHide = Visibility.Hidden;
#endif

            // parse value
            if (value == null)
            {
                return showIfNull ? Visibility.Visible : valueForHide;
            }
            else if (value is string)
            {
                val = bool.Parse((string)value);
            }
            else if (value is bool)
            {
                val = (bool)value;
            }
            else if (value is bool?)
            {
                bool? p = (bool?)value;
                if (p.HasValue)
                    val = p.Value;
                else
                    return showIfNull ? Visibility.Visible : valueForHide;
            }

            if (reverse)
                return !val ? Visibility.Visible : valueForHide;
            else
                return val ? Visibility.Visible : valueForHide;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
