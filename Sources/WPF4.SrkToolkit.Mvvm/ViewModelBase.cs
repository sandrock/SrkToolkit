
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
