// 
// Copyright 2014 SandRock
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

namespace SrkToolkit.Services.Tests
{
    using System;
    using Xunit;

    /// <summary>
    ///This is a test class for <see cref="AppEventBus"/> and is intended
    ///to contain all <see cref="AppEventBus"/> Unit Tests
    ///</summary>
    public class AppEventBusTest
    {
        public class RegisterMethod
        {
            [Fact]
            public void ValidatesInputs0()
            {
                Action<object, DateTime> action = null;
                Assert.Throws<ArgumentNullException>(() => new AppEventBus().Register(action));
            }

            [Fact]
            public void ValidatesInputs1()
            {
                Action<object, DateTime> action = (s, e) => { };
                Assert.Throws<ArgumentNullException>(() => new AppEventBus().Register(action, null));
            }

            [Fact, Trait("TestCategory", Category.Unit)]
            public void ReturnsDisposable0()
            {
                Action<object, DateTime> action = (s, e) => { };
                var result = new AppEventBus().Register(action);

                Assert.NotNull(result);
                result.Dispose();
                Assert.True(result.IsDisposed);
            }

            [Fact, Trait("TestCategory", Category.Unit)]
            public void ReturnsDisposable1()
            {
                Action<object, DateTime> action = (s, e) => { };
                var result = new AppEventBus().Register(action, new object());

                Assert.NotNull(result);
                result.Dispose();
                Assert.True(result.IsDisposed);
            }
        }

        public class PublishMethod
        {
            [Fact, Trait("TestCategory", Category.Unit)]
            public void DoesNothingIfNoRegistrations()
            {
                new AppEventBus().Publish(new object(), new object());
            }

            [Fact, Trait("TestCategory", Category.Unit)]
            public void PublishesToAllRegistered()
            {
                // prepare
                bool a = false, b = false;
                var bus = new AppEventBus();

                // execute
                bus.Register<DateTime>((s1, e1) => a = true);
                bus.Register<DateTime>((s2, e2) => b = true);
                bus.Publish(new object(), DateTime.Now);

                // verify
                Assert.True(a);
                Assert.True(b);
            }

            [Fact, Trait("TestCategory", Category.Unit)]
            public void DoesNotPublishToOthers()
            {
                // prepare
                bool a = false, b = false, c = false;
                var bus = new AppEventBus();

                // execute
                bus.Register<DateTime>((s1, e1) => a = true);
                bus.Register<DateTime>((s2, e2) => b = true);
                bus.Register<TimeSpan>((s3, e3) => c = true);
                bus.Publish(new object(), DateTime.Now);

                // verify
                Assert.True(a);
                Assert.True(b);
                Assert.False(c);
            }

            [Fact, Trait("TestCategory", Category.Unit)]
            public void DoesNotPublishToSelf()
            {
                // prepare
                object o1 = new object();
                bool a = false, b = false, c = false;
                var bus = new AppEventBus();

                // execute
                bus.Register<DateTime>((s1, e1) => a = true);
                bus.Register<DateTime>((s2, e2) => b = true, o1);
                bus.Register<DateTime>((s3, e3) => c = true);
                bus.Publish(o1, DateTime.Now);

                // verify
                Assert.True(a);
                Assert.False(b);
                Assert.True(c);
            }

            [Fact, Trait("TestCategory", Category.Unit)]
            public void WorksManyTimes()
            {
                // prepare
                object o1 = new object();
                bool a = false, b = false, c = false;
                var bus = new AppEventBus();

                // execute
                bus.Register<DateTime>((s1, e1) => a = true);
                bus.Register<DateTime>((s2, e2) => b = true, o1);
                bus.Register<TimeSpan>((s3, e3) => c = true);
                bus.Publish(o1, DateTime.Now);

                // verify
                Assert.True(a);
                Assert.False(b);
                Assert.False(c);

                // execute
                a = b = c = false;
                bus.Publish(o1, DateTime.Now);

                // verify
                Assert.True(a);
                Assert.False(b);
                Assert.False(c);

                // execute
                a = b = c = false;
                bus.Publish(o1, DateTime.Now);

                // verify
                Assert.True(a);
                Assert.False(b);
                Assert.False(c);
            }
        }
    }
}
