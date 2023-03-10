
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Xunit;

    public class DisaposableOnceTests
    {
        [Fact]
        public void DelegateIsCalledOnceDispose()
        {
            // prepare
            int disposed = 0;
            var target = new DisposableOnce(() => disposed++);

            // execute
            target.Dispose();

            // verify
            Assert.Equal(1, disposed);
        }

        [Fact]
        public void GarbageCollectorDoesNotCallDelegate()
        {
            // prepare
            int disposed = 0;
            var target = new WeakReference(new DisposableOnce(() => disposed++));

            // execute
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // verify
            Assert.False(target.IsAlive);
            Assert.Equal(0, disposed);
        }
        
        /// <summary>
        /// Verify...
        /// </summary>
        [Fact]
        public void DisposeRethrowsOnDelegateInvocationException()
        {
            // prepare
            var target = new DisposableOnce(() =>
            {
                throw new InvalidOperationException();
            });

            // execute
            Assert.Throws<InvalidOperationException>(() =>
            {
                target.Dispose();
            });
        }
    }
}
