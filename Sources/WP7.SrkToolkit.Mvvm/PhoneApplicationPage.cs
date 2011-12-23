using System.Windows.Navigation;
using SrkToolkit.Mvvm;
using System.Windows;

namespace SrkToolkit.Mvvm {

    /// <summary>
    /// Custom page to use with <see cref="CommonViewModel"/>.
    /// </summary>
    public class PhoneApplicationPage : Microsoft.Phone.Controls.PhoneApplicationPage {

        public PhoneApplicationPage() : base() {
            
        }

        public bool LightThemeEnabled {
            get {
                return (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            if (DataContext != null && DataContext is InteractionViewModelBase)
            {
                ((InteractionViewModelBase)DataContext).OnNavigatedFrom(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if (DataContext != null && DataContext is InteractionViewModelBase)
            {
                ((InteractionViewModelBase)DataContext).OnNavigatedTo(e, NavigationContext, NavigationService);
            }
        }

    }
}
