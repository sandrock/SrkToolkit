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
        [CLSCompliant(false)]
        private NavigationService navigationService;
        
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
                if (this._navigateCommand == null)
                {
                    if (this.baseViewModel != null && this.baseViewModel._navigateCommand != null)
                        return this.baseViewModel._navigateCommand;

                    this._navigateCommand = new RelayCommand<object>(this.OnNavigate);
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
        /// Navigates to the most recent entry in the back navigation history.
        /// </summary>
        /// <param name="fallbackUri">
        ///   if going back is not allowed, this param will be used to navigate somewhere else.
        /// </param>
        /// <example>
        ///   this.NavigateBack("/Pages/MainPage.xaml");
        /// </example>
        protected void NavigateBack(string fallbackUri) {
            if (this.NavigationService.CanGoBack) {
                this.NavigationService.GoBack();
            } else if (fallbackUri != null) {
                if (fallbackUri == null)
                    throw new ArgumentNullException("fallbackUri");

                this.NavigateBack(new Uri(fallbackUri, UriKind.Relative));
            }
        }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history.
        /// </summary>
        /// <param name="fallbackUri">
        ///   if going back is not allowed, this param will be used to navigate somewhere else.
        /// </param>
        protected void NavigateBack(Uri fallbackUri)
        {
            if (this.NavigationService.CanGoBack) {
                this.NavigationService.GoBack();
            } else if (fallbackUri != null) {
                if (fallbackUri == null)
                    throw new ArgumentNullException("fallbackUri");

                this.NavigationService.Navigate(fallbackUri);
            }
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
                    if (this.baseViewModel != null && this.baseViewModel._linkCommand != null)
                        return this.baseViewModel._linkCommand;

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
        protected NavigationService NavigationService
        {
            get { return this.navigationService ?? (this.baseViewModel != null && this.baseViewModel.navigationService != null ? this.baseViewModel.navigationService : null); }
            private set { this.navigationService = value; }
        }

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
                return this._isPageDeactivated;
            }
            set { this._isPageDeactivated = value; }
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
            this.NavigationService = navigationService;
            this.IsPageDeactivated = false;
        }

        /// <summary>
        /// Called just before a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        internal protected void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when navigating to a fragment on a page.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        [CLSCompliant(false)]
        internal protected void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        /// <summary>
        /// This method is called when the hardware back key is pressed.
        /// </summary>
        /// <param name="e">Set e.Cancel to true to indicate that the request was handled by the application.</param>
        internal protected void OnBackKeyPress(CancelEventArgs e)
        {
        }

        #endregion

        #region VisualStateManager

        /// <summary>
        /// Occurs when a visual state change is commanded.
        /// </summary>
        public event EventHandler<VisualStateChangeEventArgs> VisualStateChangeEvent;

        /// <summary>
        /// Commands a visual state change.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <param name="useTransitions">if set to <c>true</c> use transitions.</param>
        /// <returns>
        ///   <b>true</b> is the transition succeed; otherwise, <b>false</b>
        /// </returns>
        protected bool GoToVisualState(string stateName, bool useTransitions = true)
        {
            var args = new VisualStateChangeEventArgs(stateName, useTransitions);
            var handler = this.VisualStateChangeEvent;
            if (handler != null)
                handler(this, args);

            return args.Succeed;
        }

        #endregion
    }
}
