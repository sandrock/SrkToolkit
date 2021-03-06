﻿// 
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
    using System.Collections.Generic;
    using System.Text;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// Custom page to use with <see cref="InteractionViewModelBase"/>.
    /// </summary>
    public class Page : Windows.UI.Xaml.Controls.Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneApplicationPage"/> class.
        /// </summary>
        public Page()
            : base()
        {
            this.DataContextChanged += this.OnDataContextChanged;
            ////Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        private ViewModelBase InternalViewModel
        {
            get { return this.DataContext as ViewModelBase; }
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = this.InternalViewModel;
            if (model != null)
            {
                this.ResolveNavigationServiceIntoModel(model);
            }
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var model = this.InternalViewModel;
            if (model != null)
            {
                var service = model.NavigationService;
                if (service != null)
                {
                    service.Dispose();
                    model.NavigationService = null;
                }
            }
        }

        protected virtual void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var model = args.NewValue as ViewModelBase;
            if (model != null)
            {
                this.ResolveNavigationServiceIntoModel(model);
            }
        }

        private void ResolveNavigationServiceIntoModel(ViewModelBase model)
        {
            DependencyObject parent = this;
            Frame frame = null;
            while (parent != null && frame == null)
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is Frame)
                {
                    frame = (Frame)parent;
                }
            }

            if (frame != null && model != null)
            {
                model.NavigationService = new NavigationService(frame);
            }
        }
    }
}
