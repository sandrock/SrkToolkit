
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Common.Validation;

    public class EmailAddressTests
    {
        [TestClass]
        public class Constructor1
        {
            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void NullAddress()
            {
                var email = new EmailAddress(null);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void EmptyAddress()
            {
                var email = new EmailAddress("   ");
            }

            [TestMethod]
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
                Assert.AreEqual(local, email.LocalPart);
                Assert.AreEqual(user, email.AccountPart);
                Assert.AreEqual(tag, email.TagPart);
                Assert.AreEqual(domain, email.DomainPart);
                Assert.AreEqual(address, email.Value);
            }

            [TestMethod]
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
                Assert.AreEqual(local, email.LocalPart);
                Assert.AreEqual(user, email.AccountPart);
                Assert.AreEqual(tag, email.TagPart);
                Assert.AreEqual(domain, email.DomainPart);
                Assert.AreEqual(address, email.Value);
            }

            [TestMethod]
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
                Assert.AreEqual(local, email.LocalPart);
                Assert.AreEqual(user, email.AccountPart);
                Assert.AreEqual(tag, email.TagPart);
                Assert.AreEqual(domain, email.DomainPart);
                Assert.AreEqual(address, email.Value);
            }

            [TestMethod]
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
                Assert.AreEqual(local, email.LocalPart);
                Assert.AreEqual(user, email.AccountPart);
                Assert.AreEqual(tag, email.TagPart);
                Assert.AreEqual(domain, email.DomainPart);
                Assert.AreEqual(address, email.Value);
            }
        }

        [TestClass]
        public class Constructor2
        {
            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void NullAccountPart()
            {
                // prepare
                string user = null;
                string tag = "";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(address);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void EmptyAccountPart()
            {
                // prepare
                string user = "";
                string tag = "";
                string domain = "sparklenetworks.com";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(address);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void NullDomainPart()
            {
                // prepare
                string user = "hello";
                string tag = "";
                string domain = null;
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(address);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void EmptyDomainPart()
            {
                // prepare
                string user = "hello";
                string tag = "";
                string domain = "";
                string local = user + (tag != null ? ("+" + tag) : "");
                string address = local + "@" + domain;

                // execute
                var email = new EmailAddress(address);
            }

            [TestMethod]
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
                Assert.AreEqual(local, email.LocalPart);
                Assert.AreEqual(user, email.AccountPart);
                Assert.AreEqual(tag, email.TagPart);
                Assert.AreEqual(domain, email.DomainPart);
                Assert.AreEqual(address, email.Value);
            }

            [TestMethod]
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
                Assert.AreEqual(local, email.LocalPart);
                Assert.AreEqual(user, email.AccountPart);
                Assert.AreEqual(tag, email.TagPart);
                Assert.AreEqual(domain, email.DomainPart);
                Assert.AreEqual(address, email.Value);
            }

            [TestMethod]
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
                Assert.AreEqual(local, email.LocalPart);
                Assert.AreEqual(user, email.AccountPart);
                Assert.AreEqual(tag, email.TagPart);
                Assert.AreEqual(domain, email.DomainPart);
                Assert.AreEqual(address, email.Value);
            }

            [TestMethod]
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
                Assert.AreEqual(local, email.LocalPart);
                Assert.AreEqual(user, email.AccountPart);
                Assert.AreEqual(null, email.TagPart);
                Assert.AreEqual(domain, email.DomainPart);
                Assert.AreEqual(address, email.Value);
            }
        }

        [TestClass]
        public class ValueWithoutTagGetter
        {
            [TestMethod]
            public void WorksWithNoTag()
            {
                // prepare
                string input = "antoine.alexandre@gmail.com";
                string expected = input;

                // execute
                var email = new EmailAddress(input);

                // verify
                Assert.AreEqual(expected, email.ValueWithoutTag);
            }

            [TestMethod]
            public void WorksWithOneTag()
            {
                // prepare
                string input = "antoine.alexandre+ojertuoir@gmail.com";
                string expected = "antoine.alexandre@gmail.com";

                // execute
                var email = new EmailAddress(input);

                // verify
                Assert.AreEqual(expected, email.ValueWithoutTag);
            }
        }

        [TestClass]
        public class ObjectEqualsMethod
        {
            [TestMethod]
            public void ObjectEqual_AgainstNull()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = null;
                var result = value1.Equals((object)value2);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ObjectEqual_AgainstSameReference()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = value1;
                var result = value1.Equals((object)value2);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ObjectEqual_AgainstSameValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("test@est.com");
                var result = value1.Equals((object)value2);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ObjectEqual_AgainstDifferentValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("lalala@test.com");
                var result = value1.Equals((object)value2);
                Assert.IsFalse(result);
            }
        }

        [TestClass]
        public class EmailAddressEqualsMethod
        {
            [TestMethod]
            public void ObjectEqual_AgainstNull()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = null;
                var result = value1.Equals(value2);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ObjectEqual_AgainstSameReference()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = value1;
                var result = value1.Equals(value2);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ObjectEqual_AgainstSameValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("test@est.com");
                var result = value1.Equals(value2);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ObjectEqual_AgainstDifferentValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("lalala@test.com");
                var result = value1.Equals(value2);
                Assert.IsFalse(result);
            }
        }

        [TestClass]
        public class OperatorEqualsMethod
        {
            [TestMethod]
            public void EqualityOperator_AgainstNull()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = null;
                var result = value1 == value2;
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void EqualityOperator_AgainstSameReference()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = value1;
                var result = value1 == value2;
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void EqualityOperator_AgainstSameValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("test@est.com");
                var result = value1 == value2;
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void EqualityOperator_AgainstDifferentValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("lalala@test.com");
                var result = value1 == value2;
                Assert.IsFalse(result);
            }
        }

        [TestClass]
        public class OperatorInEqualsMethod
        {
            [TestMethod]
            public void InequalityOperator_AgainstNull()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = null;
                var result = value1 != value2;
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void InequalityOperator_AgainstSameReference()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = value1;
                var result = value1 != value2;
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void InequalityOperator_AgainstSameValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("test@est.com");
                var result = value1 != value2;
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void InequalityOperator_AgainstDifferentValue()
            {
                EmailAddress value1 = new EmailAddress("test@est.com");
                EmailAddress value2 = new EmailAddress("lalala@test.com");
                var result = value1 != value2;
                Assert.IsTrue(result);
            }
        }
    }
}
