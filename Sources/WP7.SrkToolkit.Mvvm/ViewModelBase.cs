
namespace SrkToolkit.Mvvm
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Threading;
    using System.Windows;

    partial class ViewModelBase
    {

        #region Threading

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// Make sure you instantiate this in the UI thread so that the dispatcher can attach.
        /// </summary>
        protected ViewModelBase()
        {
            if (Deployment.Current != null)
            {
                this.Dispatcher = Deployment.Current.Dispatcher;
            }
        }

        #endregion
    }
}
