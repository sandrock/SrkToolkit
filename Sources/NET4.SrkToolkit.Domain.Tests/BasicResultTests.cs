
namespace SrkToolkit.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Domain;

    [TestClass]
    public class BasicResultTests
    {
        [TestClass]
        public class AddExtension
        {
            [TestMethod]
            public void EnumValue()
            {
                var value = Lalala.One;
                var resourceManager = Strings.ResourceManager;
                var expected = "Hello World";
                IList<BasicResultError> list = new List<BasicResultError>();
                list.Add(value, resourceManager);
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
    }
}
