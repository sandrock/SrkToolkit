
namespace SrkToolkit.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class StringReplacerTests
    {
        [TestMethod]
        public void Simple()
        {
            var target = new StringReplacer();
            target.Setup("Hello", r => "World");
            var text = "Hello {Hello}";
            var expected = "Hello World";
            var result = target.Replace(text);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SimpleAndDefault()
        {
            var target = new StringReplacer();
            target.Setup("Hello", r => "World");
            var text = "Hello {Hello} {Other}";
            var expected = "Hello World ";
            var result = target.Replace(text);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SimpleAndCustomDefault()
        {
            var target = new StringReplacer();
            target.Setup("Hello", r => "World");
            target.SetupDefault(r => "???");
            var text = "Hello {Hello} {Other}";
            var expected = "Hello World ???";
            var result = target.Replace(text);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Model()
        {
            var model = new UserModel
            {
                Name = "Johny",
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Name", r => r.Model.Name);
            var text = "Hello {User.Name}";
            var expected = "Hello Johny";
            var result = target.Replace(text, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ModelAndDefault()
        {
            var model = new UserModel
            {
                Name = "Johny",
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Name", r => r.Model.Name);
            var text = "Hello {User.Name} {Other}";
            var expected = "Hello Johny ";
            var result = target.Replace(text, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ModelAndCustomDefault()
        {
            var model = new UserModel
            {
                Name = "Johny",
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Name", r => r.Model.Name);
            target.SetupDefault(r => "??" + r.Key + "??");
            var text = "Hello {User.Name} {Other}";
            var expected = "Hello Johny ??Other??";
            var result = target.Replace(text, model);
            Assert.AreEqual(expected, result);
        }

        public class UserModel
        {
            public string Name { get; set; }
        }
    }
}
