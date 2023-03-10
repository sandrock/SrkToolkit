
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class StringReplacerTests
    {
        [Fact]
        public void Simple()
        {
            var target = new StringReplacer();
            target.Setup("Hello", r => "World");
            var text = "Hello {Hello}";
            var expected = "Hello World";
            var result = target.Replace(text);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleAndDefault()
        {
            var target = new StringReplacer();
            target.Setup("Hello", r => "World");
            var text = "Hello {Hello} {Other}";
            var expected = "Hello World ";
            var result = target.Replace(text);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleAndCustomDefault()
        {
            var target = new StringReplacer();
            target.Setup("Hello", r => "World");
            target.SetupDefault(r => "???");
            var text = "Hello {Hello} {Other}";
            var expected = "Hello World ???";
            var result = target.Replace(text);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleWithCulture()
        {
            var model = new UserModel
            {
                Date = new DateTime(2015,2,15,16,42,16,0, DateTimeKind.Unspecified),
            };
            var target = new StringReplacer();
            target.Setup("User.Date", r => model.Date.ToString(r.Parameter ?? "d", r.Culture));
            var text = "Hello {User.Date}";
            var expected = "Hello 2/15/2015 4:42:16 PM";
            var culture = new CultureInfo("en-US");
            var tz = TimeZoneInfo.Local;
            var result = target.Replace(text, culture, tz);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SimpleWithCulture1()
        {
            var model = new UserModel
            {
                Date = new DateTime(2015,2,15,16,42,16,0, DateTimeKind.Unspecified),
            };
            var target = new StringReplacer();
            target.Setup("User.Date", r => model.Date.ToString(r.Parameter ?? "d", r.Culture));
            var text = "Hello {User.Date}";
            var expected = "Hello 15/02/2015 16:42:16";
            var culture = new CultureInfo("fr-FR");
            var tz = TimeZoneInfo.Local;
            var result = target.Replace(text, culture, tz);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Model()
        {
            var model = new UserModel
            {
                Name = "Johny",
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Name", r => r.Model.Name);
            var text = "Hello {User.Name}";
            var expected = "Hello Johny";
            var result = target.Replace(text, model);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ModelAndDefault()
        {
            var model = new UserModel
            {
                Name = "Johny",
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Name", r => r.Model.Name);
            var text = "Hello {User.Name} {Other}";
            var expected = "Hello Johny ";
            var result = target.Replace(text, model);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ModelAndCustomDefault()
        {
            var model = new UserModel
            {
                Name = "Johny",
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Name", r => r.Model.Name);
            target.SetupDefault(r => "??" + r.Key + "??");
            var text = "Hello {User.Name} {Other}";
            var expected = "Hello Johny ??Other??";
            var result = target.Replace(text, model);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ModelWithCulture()
        {
            var model = new UserModel
            {
                Date = new DateTime(2015,2,15,16,42,16,0, DateTimeKind.Unspecified),
            };
            var culture = new CultureInfo("en-US");
            var tz = TimeZoneInfo.Local;
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Date", r => r.Model.Date.ToString(r.Parameter ?? "d", r.Culture));
            var text = "Hello {User.Date}";
            var expected = "Hello " + model.Date.ToString("d", culture);
            var result = target.Replace(text, model, culture, tz);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ModelWithReplacerCulture_EnglishUSA()
        {
            var model = new UserModel
            {
                Date = new DateTime(2015, 2, 15, 16, 42, 16, 0, DateTimeKind.Unspecified),
            };
            var culture = new CultureInfo("en-US");
            var tz = TimeZoneInfo.Local;
            var target = new StringReplacer<UserModel>(culture, tz);
            target.Setup("User.Date", r => r.Model.Date.ToString(r.Parameter ?? "d", r.Culture));
            var text = "Hello {User.Date}";
            var expected = "Hello " + model.Date.ToString("d", culture);
            var result = target.Replace(text, model);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ModelWithReplacerCulture_FrenchFrance()
        {
            var model = new UserModel
            {
                Date = new DateTime(2015, 2, 15, 16, 42, 16, 0, DateTimeKind.Unspecified),
            };
            var culture = new CultureInfo("fr-FR");
            var tz = TimeZoneInfo.Local;
            var target = new StringReplacer<UserModel>(culture, tz);
            target.Setup("User.Date", r => r.Model.Date.ToString(r.Parameter ?? "d", r.Culture));
            var text = "Hello {User.Date}";
            var expected = "Hello " + model.Date.ToString("d", culture);
            var result = target.Replace(text, model);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ModelWithCulture1()
        {
            var model = new UserModel
            {
                Date = new DateTime(2015,2,15,16,42,16,0, DateTimeKind.Unspecified),
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Date", r => r.Model.Date.ToString(r.Parameter ?? "d", r.Culture));
            var text = "Hello {User.Date}";
            var expected = "Hello 15/02/2015 16:42:16";
            var culture = new CultureInfo("fr-FR");
            var tz = TimeZoneInfo.Local;
            var result = target.Replace(text, model, culture, tz);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ModelWithTimezone1()
        {
            var model = new UserModel
            {
                Date = new DateTime(2015,2,15,16,42,16,0, DateTimeKind.Unspecified),
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Date", r => r.Timezone.ConvertFromUtc(r.Model.Date).ToString(r.Parameter ?? "d", r.Culture));
            var text = "Hello {User.Date}";
            var expected = "Hello 2/15/2015 4:42:16 PM";
            var culture = new CultureInfo("en-US");
            var tz = TimeZoneInfo.Utc;
            var result = target.Replace(text, model, culture, tz);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ModelWithTimezone2()
        {
            var model = new UserModel
            {
                Date = new DateTime(2015,2,15,16,42,16,0, DateTimeKind.Unspecified),
            };
            var target = new StringReplacer<UserModel>();
            target.Setup("User.Date", r => r.Timezone.ConvertFromUtc(r.Model.Date).ToString(r.Parameter ?? "d", r.Culture));
            var text = "Hello {User.Date}";
            var expected = "Hello 2/15/2015 5:42:16 PM";
            var culture = new CultureInfo("en-US");
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            var result = target.Replace(text, model, culture, tz);
            Assert.Equal(expected, result);
        }

        public class UserModel
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
