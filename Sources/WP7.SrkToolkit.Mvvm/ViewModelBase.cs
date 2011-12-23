using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;
using System.Windows;

namespace SrkToolkit.Mvvm {

    partial class ViewModelBase {

        #region Threading

        static ViewModelBase() {
            if (Deployment.Current != null) {
                PresentationDispatcher = Deployment.Current.Dispatcher;
            }
        }

        #endregion

        #region Is in design mode awareness

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
