namespace SrkToolkit.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Should;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    [TestClass]
    public class RetryLogicTests
    {
        [TestMethod]
        public async Task FuncAsync_Should_Be_Tried_3Times_With_ConstantInterval()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            await RetryLogic
                .DoAsync<int>(this.ThrowExceptionAsync)
                .Handle<FileNotFoundException>()
                .NumberOfAttempts(3)
                .WithInterval(TimeSpan.FromMilliseconds(2000))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .RunAsync();

            watch.Stop();

            watch.Elapsed.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(2000 * 2));
            exceptionCount.ShouldEqual(3);
            failureCount.ShouldEqual(1);
            successCount.ShouldEqual(0);
        }

        [TestMethod]
        public async Task FuncAsync_Should_Be_Tried_3Times_With_ConstantInterval_IsTraced()
        {
            string trace = "";
            var listener = new TestTraceListener();
            listener.TraceWrite += (s, e) => trace += e.Message;
            Trace.Listeners.Add(listener);
            try
            {
                var exceptionCount = 0;
                var failureCount = 0;
                var successCount = 0;
                var watch = new Stopwatch();
                watch.Start();

                await RetryLogic
                    .DoAsync<int>(this.ThrowExceptionAsync)
                    .Handle<FileNotFoundException>()
                    .TraceRetryEvents(true)
                    .NumberOfAttempts(3)
                    .WithInterval(TimeSpan.FromMilliseconds(2000))
                    .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
                    .OnException<FileNotFoundException>(e => ++exceptionCount)
                    .OnFailure(() => ++failureCount)
                    .OnSuccess(x =>
                    {
                        ++successCount;
                    })
                    .RunAsync();

                watch.Stop();

                watch.Elapsed.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(2000 * 2));
                exceptionCount.ShouldEqual(3);
                failureCount.ShouldEqual(1);
                successCount.ShouldEqual(0);
                trace.ShouldContain("Max attempt number reached");
            }
            finally
            {
                Trace.Listeners.Remove(listener);
            }
        }

        [TestMethod]
        public async Task ActionAsync_Should_Throw_An_Exception()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            var profile = RetryLogic
                .InitSettings()
                .Handle<NullReferenceException>()
                .NumberOfAttempts(3)
                .WithInterval(TimeSpan.FromMilliseconds(2000))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
                .TraceRetryEvents(true)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .Prepare();

            try
            {
                ////await RetryLogic.DoAsync(ActionThrowExceptionAsync, profile)
                await profile.DoAsync(ActionThrowExceptionAsync)
                    .OnSuccess(x =>
                    {
                        ++successCount;
                    })
                    .RunAsync();
            }
            catch (Exception ex)
            {
                ex.ShouldBeType(typeof(FileNotFoundException));
            }
            watch.Stop();

            exceptionCount.ShouldEqual(0);
            failureCount.ShouldEqual(1);
            successCount.ShouldEqual(0);
        }

        [TestMethod]
        public async Task ActionAsync_Should_Be_Tried_3Times_With_ConstantInterval()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            var profile = RetryLogic
                .InitSettings()
                .Handle<FileNotFoundException>()
                .NumberOfAttempts(3)
                .WithInterval(TimeSpan.FromMilliseconds(2000))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
                .TraceRetryEvents(true)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .Prepare();

            ////await RetryLogic.DoAsync(ActionThrowExceptionAsync, profile)
            await profile.DoAsync(ActionThrowExceptionAsync)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .RunAsync();

            watch.Stop();

            watch.Elapsed.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(2000 * 2));
            exceptionCount.ShouldEqual(3);
            failureCount.ShouldEqual(1);
            successCount.ShouldEqual(0);
        }

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
                .NumberOfAttempts(5)
                .WithInterval(TimeSpan.FromMilliseconds(200))
                .IntervalStrategy(RetryIntervalStrategy.ProgressiveDelay, 2)
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
                .NumberOfAttempts(4)
                .WithInterval(TimeSpan.FromMilliseconds(2000))
                .IntervalStrategy(RetryIntervalStrategy.ExponentialDelay, 2)
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
                .NumberOfAttempts(3)
                .WithInterval(TimeSpan.FromMilliseconds(2000))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
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
                .NumberOfAttempts(3)
                .WithInterval(TimeSpan.FromMilliseconds(2000))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
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
        public async Task FunctionAsync_Should_Be_A_Success()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            await RetryLogic
                .DoAsync<int>(this.NoExceptionAsync)
                .Handle<FileNotFoundException>()
                .NumberOfAttempts(3)
                .WithInterval(TimeSpan.FromMilliseconds(2000))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .RunAsync();

            watch.Stop();

            exceptionCount.ShouldEqual(0);
            failureCount.ShouldEqual(0);
            successCount.ShouldEqual(1);
        }

        [TestMethod]
        public async Task ActionAsync_Should_Be_A_Success()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            await RetryLogic
                .DoAsync(this.ActionNoExceptionAsync)
                .Handle<FileNotFoundException>()
                .NumberOfAttempts(3)
                .WithInterval(TimeSpan.FromMilliseconds(2000))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .RunAsync();

            watch.Stop();

            exceptionCount.ShouldEqual(0);
            failureCount.ShouldEqual(0);
            successCount.ShouldEqual(1);
        }


        [TestMethod]
        public async Task ActionAsync_Should_Try_10_Times()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            var profile = RetryLogic
                .InitSettings()
                .Handle<FileNotFoundException>()
                .NumberOfAttempts(10)
                .WithInterval(TimeSpan.FromMilliseconds(0))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .Prepare();


            await profile.DoAsync(ActionThrowExceptionAsync)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .RunAsync();

            watch.Stop();
            exceptionCount.ShouldEqual(10);
            failureCount.ShouldEqual(1);
            successCount.ShouldEqual(0);
        }

        [TestMethod]
        public async Task Profile_Should_Be_Frozen()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            var profile = RetryLogic
                .InitSettings()
                .Handle<FileNotFoundException>()
                .NumberOfAttempts(10)
                .WithInterval(TimeSpan.FromMilliseconds(0))
                .IntervalStrategy(RetryIntervalStrategy.ConstantDelay, 0)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .Prepare();


            ////await RetryLogic.DoAsync(ActionThrowExceptionAsync, profile)
            await profile.DoAsync(ActionThrowExceptionAsync)
                .NumberOfAttempts(5)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .RunAsync();

            watch.Stop();
            exceptionCount.ShouldEqual(5);
            failureCount.ShouldEqual(1);
            successCount.ShouldEqual(0);

            ////await RetryLogic.DoAsync(ActionThrowExceptionAsync, profile)
            await profile.DoAsync(ActionThrowExceptionAsync)
                .OnSuccess(x =>
                {
                    ++successCount;
                })
                .RunAsync();

            watch.Stop();
            exceptionCount.ShouldEqual(15);
            failureCount.ShouldEqual(2);
            successCount.ShouldEqual(0);
        }

        [TestMethod]
        public async Task Retry_Without_Settings_Should_Fail()
        {
            var watch = new Stopwatch();
            watch.Start();
            Exception exception = null;

            try
            {
                await RetryLogic.DoAsync(ActionThrowExceptionAsync)
                    .RunAsync();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            exception.ShouldBeType(typeof(NullReferenceException));
            watch.Stop();
        }

        [TestMethod]
        public async Task Retry_With_Minimal_Settings_Should_Not_Fail()
        {
            var watch = new Stopwatch();
            watch.Start();
            Exception exception = null;


            await RetryLogic.DoAsync(ActionThrowExceptionAsync)
                .Handle<FileNotFoundException>()
                .RunAsync();

            watch.Stop();

            watch.Elapsed.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(4010));
        }

        private async Task<int> ThrowExceptionAsync()
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                int i = 14;
                throw new FileNotFoundException();
            });
        }

        private int ThrowException()
        {
            throw new FileNotFoundException();
        }

        private async Task ActionThrowExceptionAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                int i = 14;
                throw new FileNotFoundException();
            });
        }

        private void ActionThrowException()
        {
            throw new FileNotFoundException();
        }

        private async Task<int> NoExceptionAsync()
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                int i = 14;
                return i;
            });
        }

        private int NoException()
        {
            return 14;
        }

        private async Task ActionNoExceptionAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                return;
            });
        }

        private void ActionNoException()
        {
            return;
        }
    }
}
