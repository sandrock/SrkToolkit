
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

            [TestMethod]
            public void DoesCultureExist_YesFrenchFrance()
            {
                string frenchFrance = "fr-FR";
                var exists = CultureInfoHelper.DoesCultureExist(frenchFrance);
                Assert.IsTrue(exists);
            }

            [TestMethod]
            public void DoesCultureExist_YesEnglishUs()
            {
                string englishUs = "en-US";
                var exists = CultureInfoHelper.DoesCultureExist(englishUs);
                Assert.IsTrue(exists);
            }

            [TestMethod]
            public void DoesCultureExist_YesCustomCulture()
            {
                string frenchRU = "fr-RU";
                var cultureBuilder = new CultureAndRegionInfoBuilder(frenchRU, CultureAndRegionModifiers.None);
                cultureBuilder.LoadDataFromCultureInfo(CultureInfo.CreateSpecificCulture("fr-FR"));
                cultureBuilder.LoadDataFromRegionInfo(new RegionInfo("ru-RU"));
                cultureBuilder.CultureEnglishName = "French (Russia)";
                cultureBuilder.CultureNativeName = "France (Russie)";
                cultureBuilder.CurrencyNativeName = "Euro";
                cultureBuilder.RegionNativeName = "Россия";

                // Register the culture.
                try
                {
                    cultureBuilder.Register();
                }
                catch (InvalidOperationException)
                {
                    // Swallow the exception: the culture already is registered.
                }

                var exists = CultureInfoHelper.DoesCultureExist(frenchRU);
                Assert.IsTrue(exists);
            }

            [TestMethod]
            public void DoesCultureExist_NoUnknownCulture()
            {
                string unknownCulture = "zz-ZZ";
                var exists = CultureInfoHelper.DoesCultureExist(unknownCulture);
                Assert.IsFalse(exists);
            }

            [TestMethod]
            public void DoesCultureExist_NoGarbageString()
            {
                string garbage = "coucou";
                var exists = CultureInfoHelper.DoesCultureExist(garbage);
                Assert.IsFalse(exists);
            }

            [TestMethod]
            public void DoesCultureExist_NoNullValue()
            {
                string nullString = null;
                var exists = CultureInfoHelper.DoesCultureExist(nullString);
                Assert.IsFalse(exists);
            }

            [TestMethod]
            public void DoesCultureExist_ConsiderInvariantCulture_YesEmptyValue()
            {
                string emptyString = string.Empty;
                var exists = CultureInfoHelper.DoesCultureExist(emptyString);
                Assert.IsTrue(exists);
            }

            [TestMethod]
            public void DoesCultureExist_DoNotConsiderInvariantCulture_NoEmptyValue()
            {
                string emptyString = string.Empty;
                var exists = CultureInfoHelper.DoesCultureExist(emptyString, false);
                Assert.IsFalse(exists);
            }
        }
    }
}
