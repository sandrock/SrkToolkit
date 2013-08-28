
namespace SrkToolkit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    /// <summary>
    /// A disposable object that will cann a delegate on disposal.
    /// </summary>
    public sealed class DisposableOnce : IDisposable
    {
        private readonly object sync = new object();
        private bool isDisposed;
        private Action onDisposed1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableOnce"/> class.
        /// </summary>
        /// <param name="onDisposed">The configuration disposed.</param>
        /// <exception cref="System.ArgumentNullException">The delegate to call on dispose</exception>
        public DisposableOnce(Action onDisposed)
        {
            if (onDisposed == null)
                throw new ArgumentNullException("onDisposed");

            this.onDisposed1 = onDisposed;
            GC.SuppressFinalize(this);
        }

        ~DisposableOnce()
        {
            lock (this.sync)
            {
                this.onDisposed1 = null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            lock (this.sync)
            {
                if (!this.isDisposed)
                {
                    this.isDisposed = true;

                    if (disposing)
                    {
                        if (this.onDisposed1 != null)
                        {
                            try
                            {
                                this.onDisposed1();
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("DisposableOnce caught exception in dispose callback" + Environment.NewLine + ex.ToString());
                                throw;
                            }

                            this.onDisposed1 = null;
                        }
                    }
                } 
            }
        }
    }
}
