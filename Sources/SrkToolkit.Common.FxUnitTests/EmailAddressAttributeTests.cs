
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    ////using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Xunit;
    using EmailAddressAttribute = SrkToolkit.DataAnnotations.EmailAddressAttribute;

#if NETFRAMEWORK
    public class EmailAddressAttributeTests
    {
        public class SingleValidation
        {
            [Fact]
            public void NullValue_IsValid()
            {
                string value = null, name = "Email";
                var attr = new EmailAddressAttribute();
                var context = new ValidationContext(new object(), null, null);
                context.MemberName = name;
                var result = attr.GetValidationResult(value, context);

                Assert.Null(result);
            }

            [Fact]
            public void EmptyValue_IsValid()
            {
                string value = "", name = "Email";
                var attr = new EmailAddressAttribute();
                var context = new ValidationContext(new object(), null, null);
                context.MemberName = name;
                var result = attr.GetValidationResult(value, context);

                Assert.Null(result);
            }

            [Fact]
            public void InvalidAddress_IsInvalid()
            {
                string value = "test", name = "Email";
                var attr = new EmailAddressAttribute();
                var context = new ValidationContext(new object(), null, null);
                context.MemberName = name;
                var result = attr.GetValidationResult(value, context);

                Assert.NotNull(result);
                Assert.Equal("A valid email address is required.", result.ErrorMessage);
                Assert.Equal(1, result.MemberNames.Count());
                Assert.Equal(name, result.MemberNames.Single());
            }

            [Fact]
            public void ValidAddress_IsValid()
            {
                string value = "test@test.com", name = "Email";
                var attr = new EmailAddressAttribute();
                var context = new ValidationContext(new object(), null, null);
                context.MemberName = name;
                var result = attr.GetValidationResult(value, context);

                Assert.Null(result);
            }

            [Fact]
            public void ValidTagAddress_IsValid()
            {
                string value = "test+test@test.com", name = "Email";
                var attr = new EmailAddressAttribute();
                var context = new ValidationContext(new object(), null, null);
                context.MemberName = name;
                var result = attr.GetValidationResult(value, context);

                Assert.Null(result);
            }

            [Fact]
            public void ValidParisAddress_IsValid()
            {
                string value = "test@test.paris", name = "Email";
                var attr = new EmailAddressAttribute();
                var context = new ValidationContext(new object(), null, null);
                context.MemberName = name;
                var result = attr.GetValidationResult(value, context);

                Assert.Null(result);
            }
        }

        public class MultiValidation
        {
            [Fact]
            public void NullValue_IsValid()
            {
                string value = null;
                var attr = new EmailAddressAttribute()
                {
                    AllowMultiple = true,
                };
                var result = attr.GetIsValid(value, null);

                Assert.Null(result);
                //Assert.Equal(result.ErrorMessage, "A valid email address is required.");
            }

            [Fact]
            public void EmptyValue_IsValid()
            {
                string value = "";
                var attr = new EmailAddressAttribute()
                {
                    AllowMultiple = true,
                };
                var result = attr.GetIsValid(value, null);

                Assert.Null(result);
                //Assert.Equal(result.ErrorMessage, "A valid email address is required.");
            }

            [Fact]
            public void InvalidAddress_IsValid()
            {
                string value = "test";
                var attr = new EmailAddressAttribute()
                {
                    AllowMultiple = true,
                };
                var result = attr.GetIsValid(value, null);

                Assert.Null(result);
            }

            [Fact]
            public void ValidAddress_IsValid()
            {
                string value = "test@test.com";
                var attr = new EmailAddressAttribute()
                {
                    AllowMultiple = true,
                };
                var result = attr.GetIsValid(value, null);

                Assert.Null(result);
            }

            [Fact]
            public void MultiValidAddress_IsValid()
            {
                string value = "test@test.com test@test.com test@test.com cool";
                var attr = new EmailAddressAttribute()
                {
                    AllowMultiple = true,
                };
                var result = attr.GetIsValid(value, null);

                Assert.Null(result);
            }

            [Fact]
            public void ValidTagAddress_IsValid()
            {
                string value = "test+test@test.com";
                var attr = new EmailAddressAttribute()
                {
                    AllowMultiple = true,
                };
                var result = attr.GetIsValid(value, null);

                Assert.Null(result);
            }

            [Fact]
            public void InvalidAddressAndMin1_IsInvalid()
            {
                string value = "test";
                var attr = new EmailAddressAttribute()
                {
                    AllowMultiple = true,
                    MinimumAddresses = 1,
                };
                var result = attr.GetIsValid(value, null);

                Assert.NotNull(result);
                Assert.True(result.ErrorMessage.Contains("minimum of"));
            }

            [Fact]
            public void MultiValidAddressAndMax1_IsValid()
            {
                string value = "test@test.com test@test.com test@test.com cool";
                var attr = new EmailAddressAttribute()
                {
                    AllowMultiple = true,
                    MaximumAddresses = 1,
                };
                var result = attr.GetIsValid(value, null);

                Assert.NotNull(result);
                Assert.True(result.ErrorMessage.Contains("maximum of"));
            }
        }
    }
    #endif
}
