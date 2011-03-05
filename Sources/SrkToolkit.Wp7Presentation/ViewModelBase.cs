using System;
using System.ComponentModel;
using SrkToolkit.Base;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace SrkToolkit.Wp7Presentation {
    public class ViewModelBase : INotifyPropertyChanged, IDisposable, ICleanup {

        private static bool? _isInDesignMode;

        public void Dispose() {
            this.Dispose(true);
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged(string propertyName) {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null) {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Conditional("DEBUG"), DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName) {
            if (base.GetType().GetProperty(propertyName) == null) {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Non static member needed for data binding")]
        public bool IsInDesignMode {
            get {
                return IsInDesignModeStatic;
            }
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "The security risk here is neglectible.")]
        public static bool IsInDesignModeStatic {
            get {
                if (!_isInDesignMode.HasValue) {
                    _isInDesignMode = new bool?(DesignerProperties.IsInDesignTool);
                }
                return _isInDesignMode.Value;
            }
        }

        #region Cleanup

        [Obsolete("IDisposable is obsolete, use ICleanup.Cleanup instead.")]
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                this.Cleanup();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IDisposable Members

        public void Dispose() {
            this.Dispose(true);
        }

        #endregion

        #region ICleanup Members

        public virtual void Cleanup() {
            
        }

        #endregion
    }
}
