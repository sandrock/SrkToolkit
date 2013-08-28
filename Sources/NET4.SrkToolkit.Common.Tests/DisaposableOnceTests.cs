
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading;

    [TestClass]
    public class DisaposableOnceTests
    {
        [TestMethod]
        public void DelegateIsCalledOnceDispose()
        {
            // prepare
            int disposed = 0;
            var target = new DisposableOnce(() => disposed++);

            // execute
            target.Dispose();

            // verify
            Assert.AreEqual(1, disposed);
        }

        [TestMethod]
        public void GarbageCollectorDoesNotCallDelegate()
        {
            // prepare
            int disposed = 0;
            var target = new WeakReference(new DisposableOnce(() => disposed++));

            // execute
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // verify
            Assert.IsFalse(target.IsAlive);
            Assert.AreEqual(0, disposed);
        }
        
        /// <summary>
        /// Verify...
        /// </summary>
        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void DisposeRethrowsOnDelegateInvocationException()
        {
            // prepare
            var target = new DisposableOnce(() =>
            {
                throw new InvalidOperationException();
            });

            // execute
            target.Dispose();
        }
    }
}
