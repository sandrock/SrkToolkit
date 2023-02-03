// 
// Copyright 2014 SandRock, pyDez
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

namespace SrkToolkit.Common.Tests
{
    using Should;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public class RetryLogicTests
    {
        [Fact]
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
                .AtMost(3)
                .WithConstantInterval(TimeSpan.FromMilliseconds(2000))
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

        [Fact]
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
                    .WithTrace(true)
                    .AtMost(3)
                    .WithConstantInterval(TimeSpan.FromMilliseconds(2000))
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

        [Fact]
        public async Task ActionAsync_Should_Throw_An_Exception()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            var profile = RetryLogic
                .BeginPrepare()
                .Handle<NullReferenceException>()
                .AtMost(3)
                .WithConstantInterval(TimeSpan.FromMilliseconds(2000))
                .WithTrace(true)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .EndPrepare();

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
            failureCount.ShouldEqual(0);
            successCount.ShouldEqual(0);
        }

        [Fact]
        public async Task ActionAsync_Should_Be_Tried_3Times_With_ConstantInterval()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            var profile = RetryLogic
                .BeginPrepare()
                .Handle<FileNotFoundException>()
                .AtMost(3)
                .WithConstantInterval(TimeSpan.FromMilliseconds(2000))
                .WithTrace(true)
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .EndPrepare();

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

        [Fact]
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
                .AtMost(3)
                .WithConstantInterval(TimeSpan.FromMilliseconds(2000))
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

        [Fact]
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
                .AtMost(3)
                .WithConstantInterval(TimeSpan.FromMilliseconds(2000))
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


        [Fact]
        public async Task ActionAsync_Should_Try_10_Times()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            var profile = RetryLogic
                .BeginPrepare()
                .Handle<FileNotFoundException>()
                .AtMost(10)
                .WithConstantInterval(TimeSpan.FromMilliseconds(0))
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .EndPrepare();


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

        [Fact]
        public async Task Profile_Should_Be_Frozen()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            var profile = RetryLogic
                .BeginPrepare()
                .Handle<FileNotFoundException>()
                .AtMost(10)
                .WithConstantInterval(TimeSpan.FromMilliseconds(0))
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount)
                .EndPrepare();


            ////await RetryLogic.DoAsync(ActionThrowExceptionAsync, profile)
            await profile.DoAsync(ActionThrowExceptionAsync)
                .AtMost(5)
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

        [Fact]
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

            exception.ShouldBeType(typeof(InvalidOperationException));
            watch.Stop();
        }

        [Fact]
        public async Task Retry_With_Minimal_Settings_Should_Not_Fail()
        {
            var watch = new Stopwatch();
            watch.Start();
            Exception exception = null;


            await RetryLogic.DoAsync(ActionThrowExceptionAsync)
                .OnFailureContinue()
                .Handle<FileNotFoundException>()
                .RunAsync();

            watch.Stop();

            watch.Elapsed.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(4010));
        }

        [Fact]
        public async Task DeveloperForgetsEndPrepare()
        {
            var exceptionCount = 0;
            var failureCount = 0;
            var successCount = 0;

            var profile = RetryLogic
                .BeginPrepare()
                .Handle<FileNotFoundException>()
                .AtMost(10)
                .WithConstantInterval(TimeSpan.FromMilliseconds(0))
                .OnException<FileNotFoundException>(e => ++exceptionCount)
                .OnFailure(() => ++failureCount);
                ////.EndPrepare(); // developer forgets tha line


            ////await RetryLogic.DoAsync(ActionThrowExceptionAsync, profile)
            try
            {
                await profile.DoAsync(ActionThrowExceptionAsync)
                        .AtMost(5)
                        .OnSuccess(x =>
                        {
                            ++successCount;
                        })
                        .RunAsync();
                Assert.Fail("Should have thrown a InvalidOperationException");
            }
            catch (InvalidOperationException ex)
            {
                // ok
            }
        }

        private async Task<int> ThrowExceptionAsync()
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                int i = 14;
                throw new FileNotFoundException();
            });
        }

        private async Task ActionThrowExceptionAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                int i = 14;
                throw new FileNotFoundException();
            });
        }

        private async Task<int> NoExceptionAsync()
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                int i = 14;
                return i;
            });
        }

        private async Task ActionNoExceptionAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                return;
            });
        }
    }
}
