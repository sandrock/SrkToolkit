
namespace SrkToolkit.Common.Tests
{
    using SrkToolkit.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class DateRangeAttributeTests
    {
        [Fact]
        public void NullValue_IsValid()
        {
            string value = null, name = "Date";
            var attr = new DateRangeAttribute();
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);

            Assert.Null(result);
        }

        [Fact]
        public void EmptyValue_IsValid()
        {
            string value = "", name = "Date";
            var attr = new DateRangeAttribute();
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);

            Assert.Null(result);
        }

        [Fact]
        public void InvalidValue_IsInvalid()
        {
            string value = "test", name = "Date";
            var attr = new DateRangeAttribute();
            attr.Minimum = "2015-01-01T00:00:00";
            attr.Maximum = "2015-01-03T00:00:00";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);

            Assert.NotNull(result);
            Assert.Equal("The field Date must be between 2015-01-01 00:00:00 and 2015-01-03 00:00:00.", result.ErrorMessage);
            Assert.Equal(1, result.MemberNames.Count());
            Assert.Equal(name, result.MemberNames.Single());
        }

        [Fact]
        public void ValidValue_IsValid()
        {
            string value = "2015-01-02T01:00:02", name = "Date";
            var attr = new DateRangeAttribute();
            attr.Minimum = "2015-01-01T00:00:00";
            attr.Maximum = "2015-01-03T00:00:00";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);

            Assert.Null(result);
        }
    }
}
