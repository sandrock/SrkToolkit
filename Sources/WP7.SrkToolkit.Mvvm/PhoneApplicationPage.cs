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
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Navigation;

    /// <summary>
    /// Custom page to use with <see cref="InteractionViewModelBase"/>.
    /// </summary>
    [CLSCompliant(false)]
    public class PhoneApplicationPage : Microsoft.Phone.Controls.PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneApplicationPage"/> class.
        /// </summary>
        public PhoneApplicationPage()
            : base()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the current theme is light.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current theme is light; otherwise, <c>false</c>.
        /// </value>
        public bool LightThemeEnabled
        {
            get
            {
                return (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }

        /// <summary>
        /// This method is called when the hardware back key is pressed.
        /// </summary>
        /// <param name="e">Set e.Cancel to true to indicate that the request was handled by the application.</param>
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (this.DataContext != null && this.DataContext is InteractionViewModelBase)
            {
                ((InteractionViewModelBase)this.DataContext).OnBackKeyPress(e);
            }

            base.OnBackKeyPress(e);
        }

        /// <summary>
        /// Called when a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (this.DataContext != null && this.DataContext is InteractionViewModelBase)
            {
                var vm = (InteractionViewModelBase)this.DataContext;
#if !WP80
                vm.VisualStateChangeEvent -= this.OnVisualStateChange;
#endif
                vm.OnNavigatedFrom(e);
                vm.PageState = null;
            }

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.DataContext != null && this.DataContext is InteractionViewModelBase)
            {
                var vm = (InteractionViewModelBase)this.DataContext;
#if !WP80
                vm.VisualStateChangeEvent += this.OnVisualStateChange;
#endif
                vm.PageState = this.State;
                vm.OnNavigatedTo(e, NavigationContext, NavigationService);
            }
        }

        /// <summary>
        /// Called when navigating to a fragment on a page.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        [CLSCompliant(false)]
        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            base.OnFragmentNavigation(e);

            if (this.DataContext != null && this.DataContext is InteractionViewModelBase)
            {
                var vm = (InteractionViewModelBase)this.DataContext;
                vm.PageState = this.State;
                vm.OnFragmentNavigation(e);
            }
        }

        /// <summary>
        /// Called just before a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (this.DataContext != null && this.DataContext is InteractionViewModelBase)
            {
                var vm = (InteractionViewModelBase)this.DataContext;
                ////vm.PageState = this.State;
                vm.OnNavigatingFrom(e);
            }

            base.OnNavigatingFrom(e);
        }

#if !WP80
        /// <summary>
        /// Called when a visual state changeis commanded from the viewmodel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SrkToolkit.Mvvm.VisualStateChangeEventArgs"/> instance containing the event data.</param>
        protected virtual void OnVisualStateChange(object sender, VisualStateChangeEventArgs e)
        {
            e.Succeed = VisualStateManager.GoToState(this, e.StateName, e.UseTransitions);
        }
#endif
    }
}
