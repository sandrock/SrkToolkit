
namespace SrkToolkit.Common.Tests
{
    using SrkToolkit.Common.Validation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class EmailAddressTests
    {
        public class Constructor1
        {
            [Fact]
            public void NullAddress()
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    var email = new EmailAddress(null);
                });
            }

            [Fact]
            public void EmptyAddress()
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    var email = new EmailAddress("   ");
                });
            }

            [Fact]
            public void SplitsEmailInParts_NoTag()
            {
                // prepare
                string user = "kevin.alexandre";
                string tag = null;
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(address);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(tag, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }

            [Fact]
            public void SplitsEmailInParts_WithTag()
            {
                // prepare
                string user = "kevin.alexandre";
                string tag = "test";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(address);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(tag, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }

            [Fact]
            public void SplitsEmailInParts_MultiTag()
            {
                // prepare
                string user = "kevin.alexandre";
                string tag = "test+test+";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(address);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(tag, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }

            [Fact]
            public void SplitsEmailInParts_EmptyTag()
            {
                // prepare
                string user = "kevin.alexandre";
                string tag = "";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(address);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(tag, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }
        }

        public class Constructor2
        {
            [Fact]
            public void NullAccountPart()
            {
                // prepare
                string user = null;
                string tag = "";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                Assert.Throws<ArgumentException>(() =>
                {
                    var email = new EmailAddress(address);
                });
            }

            [Fact]
            public void EmptyAccountPart()
            {
                // prepare
                string user = "";
                string tag = "";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                Assert.Throws<ArgumentException>(() =>
                {
                    var email = new EmailAddress(address);
                });
            }

            [Fact]
            public void NullDomainPart()
            {
                // prepare
                string user = "hello";
                string tag = "";
                string domain = null;
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                Assert.Throws<ArgumentException>(() =>
                {
                    var email = new EmailAddress(address);
                });
            }

            [Fact]
            public void EmptyDomainPart()
            {
                // prepare
                string user = "hello";
                string tag = "";
                string domain = "";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                Assert.Throws<ArgumentException>(() =>
                {
                    var email = new EmailAddress(address);
                });
            }

            [Fact]
            public void SplitsEmailInParts_NoTag()
            {
                // prepare
                string user = "kevin.alexandre";
                string tag = null;
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(user, tag, domain);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(tag, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }

            [Fact]
            public void SplitsEmailInParts_WithTag()
            {
                // prepare
                string user = "kevin.alexandre";
                string tag = "test";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(user, tag, domain);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(tag, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }

            [Fact]
            public void SplitsEmailInParts_MultiTag()
            {
                // prepare
                string user = "kevin.alexandre";
                string tag = "test+test+";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(user, tag, domain);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(tag, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }

            [Fact]
            public void SplitsEmailInParts_EmptyTag()
            {
                // prepare
                string user = "kevin.alexandre";
                string tag = "";
                string domain = "sparklenetworks.com";
                string local = user + (string.IsNullOrEmpty(tag) ? "" : ("+" + tag));
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(user, tag, domain);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(null, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }

            [Fact]
            public void SplitsEmailInParts_EmptyTag_SingleQuote()
            {
                // prepare
                string user = "danna.o'harra";
                string tag = "";
                string domain = "space.com";
                string local = user + (string.IsNullOrEmpty(tag) ? "" : ("+" + tag));
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(user, tag, domain);

                // verify
                Assert.Equal(local, email.LocalPart);
                Assert.Equal(user, email.AccountPart);
                Assert.Equal(null, email.TagPart);
                Assert.Equal(domain, email.DomainPart);
                Assert.Equal(address, email.Value);
            }
        }

        public class ValueWithoutTagGetter
        {
            [Fact]
            public void WorksWithNoTag()
            {
                // prepare
                string input = "antoine.alexandre@gmail.com";
                string expected = input;

                // execute
                var email = new EmailAddress(input);

                // verify
                Assert.Equal(expected, email.ValueWithoutTag);
            }

            [Fact]
            public void WorksWithOneTag()
            {
                // prepare
                string input = "antoine.alexandre+ojertuoir@gmail.com";
                string expected = "antoine.alexandre@gmail.com";

                // execute
                var email = new EmailAddress(input);

                // verify
                Assert.Equal(expected, email.ValueWithoutTag);
            }
        }

        public class ObjectEqualsMethod
        {
            [Fact]
            public void ObjectEqual_AgainstNull()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = null;
                var result = value1.Equals((object)value2);
                Assert.False(result);
            }

            [Fact]
            public void ObjectEqual_AgainstSameReference()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = value1;
                var result = value1.Equals((object)value2);
                Assert.True(result);
            }

            [Fact]
            public void ObjectEqual_AgainstSameValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("test@est.com");
                var result = value1.Equals((object)value2);
                Assert.True(result);
            }

            [Fact]
            public void ObjectEqual_AgainstDifferentValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("lalala@test.com");
                var result = value1.Equals((object)value2);
                Assert.False(result);
            }
        }

        public class EmailAddressEqualsMethod
        {
            [Fact]
            public void ObjectEqual_AgainstNull()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = null;
                var result = value1.Equals(value2);
                Assert.False(result);
            }

            [Fact]
            public void ObjectEqual_AgainstSameReference()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = value1;
                var result = value1.Equals(value2);
                Assert.True(result);
            }

            [Fact]
            public void ObjectEqual_AgainstSameValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("test@est.com");
                var result = value1.Equals(value2);
                Assert.True(result);
            }

            [Fact]
            public void ObjectEqual_AgainstDifferentValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("lalala@test.com");
                var result = value1.Equals(value2);
                Assert.False(result);
            }
        }

        public class OperatorEqualsMethod
        {
            [Fact]
            public void EqualityOperator_AgainstNull()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = null;
                var result = value1 == value2;
                Assert.False(result);
            }

            [Fact]
            public void EqualityOperator_AgainstSameReference()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = value1;
                var result = value1 == value2;
                Assert.True(result);
            }

            [Fact]
            public void EqualityOperator_AgainstSameValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("test@est.com");
                var result = value1 == value2;
                Assert.True(result);
            }

            [Fact]
            public void EqualityOperator_AgainstDifferentValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("lalala@test.com");
                var result = value1 == value2;
                Assert.False(result);
            }
        }

        public class OperatorInEqualsMethod
        {
            [Fact]
            public void InequalityOperator_AgainstNull()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = null;
                var result = value1 != value2;
                Assert.True(result);
            }

            [Fact]
            public void InequalityOperator_AgainstSameReference()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = value1;
                var result = value1 != value2;
                Assert.False(result);
            }

            [Fact]
            public void InequalityOperator_AgainstSameValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("test@est.com");
                var result = value1 != value2;
                Assert.False(result);
            }

            [Fact]
            public void InequalityOperator_AgainstDifferentValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("lalala@test.com");
                var result = value1 != value2;
                Assert.True(result);
            }
        }
    }
}
