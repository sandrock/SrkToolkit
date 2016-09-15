namespace SrkToolkit.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Should;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    [TestClass]
    public partial class RetryLogicTests
    {
        [TestMethod]
        public void Action_Should_Be_Tried_3Times_With_ProgressiveInterval()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            RetryLogic
                .Do(this.ActionThrowException)
                .Handle<FileNotFoundException>()
                .AtMost(5)
                .WithProgressiveInterval(TimeSpan.FromMilliseconds(200),2)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .Run();

            watch.Stop();

            watch.Elapsed.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(2600));
            exceptionCount.ShouldEqual(5);
            failureCount.ShouldEqual(1);
            successCount.ShouldEqual(0);
        }

        [TestMethod]
        public void Function_Should_Be_Tried_3Times_With_ExponentialInterval()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            RetryLogic
                .Do<int>(this.ThrowException)
                .Handle<FileNotFoundException>()
                .AtMost(4)
                .WithExponentialInterval(TimeSpan.FromMilliseconds(2000),2)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .Run();

            watch.Stop();

            watch.Elapsed.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(12000));
            exceptionCount.ShouldEqual(4);
            failureCount.ShouldEqual(1);
            successCount.ShouldEqual(0);
        }

        [TestMethod]
        public void Function_Should_Be_A_Success()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            RetryLogic
                .Do<int>(this.NoException)
                .Handle<FileNotFoundException>()
                .AtMost(3)
                .WithConstantInterval(TimeSpan.FromMilliseconds(2000))
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .Run();

            watch.Stop();

            exceptionCount.ShouldEqual(0);
            failureCount.ShouldEqual(0);
            successCount.ShouldEqual(1);
        }

        [TestMethod]
        public void Action_Should_Be_A_Success()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            RetryLogic
                .Do(this.ActionNoException)
                .Handle<FileNotFoundException>()
                .AtMost(3)
                .WithConstantInterval(TimeSpan.FromMilliseconds(2000))
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .Run();

            watch.Stop();

            exceptionCount.ShouldEqual(0);
            failureCount.ShouldEqual(0);
            successCount.ShouldEqual(1);
        }

        private int ThrowException()
        {
            throw new FileNotFoundException();
        }

        private void ActionThrowException()
        {
            throw new FileNotFoundException();
        }

        private int NoException()
        {
            return 14;
        }

        private void ActionNoException()
        {
            return;
        }
    }
}
