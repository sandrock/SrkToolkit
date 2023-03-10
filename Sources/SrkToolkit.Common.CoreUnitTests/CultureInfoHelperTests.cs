
namespace SrkToolkit.Common.Tests
{
    using SrkToolkit.Globalization;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class CultureInfoHelperTests
    {
        public class GetCountriesMethod
        {
            static IList<RegionInfo> countries = CultureInfoHelper.GetCountries();

            [Fact]
            public void HasUK()
            {
                string nativeName = "United Kingdom";
                string englishName = "United Kingdom";

                var match = countries.SingleOrDefault(c => c.NativeName == nativeName);
                Assert.NotNull(match);
                Assert.Equal(englishName, match.EnglishName);
                Assert.Equal(nativeName, match.NativeName);
            }

            [Fact]
            public void HasUSA()
            {
                string nativeName = "United States";
                string englishName = "United States";

                var match = countries.SingleOrDefault(c => c.NativeName == nativeName);
                Assert.NotNull(match);
                Assert.Equal(englishName, match.EnglishName);
                Assert.Equal(nativeName, match.NativeName);
            }

            [Fact]
            public void HasFrance()
            {
                string nativeName = "France";
                string englishName = "France";

                var match = countries.SingleOrDefault(c => c.NativeName == nativeName);
                Assert.NotNull(match);
                Assert.Equal(englishName, match.EnglishName);
                Assert.Equal(nativeName, match.NativeName);
            }

            [Fact] // something is wrong
            public void HasChina()
            {
                string englishName = "China";

                var match = countries.SingleOrDefault(c => c.EnglishName == englishName);
                Assert.NotNull(match);
            }

            [Fact] // something is wrong
            public void HasChina2()
            {
                string nativeName = "ཀྲུང་ཧྭ་མི་དམངས་སྤྱི་མཐུན་རྒྱལ་ཁབ";
                string englishName = "China";
                string cultureName = "zh-CN";

                var match = countries.SingleOrDefault(c => c.EnglishName == englishName);
                Assert.NotNull(match);
            }
        
        }
    }
}
