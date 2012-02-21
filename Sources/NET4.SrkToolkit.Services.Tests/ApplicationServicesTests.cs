using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SrkToolkit.Services.Tests
{
    [TestClass]
    public class ApplicationServicesTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            ApplicationServices.Clear();
        }

        [TestMethod, TestCategory(Category.Unit)]
        public void RegisterInstanceAndResolve()
        {
            // prepare
            ClassA subject = new ClassA();
            object resolved = null;

            // execute
            ApplicationServices.Register<InterfaceA, ClassA>(subject);
            resolved = ApplicationServices.Resolve<InterfaceA>();

            // verify
            Assert.AreEqual(subject, resolved);
        }

        [TestMethod, TestCategory(Category.Unit), ExpectedException(typeof(ArgumentException))]
        public void CannotRegisterAgain()
        {
            // prepare
            ClassA subject1 = new ClassA();
            ClassA subject2 = new ClassA();

            // execute
            ApplicationServices.Register<InterfaceA, ClassA>(subject1);
            ApplicationServices.Register<InterfaceA, ClassA>(subject2);
        }

        [TestMethod, TestCategory(Category.Unit)]
        public void RegisterNewAndResolve()
        {
            // prepare
            object resolved = null;
            object resolved1 = null;

            // execute
            ApplicationServices.Register<InterfaceA, ClassA>();
            resolved = ApplicationServices.Resolve<InterfaceA>();

            // verify
            Assert.IsNotNull(resolved);
            Assert.IsInstanceOfType(resolved, typeof(ClassA));

            // execute again
            resolved1 = ApplicationServices.Resolve<InterfaceA>();

            // verify
            Assert.AreSame(resolved1, resolved);
        }

        [TestMethod, TestCategory(Category.Unit)]
        public void RegisterFactoryAndResolve()
        {
            // prepare
            Func<ClassA> subject = () => new ClassA();
            object resolved = null;
            object resolved1 = null;

            // execute
            ApplicationServices.Register<InterfaceA>(subject);
            resolved = ApplicationServices.Resolve<InterfaceA>();

            // verify
            Assert.IsNotNull(resolved);
            Assert.IsInstanceOfType(resolved, typeof(ClassA));

            // execute again
            resolved1 = ApplicationServices.Resolve<InterfaceA>();

            // verify
            Assert.AreSame(resolved1, resolved);
        }

        interface InterfaceA { }
        interface InterfaceB { }
        class ClassA : InterfaceA { }
        class ClassB : InterfaceB { }
    }
}
