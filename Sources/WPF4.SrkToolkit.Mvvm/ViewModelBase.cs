using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace SrkToolkit.Mvvm {

    partial class ViewModelBase {

        #region Threading

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// Make sure you instantiate this in the UI thread so that the dispatcher can attach.
        /// </summary>
        protected ViewModelBase() {
            this.Dispatcher = Dispatcher.CurrentDispatcher;
        }

        protected void DispatchBackground(Action action) {
            this.Dispatcher.BeginInvoke(action, DispatcherPriority.Background, null);
        }

        protected void DispatchApplicationIdle(Action action) {
            this.Dispatcher.BeginInvoke(action, DispatcherPriority.ApplicationIdle, null);
        }

        #endregion

        #region Is in design mode awareness

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "The security risk here is neglectible.")]
        public static bool IsInDesignModeStatic {
            get {
                if (!_isInDesignMode.HasValue) {
                    _isInDesignMode = new bool?((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue);
                    if (!(_isInDesignMode.Value || !Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))) {
                        _isInDesignMode = true;
                    }
                }
                return _isInDesignMode.Value;
            }
        }

        #endregion

    }
}
