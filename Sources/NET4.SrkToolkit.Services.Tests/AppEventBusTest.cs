using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SrkToolkit.Services.Tests
{
    /// <summary>
    ///This is a test class for <see cref="AppEventBus"/> and is intended
    ///to contain all <see cref="AppEventBus"/> Unit Tests
    ///</summary>
    [TestClass()]
    public class AppEventBusTest {
        [TestClass]
        public class RegisterMethod {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ValidatesInputs0() {
                Action<object, DateTime> action = null;
                new AppEventBus().Register(action);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void ValidatesInputs1() {
                Action<object, DateTime> action = (s, e) => { };
                new AppEventBus().Register(action, null);
            }

            [TestMethod]
            public void ReturnsDisposable0() {
                Action<object, DateTime> action = (s, e) => { };
                var result = new AppEventBus().Register(action);

                Assert.IsNotNull(result);
                result.Dispose();
                Assert.IsTrue(result.IsDisposed);
            }

            [TestMethod]
            public void ReturnsDisposable1() {
                Action<object, DateTime> action = (s, e) => { };
                var result = new AppEventBus().Register(action, new object());

                Assert.IsNotNull(result);
                result.Dispose();
                Assert.IsTrue(result.IsDisposed);
            }
        }

        [TestClass]
        public class PublishMethod {
            [TestMethod]
            public void DoesNothingIfNoRegistrations() {
                new AppEventBus().Publish(new object(), new object());
            }

            [TestMethod]
            public void PublishesToAllRegistered() {
                // prepare
                bool a = false, b = false;
                var bus = new AppEventBus();

                // execute
                bus.Register<DateTime>((s1, e1) => a = true);
                bus.Register<DateTime>((s2, e2) => b = true);
                bus.Publish(new object(), DateTime.Now);

                // verify
                Assert.IsTrue(a);
                Assert.IsTrue(b);
            }

            [TestMethod]
            public void DoesNotPublishToOthers() {
                // prepare
                bool a = false, b = false, c = false;
                var bus = new AppEventBus();

                // execute
                bus.Register<DateTime>((s1, e1) => a = true);
                bus.Register<DateTime>((s2, e2) => b = true);
                bus.Register<TimeSpan>((s3, e3) => c = true);
                bus.Publish(new object(), DateTime.Now);

                // verify
                Assert.IsTrue(a);
                Assert.IsTrue(b);
                Assert.IsFalse(c);
            }

            [TestMethod]
            public void DoesNotPublishToSelf() {
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
                Assert.IsTrue(a);
                Assert.IsFalse(b);
                Assert.IsTrue(c);
            }

            [TestMethod]
            public void WorksManyTimes() {
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
                Assert.IsTrue(a);
                Assert.IsFalse(b);
                Assert.IsFalse(c);

                // execute
                a = b = c = false;
                bus.Publish(o1, DateTime.Now);

                // verify
                Assert.IsTrue(a);
                Assert.IsFalse(b);
                Assert.IsFalse(c);

                // execute
                a = b = c = false;
                bus.Publish(o1, DateTime.Now);

                // verify
                Assert.IsTrue(a);
                Assert.IsFalse(b);
                Assert.IsFalse(c);
            }
        }
    }
}
