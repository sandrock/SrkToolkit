
namespace Sparkle.UnitTests
{
    using SrkToolkit.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Xunit;

    public class TwitterUsernameAttributeTests
    {
        public class RegexTests
        {
            private Regex TwitterRegex;

            public RegexTests()
            {
                this.TwitterRegex = new Regex(TwitterUsernameAttribute.TwitterRegex, RegexOptions.Compiled);
            }

            [Fact]
            public void TwitterUsernameNoMatch()
            {
                Assert.False(this.TwitterRegex.IsMatch("é'çà&="));
                Assert.False(this.TwitterRegex.IsMatch("user@name"));
                Assert.False(this.TwitterRegex.IsMatch("usernamereallytoolong"));
            }

            [Fact]
            public void TwitterUsernameMatch()
            {
                Assert.True(this.TwitterRegex.IsMatch("username"));
                Assert.True(this.TwitterRegex.IsMatch("user_name"));
                Assert.True(this.TwitterRegex.IsMatch("@username"));
            }

            [Fact]
            public void TwitterWeirdUrlMatch()
            {
                Assert.True(this.TwitterRegex.IsMatch("twitter/username"));
                Assert.True(this.TwitterRegex.IsMatch("twitter/@username"));
            }

            [Fact]
            public void TwitterUrlWithoutProtocolMatch()
            {
                Assert.True(this.TwitterRegex.IsMatch("twitter.com/username"));
                Assert.True(this.TwitterRegex.IsMatch("twitter.com/@username"));
                Assert.True(this.TwitterRegex.IsMatch("www.twitter.com/username"));
                Assert.True(this.TwitterRegex.IsMatch("www.twitter.com/@username"));
            }

            [Fact]
            public void TwitterUrlWithProtocolMatch()
            {
                Assert.True(this.TwitterRegex.IsMatch("http://twitter.com/username"));
                Assert.True(this.TwitterRegex.IsMatch("https://twitter.com/username"));
                Assert.True(this.TwitterRegex.IsMatch("http://twitter.com/@username"));
                Assert.True(this.TwitterRegex.IsMatch("https://twitter.com/@username"));
                Assert.True(this.TwitterRegex.IsMatch("http://www.twitter.com/username"));
                Assert.True(this.TwitterRegex.IsMatch("https://www.twitter.com/username"));
                Assert.True(this.TwitterRegex.IsMatch("http://www.twitter.com/@username"));
                Assert.True(this.TwitterRegex.IsMatch("https://www.twitter.com/@username"));
            }
        }

        public class GetUsernameMethod
        {
            [Fact]
            public void TwitterGetUsernameFail()
            {
                string username = null;

                Assert.False(TwitterUsernameAttribute.GetUsername("é'çà&=", out username));
                Assert.Null(username);
                Assert.False(TwitterUsernameAttribute.GetUsername("user@name", out username));
                Assert.Null(username);
                Assert.False(TwitterUsernameAttribute.GetUsername("usernamereallytoolong", out username));
                Assert.Null(username);
            }

            [Fact]
            public void TwitterGetUsernameSuccess()
            {
                string username = null;

                Assert.True(TwitterUsernameAttribute.GetUsername("username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("user_name", out username));
                Assert.Equal("user_name", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("@username", out username));
                Assert.Equal("username", username);
            }

            [Fact]
            public void TwitterGetUsernameFromWeirdUrl()
            {
                string username = null;

                Assert.True(TwitterUsernameAttribute.GetUsername("twitter/username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("twitter/@username", out username));
                Assert.Equal("username", username);
            }

            [Fact]
            public void TwitterGetUsernameFromUrlWithoutProtocol()
            {
                string username = null;

                Assert.True(TwitterUsernameAttribute.GetUsername("twitter.com/username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("twitter.com/@username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("www.twitter.com/username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("www.twitter.com/@username", out username));
                Assert.Equal("username", username);
            }

            [Fact]
            public void TwitterGetUsernameFromUrlWithProtocol()
            {
                string username = null;

                Assert.True(TwitterUsernameAttribute.GetUsername("http://twitter.com/username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("http://twitter.com/@username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("http://www.twitter.com/username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("http://www.twitter.com/@username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("https://twitter.com/username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("https://twitter.com/@username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("https://www.twitter.com/username", out username));
                Assert.Equal("username", username);
                Assert.True(TwitterUsernameAttribute.GetUsername("https://www.twitter.com/@username", out username));
                Assert.Equal("username", username);
            }
        }

        [Fact]
        public void ObviousInvalid()
        {
            var attr = new TwitterUsernameAttribute();
            string value = "foo bar", name = "Twitter";
            var context = new ValidationContext(new object(), null, null);
            context.MemberName = name;
            var result = attr.GetValidationResult(value, context);
            Assert.NotNull(result);
            Assert.Equal(@"Invalid Twitter username. You must provide the username or the url to your account.", result.ErrorMessage);
            Assert.Equal(1, result.MemberNames.Count());
            Assert.Equal(name, result.MemberNames.Single());
        }
    }
}
