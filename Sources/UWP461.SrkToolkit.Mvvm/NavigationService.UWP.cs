
namespace SrkToolkit.Mvvm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Controls;

    public class NavigationService : IDisposable
    {
        private Frame frame;
        private bool isDisposed;

        public NavigationService(Frame frame)
        {
            this.frame = frame;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.frame = null;
                }

                this.isDisposed = true;
            }
        }
    }
}
