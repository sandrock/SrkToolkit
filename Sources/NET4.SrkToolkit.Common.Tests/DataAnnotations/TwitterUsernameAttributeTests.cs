
namespace Sparkle.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    [TestClass]
    public class TwitterUsernameAttributeTests
    {
        [TestClass]
        public class RegexTests
        {
            private Regex TwitterRegex;

            [TestInitialize]
            public void Initialize()
            {
                this.TwitterRegex = new Regex(TwitterUsernameAttribute.TwitterRegex, RegexOptions.Compiled);
            }

            [TestMethod]
            public void TwitterUsernameNoMatch()
            {
                Assert.IsFalse(this.TwitterRegex.IsMatch("é'çà&="));
                Assert.IsFalse(this.TwitterRegex.IsMatch("user@name"));
                Assert.IsFalse(this.TwitterRegex.IsMatch("usernamereallytoolong"));
            }

            [TestMethod]
            public void TwitterUsernameMatch()
            {
                Assert.IsTrue(this.TwitterRegex.IsMatch("username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("user_name"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("@username"));
            }

            [TestMethod]
            public void TwitterWeirdUrlMatch()
            {
                Assert.IsTrue(this.TwitterRegex.IsMatch("twitter/username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("twitter/@username"));
            }

            [TestMethod]
            public void TwitterUrlWithoutProtocolMatch()
            {
                Assert.IsTrue(this.TwitterRegex.IsMatch("twitter.com/username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("twitter.com/@username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("www.twitter.com/username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("www.twitter.com/@username"));
            }

            [TestMethod]
            public void TwitterUrlWithProtocolMatch()
            {
                Assert.IsTrue(this.TwitterRegex.IsMatch("http://twitter.com/username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("https://twitter.com/username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("http://twitter.com/@username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("https://twitter.com/@username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("http://www.twitter.com/username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("https://www.twitter.com/username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("http://www.twitter.com/@username"));
                Assert.IsTrue(this.TwitterRegex.IsMatch("https://www.twitter.com/@username"));
            }
        }

        [TestClass]
        public class GetUsernameMethod
        {
            [TestMethod]
            public void TwitterGetUsernameFail()
            {
                string username = null;

                Assert.IsFalse(TwitterUsernameAttribute.GetUsername("é'çà&=", out username));
                Assert.IsNull(username);
                Assert.IsFalse(TwitterUsernameAttribute.GetUsername("user@name", out username));
                Assert.IsNull(username);
                Assert.IsFalse(TwitterUsernameAttribute.GetUsername("usernamereallytoolong", out username));
                Assert.IsNull(username);
            }

            [TestMethod]
            public void TwitterGetUsernameSuccess()
            {
                string username = null;

                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("user_name", out username));
                Assert.AreEqual("user_name", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("@username", out username));
                Assert.AreEqual("username", username);
            }

            [TestMethod]
            public void TwitterGetUsernameFromWeirdUrl()
            {
                string username = null;

                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("twitter/username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("twitter/@username", out username));
                Assert.AreEqual("username", username);
            }

            [TestMethod]
            public void TwitterGetUsernameFromUrlWithoutProtocol()
            {
                string username = null;

                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("twitter.com/username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("twitter.com/@username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("www.twitter.com/username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("www.twitter.com/@username", out username));
                Assert.AreEqual("username", username);
            }

            [TestMethod]
            public void TwitterGetUsernameFromUrlWithProtocol()
            {
                string username = null;

                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("http://twitter.com/username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("http://twitter.com/@username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("http://www.twitter.com/username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("http://www.twitter.com/@username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("https://twitter.com/username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("https://twitter.com/@username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("https://www.twitter.com/username", out username));
                Assert.AreEqual("username", username);
                Assert.IsTrue(TwitterUsernameAttribute.GetUsername("https://www.twitter.com/@username", out username));
                Assert.AreEqual("username", username);
            }
        }
    }
}
