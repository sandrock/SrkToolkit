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

namespace SrkToolkit.Mvvm
{
    using System;
    using System.Windows;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

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
