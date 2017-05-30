
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.DataAnnotations;
    using System.ComponentModel.DataAnnotations;

    [TestClass]
    public class DateRangeAttributeTests
    {
        [TestMethod]
        public void NullValue_IsValid()
        {
            string value = null, name = "Date";
            var attr = new DateRangeAttribute();
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void EmptyValue_IsValid()
        {
            string value = "", name = "Date";
            var attr = new DateRangeAttribute();
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void InvalidValue_IsInvalid()
        {
            string value = "test", name = "Date";
            var attr = new DateRangeAttribute();
            attr.Minimum = "2015-01-01T00:00:00";
            attr.Maximum = "2015-01-03T00:00:00";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);

            Assert.IsNotNull(result);
            Assert.AreEqual("The field Date must be between 2015-01-01 00:00:00 and 2015-01-03 00:00:00.", result.ErrorMessage);
            Assert.AreEqual(1, result.MemberNames.Count());
            Assert.AreEqual(name, result.MemberNames.Single());
        }

        [TestMethod]
        public void ValidValue_IsValid()
        {
            string value = "2015-01-02T01:00:02", name = "Date";
            var attr = new DateRangeAttribute();
            attr.Minimum = "2015-01-01T00:00:00";
            attr.Maximum = "2015-01-03T00:00:00";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);

            Assert.IsNull(result);
        }
    }
}
