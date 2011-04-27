using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SrkToolkit.Base;
using System.Windows.Threading;
using SrkToolkit.Mvvm.Tools;

namespace SrkToolkit.Mvvm {

    /// <summary>
    /// A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    public partial class ViewModelBase : INotifyPropertyChanged, IDisposable, ICleanup {

        #region Threading

        private static Dispatcher PresentationDispatcher;

        protected void Dispatch(Action action) {
            PresentationDispatcher.BeginInvoke(action, null);
        }

        #endregion

        #region Is in design mode awareness

        private static bool? _isInDesignMode;

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Non static member needed for data binding")]
        public bool IsInDesignMode {
            get {
                return IsInDesignModeStatic;
            }
        }

        #endregion

        #region Property change notification

        /// <summary>
        /// Change a property's value and notify the view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">a reference to a field</param>
        /// <param name="value">the new value</param>
        /// <param name="propertyName">the public property name for change notification</param>
        /// <returns>returns true if the new value is different from the old one</returns>
        protected bool SetValue<T>(ref T property, T value, string propertyName) {
            if (Object.Equals(property, value)) {
                return false;
            }
            property = value;

            RaisePropertyChanged(propertyName);

            return true;

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

        #region INotifyPropertyChanged Members
#pragma warning disable 1591

        public event PropertyChangedEventHandler PropertyChanged;

#pragma warning restore 1591
        #endregion

        #endregion

        #region Cleanup

        private bool _disposed;
        protected bool disposed {
            get { return _disposed; }
        }

        #region IDisposable Members
#pragma warning disable 1591

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

#pragma warning restore 1591
        #endregion

        #region ICleanup Members

        /// <summary>
        /// Override this method to unload data.
        /// It is called by <see cref="IDisposable.Dispose"/>.
        /// Always call the parent method. 
        /// </summary>
        /// <remarks>
        /// It can be called many times without altering the object.
        /// For navigation apps, can be called by OnNavigatedFrom.
        /// For xaml popups, can be called by OnPopupClosed or OnUnload.
        /// </remarks>
        public virtual void Cleanup() {
            
        }

        #endregion

        /// <summary>
        /// Dispose method to free resources.
        /// The object will not be usable anymore.
        /// It calls <see cref="Cleanup"/> before disposing anything.
        /// Always call the parent method. 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (_disposed)
                return;

            if (disposing) {
                this.Cleanup();
            }
        }

        #endregion

    }
}
