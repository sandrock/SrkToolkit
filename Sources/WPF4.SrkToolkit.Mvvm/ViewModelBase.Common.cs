using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;
using SrkToolkit.Mvvm.Tools;

namespace SrkToolkit.Mvvm {

    /// <summary>
    /// A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    public partial class ViewModelBase : INotifyPropertyChanged, IDisposable {
        private bool _disposed;

        #region Threading

        private Dispatcher Dispatcher;

        /// <summary>
        /// Executes the specified delegate asynchronously on the thread the <see cref="Dispatcher"/> is associated with.
        /// </summary>
        /// <param name="action">
        /// A delegate to a method that takes no arguments and does not return a value, which is pushed onto the <see cref="Dispatcher"/> event queue.
        /// </param>
        protected void Dispatch(Action action) {
            Dispatcher.BeginInvoke(action, null);
        }

        #endregion

        #region Is in design mode awareness

        private static bool? _isInDesignMode;

        /// <summary>
        /// Gets a value indicating whether is in design mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if is in design mode static; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Verifies the name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
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

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        protected bool Disposed {
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

        /// <summary>
        /// Dispose method to free resources.
        /// The object will not be usable anymore.
        /// Always call the parent method. 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            this._disposed = true;
        }

        #endregion
    }
}
