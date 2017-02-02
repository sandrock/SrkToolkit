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
    using SrkToolkit.Mvvm.Tools;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
#if SILVERLIGHT || WPF
    using System.Windows;
    using System.Windows.Threading;
#elif UWP
    using Windows.ApplicationModel;
#endif
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    public partial class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        private bool _disposed;

        #region Threading

#if SILVERLIGHT || WPF
        /// <summary>
        /// Contains the UI Dispatcher.
        /// Use the property <see cref="Dispatcher"/> instead.
        /// </summary>
        private Dispatcher dispatcher;

        /// <summary>
        /// Gets or sets the UI Dispatcher.
        /// </summary>
        public Dispatcher Dispatcher
        {
            get { return this.dispatcher; }
            set { this.dispatcher = value; }
        }

        /// <summary>
        /// Executes the specified delegate asynchronously on the thread the <see cref="Dispatcher"/> is associated with.
        /// Will do nothing if <see cref="ViewModelBase.Dispatcher"/> is null or <see cref="ViewModelBase.Disposed"/> is true.
        /// </summary>
        /// <param name="action">
        /// A delegate to a method that takes no arguments and does not return a value, which is pushed onto the <see cref="Dispatcher"/> event queue.
        /// </param>
        protected internal void Dispatch(Action action)
        {
            ////if (this.Disposed)
            ////    throw new ObjectDisposedException(this.GetType().Name);
            ////if (this.Dispatcher == null)
            ////    throw new ArgumentException("Dispatcher is not set");

            if (this.Dispatcher != null && !this.Disposed && action != null)
                this.Dispatcher.BeginInvoke(action, null);
        }

#endif

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
        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }

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
#if WPF
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = new bool?((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue);
                    if (!(_isInDesignMode.Value || !Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal)))
                    {
                        _isInDesignMode = true;
                    }
                }
#elif SILVERLIGHT
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
                }
#elif UWP
                _isInDesignMode = DesignMode.DesignModeEnabled;
#else
                throw new NotSupportedException();
#endif
                return _isInDesignMode.Value;
            }
        }

        #endregion

        #region Property change notification

        /// <summary>
        /// Changes a property's value and notifies the view.
        /// </summary>
        /// <typeparam name="T">the property type</typeparam>
        /// <param name="property">a reference to a field</param>
        /// <param name="value">the new value</param>
        /// <param name="propertyName">the public property name for change notification</param>
        /// <returns>
        /// returns true if the new value is different from the old one
        /// </returns>
        protected bool SetValue<T>(ref T property, T value, string propertyName)
        {
            if (Object.Equals(property, value))
                return false;
            property = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Verifies the name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [Conditional("DEBUG"), DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
#if SILVERLIGHT || WPF
            if (base.GetType().GetProperty(propertyName) == null)
            {
                throw new ArgumentException("Property'" + propertyName + "' not found on " + this.GetType().FullName, propertyName);
            }
#elif UWP
            if (base.GetType().GetTypeInfo().GetDeclaredProperty(propertyName) == null)
            {
                throw new ArgumentException("Property'" + propertyName + "' not found on " + this.GetType().FullName, propertyName);
            }
#endif
        }

        #region INotifyPropertyChanged Members
#pragma warning disable 1591

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
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
        protected bool Disposed
        {
            get { return this._disposed; }
        }

        #region IDisposable Members
#pragma warning disable 1591

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

#pragma warning restore 1591
        #endregion

        /// <summary>
        /// Dispose method to free resources.
        /// The object will not be usable anymore.
        /// Always call the parent method.
        /// Clears the <see cref="ViewModelBase.Dispatcher"/>.
        /// Clears the <see cref="ViewModelBase.PropertyChanged"/> event handler.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.PropertyChanged = null;
            }

            this._disposed = true;
        }

        #endregion
    }
}
