
namespace SrkToolkit.Common.Tests
{
    using SrkToolkit.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Xunit;

    public class DateRangeAttributeTests
    {
        private static ValidationContext CreateContext(string name = "Date")
        {
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            return context;
        }

        public class IsValidMethod
        {
            [Fact]
            public void NullValue_IsValid()
            {
                var result = new DateRangeAttribute().GetValidationResult(null, CreateContext());

                Assert.Null(result);
            }

            [Fact]
            public void EmptyString_IsValid()
            {
                var result = new DateRangeAttribute().GetValidationResult("", CreateContext());

                Assert.Null(result);
            }

            [Fact]
            public void NoBounds_ValidDate_IsValid()
            {
                var result = new DateRangeAttribute().GetValidationResult("2015-01-02T00:00:00", CreateContext());

                Assert.Null(result);
            }

            [Fact]
            public void NonIsoString_IsInvalid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };
                var result = attr.GetValidationResult("not-a-date", CreateContext());

                Assert.NotNull(result);
                Assert.Equal(1, result.MemberNames.Count());
                Assert.Equal("Date", result.MemberNames.Single());
            }

            [Fact]
            public void ValueWithinRange_IsValid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };

                Assert.Null(attr.GetValidationResult("2015-01-02T01:00:02", CreateContext()));
            }

            [Fact]
            public void ValueBelowMinimum_IsInvalid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };

                Assert.NotNull(attr.GetValidationResult("2014-12-31T23:59:59", CreateContext()));
            }

            [Fact]
            public void ValueAboveMaximum_IsInvalid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };

                Assert.NotNull(attr.GetValidationResult("2015-01-04T00:00:00", CreateContext()));
            }

            [Fact]
            public void ValueEqualsMinimum_IsValid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };

                Assert.Null(attr.GetValidationResult("2015-01-01T00:00:00", CreateContext()));
            }

            [Fact]
            public void ValueEqualsMaximum_IsValid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };

                Assert.Null(attr.GetValidationResult("2015-01-03T00:00:00", CreateContext()));
            }

            [Fact]
            public void OnlyMinimum_ValueAbove_IsValid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00" };

                Assert.Null(attr.GetValidationResult("2015-06-01T00:00:00", CreateContext()));
            }

            [Fact]
            public void OnlyMinimum_ValueBelow_IsInvalid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00" };

                Assert.NotNull(attr.GetValidationResult("2014-12-31T00:00:00", CreateContext()));
            }

            [Fact]
            public void OnlyMaximum_ValueBelow_IsValid()
            {
                var attr = new DateRangeAttribute { Maximum = "2015-01-03T00:00:00" };

                Assert.Null(attr.GetValidationResult("2015-01-01T00:00:00", CreateContext()));
            }

            [Fact]
            public void OnlyMaximum_ValueAbove_IsInvalid()
            {
                var attr = new DateRangeAttribute { Maximum = "2015-01-03T00:00:00" };

                Assert.NotNull(attr.GetValidationResult("2015-01-04T00:00:00", CreateContext()));
            }

            [Fact]
            public void DateTimeValue_WithinRange_IsValid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };
                var value = new DateTime(2015, 1, 2, 0, 0, 0, DateTimeKind.Utc);

                Assert.Null(attr.GetValidationResult(value, CreateContext()));
            }

            [Fact]
            public void DateTimeValue_BelowMinimum_IsInvalid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };
                var value = new DateTime(2014, 12, 31, 0, 0, 0, DateTimeKind.Utc);

                Assert.NotNull(attr.GetValidationResult(value, CreateContext()));
            }

            [Fact]
            public void InvalidMinimum_ThrowsInvalidOperationException()
            {
                var attr = new DateRangeAttribute { Minimum = "not-a-date", Maximum = "2015-01-03T00:00:00" };
                var value = new DateTime(2015, 1, 2, 0, 0, 0, DateTimeKind.Utc);

                Assert.Throws<InvalidOperationException>(() => attr.GetValidationResult(value, CreateContext()));
            }

            [Fact]
            public void InvalidMaximum_ThrowsInvalidOperationException()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "not-a-date" };
                var value = new DateTime(2015, 1, 2, 0, 0, 0, DateTimeKind.Utc);

                Assert.Throws<InvalidOperationException>(() => attr.GetValidationResult(value, CreateContext()));
            }

            [Fact]
            public void UserInput_SpaceSeparator_IsValid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };

                Assert.Null(attr.GetValidationResult("2015-01-02 01:00:02", CreateContext()));
            }

            [Fact]
            public void UserInput_NoSeconds_IsValid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };

                Assert.Null(attr.GetValidationResult("2015-01-02T01:00", CreateContext()));
            }

            [Fact]
            public void UserInput_DateOnly_IsValid()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01", Maximum = "2015-01-03" };

                Assert.Null(attr.GetValidationResult("2015-01-02", CreateContext()));
            }

            [Fact]
            public void BoundsWithSpaceSeparator_Work()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01 00:00:00", Maximum = "2015-01-03 00:00:00" };

                Assert.Null(attr.GetValidationResult("2015-01-02T00:00:00", CreateContext()));
                Assert.NotNull(attr.GetValidationResult("2014-12-31T00:00:00", CreateContext()));
            }

            [Fact]
            public void BoundsWithNoSeconds_Work()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00", Maximum = "2015-01-03T00:00" };

                Assert.Null(attr.GetValidationResult("2015-01-02T00:00:00", CreateContext()));
                Assert.NotNull(attr.GetValidationResult("2014-12-31T00:00:00", CreateContext()));
            }
        }

        public class FormatErrorMessageMethod
        {
            [Fact]
            public void MinAndMax_FormatsCorrectly()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "2015-01-03T00:00:00" };

                Assert.Equal("The field Date must be between 2015-01-01 00:00:00 and 2015-01-03 00:00:00.", attr.FormatErrorMessage("Date"));
            }

            [Fact]
            public void MinOnly_FormatsCorrectly()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00" };

                Assert.Equal("The field Date must be greater than 2015-01-01 00:00:00.", attr.FormatErrorMessage("Date"));
            }

            [Fact]
            public void MaxOnly_FormatsCorrectly()
            {
                var attr = new DateRangeAttribute { Maximum = "2015-01-03T00:00:00" };

                Assert.Equal("The field Date must be lower than 2015-01-03 00:00:00.", attr.FormatErrorMessage("Date"));
            }

            [Fact]
            public void NoBounds_FormatsCorrectly()
            {
                var attr = new DateRangeAttribute();

                Assert.Equal("The field Date is not valid.", attr.FormatErrorMessage("Date"));
            }

            [Fact]
            public void InvalidMinimum_Throws()
            {
                var attr = new DateRangeAttribute { Minimum = "not-a-date", Maximum = "2015-01-03T00:00:00" };

                Assert.Throws<InvalidOperationException>(() => attr.FormatErrorMessage("Date"));
            }

            [Fact]
            public void InvalidMaximum_Throws()
            {
                var attr = new DateRangeAttribute { Minimum = "2015-01-01T00:00:00", Maximum = "not-a-date" };

                Assert.Throws<InvalidOperationException>(() => attr.FormatErrorMessage("Date"));
            }
        }
    }
}
