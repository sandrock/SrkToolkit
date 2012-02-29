using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using SrkToolkit.Mvvm.Commands;

namespace SrkToolkit.Mvvm
{
    partial class InteractionViewModelBase
    {

        #region Phone stuff

        #region Navigate Command

        /// <summary>
        /// Useless thing.
        /// </summary>
        [CLSCompliant(false)]
        protected static PhoneApplicationFrame RootVisual
        {
            get
            {
                return Application.Current.RootVisual as PhoneApplicationFrame;
            }
        }

        /// <summary>
        /// Navigate to any page in the application.
        /// To be bound in the view.
        /// You can pass a string or a Uri as parameter.
        /// </summary>
        public ICommand NavigateCommand
        {
            get
            {
                if (_navigateCommand == null)
                {
                    _navigateCommand = new RelayCommand<object>(OnNavigate);
                }
                return _navigateCommand;
            }
        }
        private ICommand _navigateCommand;

        private void OnNavigate(object param)
        {
            Uri uri = null;
            if (param is string)
            {
                uri = new Uri((string)param, UriKind.Relative);
            }
            if (param is Uri)
                uri = (Uri)param;
            if (uri != null)
            {
                RootVisual.Navigate(uri);
            }
        }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or throws an exception if no entry exists in back navigation.
        /// </summary>
        /// <param name="fallbackUri">
        ///   if going back is not allowed, this param will be used to navigate somewhere else.
        /// </param>
        protected void NavigateBack(string fallbackUri = "/MainPage.xaml")
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else if (fallbackUri != null)
                NavigationService.Navigate(new Uri(fallbackUri, UriKind.Relative));
        }

        #endregion

        #region Browser link

        /// <summary>
        /// Web browser link.
        /// To be bound in the view.
        /// </summary>
        public ICommand WebLinkCommand
        {
            get
            {
                if (_linkCommand == null)
                {
                    _linkCommand = new RelayCommand<object>(OnWebLinkPrivate);
                }
                return _linkCommand;
            }
        }
        private ICommand _linkCommand;

        private void OnWebLinkPrivate(object param)
        {
            Uri uri = null;
            if (param is Uri)
                uri = (Uri)param;
            if (param is string)
                uri = new Uri((string)param, UriKind.Absolute);

            if (OnWebLink(uri))
            {
                var webTask = new WebBrowserTask();
                webTask.URL = uri.OriginalString;
                webTask.Show();
            }
        }

        /// <summary>
        /// Called when the <see cref="WebLinkCommand"/> is executed.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// if the returned value is true, then a <see cref="WebBrowserTask"/> will be xecuted for the specified url.
        /// </returns>
        protected virtual bool OnWebLink(Uri url)
        {
            return true;
        }

        #endregion

        #endregion

        #region View events

        /// <summary>
        /// Nice for navigation.
        /// </summary>
        [CLSCompliant(false)]
        protected NavigationService NavigationService { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page is deactivated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is page deactivated; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsPageDeactivated
        {
            get
            {
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached && _isPageDeactivated)
                    System.Diagnostics.Debugger.Break(); // breaking here means a page still works even if no more displayed
#endif
                return _isPageDeactivated;
            }
            set { _isPageDeactivated = value; }
        }
        private bool _isPageDeactivated;

        /// <summary>
        /// Called when a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        internal protected virtual void OnNavigatedFrom(NavigationEventArgs e)
        {
            IsPageDeactivated = true;
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// Set the <see cref="NavigationService"/> property.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        /// <param name="context">The navigation context.</param>
        /// <param name="navigationService">The navigation service.</param>
        [CLSCompliant(false)]
        internal protected virtual void OnNavigatedTo(NavigationEventArgs e, NavigationContext context, NavigationService navigationService)
        {
            NavigationService = navigationService;
            IsPageDeactivated = false;
        }

        /// <summary>
        /// Called just before a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        internal protected void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when navigating to a fragment on a page.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        [CLSCompliant(false)]
        internal protected void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is called when the hardware back key is pressed.
        /// </summary>
        /// <param name="e">Set e.Cancel to true to indicate that the request was handled by the application.</param>
        internal protected void OnBackKeyPress(CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
