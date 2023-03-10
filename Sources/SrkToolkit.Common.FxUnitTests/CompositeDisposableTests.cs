
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class CompositeDisposableTests
    {
        public class AddMethod
        {
            [Fact]
            public void IsFluent()
            {
                // prepare
                var disposable = new Disposable();
                var target = new CompositeDisposable();
                object expected = disposable;

                // execute
                object actual = target.Add(disposable);

                // verify
                Assert.Same(expected, actual);
            }

            [Fact]
            public void DisposesChildren()
            {
                // prepare
                var disposable = new Disposable();
                var target = new CompositeDisposable();

                // execute
                target.Add(disposable);
                target.Dispose();

                // verify
                Assert.True(disposable.IsDisposed);
            }

            [Fact]
            public void MultipleDisposeDoNotThrow()
            {
                // prepare
                var disposable = new Disposable();
                var target = new CompositeDisposable();

                // execute
                target.Add(disposable);
                target.Dispose();
                target.Dispose();
                target.Dispose();
            }

            [Fact]
            public void DisposeAndForget()
            {
                // prepare
                var disposable = new Disposable();
                var target = new CompositeDisposable();

                // execute
                target.Add(disposable);
                target.Dispose();
                Trace.Assert(disposable.IsDisposed);
                disposable.IsDisposed = false;

                target.Dispose();

                // verify
                Assert.False(disposable.IsDisposed);
            }

            [Fact]
            public void IgnoresExceptions()
            {
                // prepare
                var disposable = new Disposable();
                disposable.ThrowOnDispose = true;
                var target = new CompositeDisposable();

                // execute
                target.Add(disposable);
                target.Dispose();
            }
        }

        public class Disposable : IDisposable
        {
            private bool isDisposed;

            public bool IsDisposed
            {
                get { return this.isDisposed; }
                set { this.isDisposed = value; }
            }

            public bool ThrowOnDispose { get; set; }

            #region IDisposable members

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
                        if (this.ThrowOnDispose)
                            throw new InvalidOperationException("Object already disposed");
                    }

                    this.isDisposed = true;
                }
            }

            #endregion
        }
    }
}
