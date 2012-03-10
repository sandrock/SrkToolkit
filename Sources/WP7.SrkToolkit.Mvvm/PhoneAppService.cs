using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SrkToolkit.Mvvm
{
    /// <summary>
    /// Windows Phone application related services.
    /// </summary>
    public class PhoneAppService
    {
        /// <summary>
        /// Gets the navigation state.
        /// </summary>
        /// <value>
        /// The navigation state.
        /// </value>
        public static PhoneAppNavigationState NavigationState { get; private set; }

        /// <summary>
        /// Setups the NavigationState service.
        /// To be called one time at the end of the App class contrustor.
        /// </summary>
        public static void Setup()
        {
            Setup(Application.Current);
        }

        /// <summary>
        /// Setups the NavigationState service.
        /// To be called one time at the end of the App class contrustor.
        /// </summary>
        public static void Setup(Application application)
        {
            application.Exit += OnAppExit;
            application.Startup += OnAppStartup;

            var appService = PhoneApplicationService.Current;
            if (appService != null)
            {
                appService.Activated += OnAppActivated;
                appService.Deactivated += OnAppDeactivated;
                appService.Launching += OnAppLaunching;
                appService.Closing += OnAppClosing;
            }

            var frame = application.RootVisual as PhoneApplicationFrame;
            if (frame != null)
            {
                frame.Navigated += OnFrameNavigated;
            }
        }

        static void OnAppStartup(object sender, StartupEventArgs e)
        {
        }

        static void OnAppLaunching(object sender, LaunchingEventArgs e)
        {
            NavigationState = PhoneAppNavigationState.Starting;
        }

        static void OnAppActivated(object sender, ActivatedEventArgs e)
        {
            NavigationState = PhoneAppNavigationState.Activating;
        }

        static void OnAppDeactivated(object sender, DeactivatedEventArgs e)
        {
            NavigationState = PhoneAppNavigationState.Deactivated;
        }

        static void OnAppClosing(object sender, ClosingEventArgs e)
        {
            NavigationState = PhoneAppNavigationState.Closed;
        }

        static void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (NavigationState == PhoneAppNavigationState.Activated)
                NavigationState = PhoneAppNavigationState.Internal;
            else if (NavigationState == PhoneAppNavigationState.Activating)
                NavigationState = PhoneAppNavigationState.Activated;
            else if (NavigationState == PhoneAppNavigationState.Starting)
                NavigationState = PhoneAppNavigationState.Started;
            else
                NavigationState = PhoneAppNavigationState.Internal;
        }

        static void OnAppExit(object sender, EventArgs e)
        {
        }
    }
}
