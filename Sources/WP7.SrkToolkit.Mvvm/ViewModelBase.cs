using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;
using System.Windows;

namespace SrkToolkit.Mvvm {

    partial class ViewModelBase {

        #region Threading

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// Make sure you instantiate this in the UI thread so that the dispatcher can attach.
        /// </summary>
        protected ViewModelBase() {
            if (Deployment.Current != null) {
                this.Dispatcher = Deployment.Current.Dispatcher;
            }
        }

        #endregion

        #region Is in design mode awareness

        /// <summary>
        /// Gets a value indicating whether is in design mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if is in design mode static; otherwise, <c>false</c>.
        /// </value>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "The security risk here is neglectible.")]
        public static bool IsInDesignModeStatic {
            get {
                if (!_isInDesignMode.HasValue) {
                    _isInDesignMode = new bool?(DesignerProperties.IsInDesignTool);
                }
                return _isInDesignMode.Value;
            }
        }

        #endregion

    }
}
