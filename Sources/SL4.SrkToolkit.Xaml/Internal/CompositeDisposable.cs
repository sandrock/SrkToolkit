
namespace SrkToolkit.Xaml.Internal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Holds a list of disposable object for mass Dispose()ition.
    /// Not thread-safe.
    /// </summary>
    internal sealed class CompositeDisposable : IDisposable
    {
        private readonly List<WeakReference> collection = new List<WeakReference>();
        private bool disposed;

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
        /// <typeparam name="T"></typeparam>
        /// <param name="disposable">The disposable.</param>
        public void Add<T>(IDisposable disposable)
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
                this.collection.ForEach(i =>
                {
                    if (i != null && i.IsAlive)
                        ((IDisposable)i.Target).Dispose();
                });
                this.collection.Clear();
            }
        }

        #endregion
    }
}
