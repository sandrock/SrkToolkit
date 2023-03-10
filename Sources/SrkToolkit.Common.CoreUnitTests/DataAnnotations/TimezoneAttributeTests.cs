
namespace SrkToolkit.Common.Tests.DataAnnotations
{
    using SrkToolkit.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.DataAnnotations;
    using Xunit;

    public class TimezoneAttributeTests
    {
        [Fact]
        public void Constructs()
        {
            var value = new TimezoneAttribute();
        }

        [Fact]
        public void Model_ValidTz_Pass()
        {
            var model = new Model
            {
                Tz = "Romance Standard Time",
            };
            var attr = (TimezoneAttribute)model.GetType().GetProperty("Tz").GetCustomAttributes(false).First();
            Assert.True(attr.IsValid(model.Tz));
        }

        [Fact]
        public void Model_NullTz_Pass()
        {
            var model = new Model
            {
                Tz = null,
            };
            var attr = (TimezoneAttribute)model.GetType().GetProperty("Tz").GetCustomAttributes(false).First();
            Assert.True(attr.IsValid(model.Tz));
        }

        [Fact]
        public void Model_EmptyTz_Pass()
        {
            var model = new Model
            {
                Tz = "",
            };
            var attr = (TimezoneAttribute)model.GetType().GetProperty("Tz").GetCustomAttributes(false).First();
            Assert.True(attr.IsValid(model.Tz));
        }

        [Fact]
        public void Model_InvalidTz_Invalid()
        {
            var model = new Model
            {
                Tz = "Paris", // not a valid timezone id
            };
            var attr = (TimezoneAttribute)model.GetType().GetProperty("Tz").GetCustomAttributes(false).First();
            Assert.False(attr.IsValid(model.Tz));
        }

        [Fact]
        public void InvalidTz_Invalid()
        {
            var attr = new TimezoneAttribute();
            string value = "foobar", name = "Tz";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);
            Assert.NotNull(result);
            Assert.Equal(@"The timezone is not valid.", result.ErrorMessage);
            Assert.Equal(1, result.MemberNames.Count());
            Assert.Equal(name, result.MemberNames.Single());
        }

        public class Model
        {
            [Timezone]
            public string Tz { get; set; }
        }
    }
}
