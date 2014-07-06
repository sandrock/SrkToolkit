
namespace SrkToolkit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Holds a list of disposable object for mass Dispose()ition.
    /// Not thread-safe.
    /// </summary>
    public sealed class CompositeDisposable : IDisposable
    {
        private readonly List<WeakReference> collection = new List<WeakReference>();

        /// <summary>
        /// Adds the specified disposable and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="disposable">The disposable.</param>
        /// <returns></returns>
        public T Add<T>(T disposable)
            where T : IDisposable
        {
            if (disposable != null)
                this.collection.Add(new WeakReference(disposable));
            return disposable;
        }

        /// <summary>
        /// Adds the specified disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        public void Add(IDisposable disposable)
        {
            if (disposable != null)
                this.collection.Add(new WeakReference(disposable));
        }

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                var copy = this.collection.ToList();
                this.collection.Clear();

                copy.ForEach(i =>
                {
                    if (i != null && i.IsAlive)
                    {
                        try
                        {
                            ((IDisposable)i.Target).Dispose();
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError("CompositeDisposable failed to dispose object" + Environment.NewLine + ex.ToString());
                            if (ex.IsFatal())
                                throw;
                        }
                    }
                });
            }
        }

        #endregion
    }
}
