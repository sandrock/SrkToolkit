
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Globalization;

    public class CultureInfoHelperTests
    {
        [TestClass]
        public class GetCountriesMethod
        {
            static IList<RegionInfo> countries = CultureInfoHelper.GetCountries();

            [TestMethod]
            public void HasUK()
            {
                string nativeName = "United Kingdom";
                string englishName = "United Kingdom";

                var match = countries.SingleOrDefault(c => c.NativeName == nativeName);
                Assert.IsNotNull(match);
                Assert.AreEqual(englishName, match.EnglishName);
                Assert.AreEqual(nativeName, match.NativeName);
            }

            [TestMethod]
            public void HasUSA()
            {
                string nativeName = "United States";
                string englishName = "United States";

                var match = countries.SingleOrDefault(c => c.NativeName == nativeName);
                Assert.IsNotNull(match);
                Assert.AreEqual(englishName, match.EnglishName);
                Assert.AreEqual(nativeName, match.NativeName);
            }

            [TestMethod]
            public void HasFrance()
            {
                string nativeName = "France";
                string englishName = "France";

                var match = countries.SingleOrDefault(c => c.NativeName == nativeName);
                Assert.IsNotNull(match);
                Assert.AreEqual(englishName, match.EnglishName);
                Assert.AreEqual(nativeName, match.NativeName);
            }

            [TestMethod] // something is wrong
            public void HasChina()
            {
                string englishName = "China";

                var match = countries.SingleOrDefault(c => c.EnglishName == englishName);
                Assert.IsNotNull(match);
            }

            [TestMethod] // something is wrong
            public void HasChina2()
            {
                string nativeName = "ཀྲུང་ཧྭ་མི་དམངས་སྤྱི་མཐུན་རྒྱལ་ཁབ";
                string englishName = "China";
                string cultureName = "zh-CN";

                var match = countries.SingleOrDefault(c => c.EnglishName == englishName);
                Assert.IsNotNull(match);
            }
        
        }
    }
}
