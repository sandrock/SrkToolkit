
namespace SrkToolkit.Mvvm
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Threading;

    partial class ViewModelBase
    {

        #region Threading

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// Make sure you instantiate this in the UI thread so that the dispatcher can attach.
        /// </summary>
        protected ViewModelBase()
        {
            this.Dispatcher = Dispatcher.CurrentDispatcher;
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
            if (this.Dispatcher != null && !this.Disposed && action != null)
                this.Dispatcher.BeginInvoke(action, DispatcherPriority.Background, null);
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
            if (this.Dispatcher != null && !this.Disposed && action != null)
                this.Dispatcher.BeginInvoke(action, DispatcherPriority.ApplicationIdle, null);
        }

        #endregion

        #region Is in design mode awareness

        /// <summary>
        /// Gets a value indicating whether this instance is in design mode.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is in design mode; otherwise, <c>false</c>.
        /// </value>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "The security risk here is neglectible.")]
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = new bool?((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue);
                    if (!(_isInDesignMode.Value || !Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal)))
                    {
                        _isInDesignMode = true;
                    }
                }
                return _isInDesignMode.Value;
            }
        }

        #endregion
    }
}
