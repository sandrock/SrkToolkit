
namespace SrkToolkit.Common.Tests.DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.DataAnnotations;

    [TestClass]
    public class TimezoneAttributeTests
    {
        [TestMethod]
        public void Constructs()
        {
            var value = new TimezoneAttribute();
        }

        [TestMethod]
        public void ValidTz_Pass()
        {
            var model = new Model
            {
                Tz = "Romance Standard Time",
            };
            var attr = (TimezoneAttribute)model.GetType().GetProperty("Tz").GetCustomAttributes(false).First();
            Assert.IsTrue(attr.IsValid(model.Tz));
        }

        [TestMethod]
        public void NullTz_Pass()
        {
            var model = new Model
            {
                Tz = null,
            };
            var attr = (TimezoneAttribute)model.GetType().GetProperty("Tz").GetCustomAttributes(false).First();
            Assert.IsTrue(attr.IsValid(model.Tz));
        }

        [TestMethod]
        public void EmptyTz_Pass()
        {
            var model = new Model
            {
                Tz = "",
            };
            var attr = (TimezoneAttribute)model.GetType().GetProperty("Tz").GetCustomAttributes(false).First();
            Assert.IsTrue(attr.IsValid(model.Tz));
        }

        [TestMethod]
        public void InvalidTz_Invalid()
        {
            var model = new Model
            {
                Tz = "Paris", // not a valid timezone id
            };
            var attr = (TimezoneAttribute)model.GetType().GetProperty("Tz").GetCustomAttributes(false).First();
            Assert.IsFalse(attr.IsValid(model.Tz));
        }

        public class Model
        {
            [Timezone]
            public string Tz { get; set; }
        }
    }
}
