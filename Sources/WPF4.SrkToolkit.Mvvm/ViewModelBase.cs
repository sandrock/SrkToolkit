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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
#if SILVERLIGHT || WPF
    using System.Windows.Threading;
#elif UWP
    using Windows.ApplicationModel;
#endif

    partial class ViewModelBase
    {

        #region Threading

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// Make sure you instantiate this in the UI thread so that the dispatcher can attach.
        /// </summary>
        protected ViewModelBase()
        {
#if SILVERLIGHT || WPF
            this.Dispatcher = Dispatcher.CurrentDispatcher;
#endif
        }

        /// <summary>
        /// Executes the specified delegate asynchronously on the thread the <see cref="Dispatcher"/> is associated with at a <see cref="DispatcherPriority.Background"/> priority.
        /// Will do nothing if <see cref="ViewModelBase.Dispatcher"/> is null or <see cref="ViewModelBase.Disposed"/> is true.
        /// </summary>
        /// <param name="action">
        /// A delegate to a method that takes no arguments and does not return a value, which is pushed onto the <see cref="Dispatcher"/> event queue.
        /// </param>
        [Obsolete("Use the Dispatcher property instead")]
        protected void DispatchBackground(Action action)
        {
#if SILVERLIGHT || WPF
            if (this.Dispatcher != null && !this.Disposed && action != null)
                this.Dispatcher.BeginInvoke(action, DispatcherPriority.Background, null);
#endif
        }

        /// <summary>
        /// Executes the specified delegate asynchronously on the thread the <see cref="Dispatcher"/> is associated with at a <see cref="DispatcherPriority.ApplicationIdle"/> priority.
        /// Will do nothing if <see cref="ViewModelBase.Dispatcher"/> is null or <see cref="ViewModelBase.Disposed"/> is true.
        /// </summary>
        /// <param name="action">
        /// A delegate to a method that takes no arguments and does not return a value, which is pushed onto the <see cref="Dispatcher"/> event queue.
        /// </param>
        [Obsolete("Use the Dispatcher property instead")]
        protected void DispatchApplicationIdle(Action action)
        {
#if SILVERLIGHT || WPF
            if (this.Dispatcher != null && !this.Disposed && action != null)
                this.Dispatcher.BeginInvoke(action, DispatcherPriority.ApplicationIdle, null);
#endif
        }

        #endregion

        #region Is in design mode awareness

        #endregion
    }
}
