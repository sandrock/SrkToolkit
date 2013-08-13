
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Diagnostics;

    public class CompositeDisposableTests
    {
        [TestClass]
        public class AddMethod
        {
            [TestMethod]
            public void IsFluent()
            {
                // prepare
                var disposable = new Disposable();
                var target = new CompositeDisposable();
                object expected = disposable;

                // execute
                object actual = target.Add(disposable);

                // verify
                Assert.AreSame(expected, actual);
            }

            [TestMethod]
            public void DisposesChildren()
            {
                // prepare
                var disposable = new Disposable();
                var target = new CompositeDisposable();

                // execute
                target.Add(disposable);
                target.Dispose();

                // verify
                Assert.IsTrue(disposable.IsDisposed);
            }

            [TestMethod]
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

            [TestMethod]
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
                Assert.IsFalse(disposable.IsDisposed);
            }

            [TestMethod]
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
