using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using SrkToolkit.Mvvm.Commands;
using Microsoft.Phone.Tasks;

namespace SrkToolkit.Mvvm
{
    partial class InteractionViewModelBase
    {

        #region Navigation (internal and web)

        #region Navigate Command

        /// <summary>
        /// Useless thing.
        /// </summary>
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

        protected void NavigateBack()
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
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
        protected NavigationService NavigationService { get; private set; }

        public virtual bool IsPageDeactivated
        {
            get
            {
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached && _isPageDeactivated)
                    System.Diagnostics.Debugger.Break();
#endif
                return _isPageDeactivated;
            }
            set { _isPageDeactivated = value; }
        }
        private bool _isPageDeactivated;


        /// <summary>
        /// Called by the view.
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnNavigatedFrom(NavigationEventArgs e)
        {
            IsPageDeactivated = true;
        }

        /// <summary>
        /// Called by the view.
        /// Set the <see cref="NavigationService"/> property.
        /// Triggers auto-login and session persistance.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="context"></param>
        /// <param name="navigationService"></param>
        public virtual void OnNavigatedTo(NavigationEventArgs e, NavigationContext context, NavigationService navigationService)
        {
            NavigationService = navigationService;
            IsPageDeactivated = false;
        }

        #endregion

    }
}
