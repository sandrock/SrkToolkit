
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
        public class Constructor
        {
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
    }
}
