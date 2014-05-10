// -----------------------------------------------------------------------
// <copyright file="ResultErrorTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Resources;

    public class ResultErrorTests
    {
        [TestClass]
        public class AddExtension
        {
            [TestMethod]
            public void Works()
            {
                var value = Lalala.One;
                var resourceManager = Strings.ResourceManager;
                var expected = "Hello World";
                var list = new List<ResultError<Lalala>>();
                list.Add(value, resourceManager);
                var result = list[0].DisplayMessage;
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void Format()
            {
                var value = Lalala.Infinity;
                var resourceManager = Strings.ResourceManager;
                var expected = "here is a format => test <=";
                var list = new List<ResultError<Lalala>>();
                list.Add(value, resourceManager, "test");
                var result = list[0].DisplayMessage;
                Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class WithPostProcessExtension
        {
            [TestMethod]
            public void Works()
            {
                var value = Lalala.One;
                var resourceManager = Strings.ResourceManager;
                var expected = "[Hello World]";
                var list = new List<ResultError<Lalala>>();
                list.WithPostProcess(str => "[" + str + "]").Add(value, resourceManager);
                var result = list[0].DisplayMessage;
                Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void Format()
            {
                var value = Lalala.Infinity;
                var resourceManager = Strings.ResourceManager;
                var expected = "[here is a format => test <=]";
                var list = new List<ResultError<Lalala>>();
                list.WithPostProcess(str => "[" + str + "]").Add(value, resourceManager, "test");
                var result = list[0].DisplayMessage;
                Assert.AreEqual(expected, result);
            }
        }

        public enum Lalala
        {
            None,
            One,
            Many,
            Infinity,
        }

        public enum Lololo
        {
            Unknown,
            Known,
            Other,
        }
    }
}
