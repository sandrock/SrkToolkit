
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading;
    using System.Globalization;

    [TestClass]
    public class EnumToolsTests
    {
        private static CultureInfo defaultCulture = new CultureInfo("en-US");
        private static CultureInfo frenchCulture = new CultureInfo("fr-FR");

        [TestClass]
        public class GetDescription
        {

            [TestMethod]
            public void HasDescription_ThreadCulture_AssembliesCulture()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);

                // verify
                Assert.AreEqual("Description of SuperEnum.Two", desc);
            }

            [TestMethod]
            public void NoDescription_ThreadCulture_AssembliesCulture()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Three;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);

                // verify
                Assert.AreEqual("Three", desc);
            }

            [TestMethod]
            public void HasDescription_SpecifiedCulture_AssembliesCulture()
            {
                Thread.CurrentThread.CurrentUICulture = frenchCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, defaultCulture);

                // verify
                Assert.AreEqual("Description of SuperEnum.Two", desc);
            }

            [TestMethod]
            public void NoDescription_SpecifiedCulture_AssembliesCulture()
            {
                Thread.CurrentThread.CurrentUICulture = frenchCulture;

                // prepare
                var value = SuperEnum.Three;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, defaultCulture);

                // verify
                Assert.AreEqual("Three", desc);
            }

            [TestMethod]
            public void HasDescription_ThreadCulture_FrenchCulture()
            {
                Thread.CurrentThread.CurrentUICulture = frenchCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);

                // verify
                Assert.AreEqual("Description de SuperEnum.Two", desc);
            }

            [TestMethod]
            public void NoDescription_ThreadCulture_FrenchCulture()
            {
                Thread.CurrentThread.CurrentUICulture = frenchCulture;

                // prepare
                var value = SuperEnum.Three;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);

                // verify
                Assert.AreEqual("Three", desc);
            }

            [TestMethod]
            public void HasDescription_SpecifiedCulture_FrenchCulture()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, frenchCulture);

                // verify
                Assert.AreEqual("Description de SuperEnum.Two", desc);
            }

            [TestMethod]
            public void NoDescription_SpecifiedCulture_FrenchCulture()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Three;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, frenchCulture);

                // verify
                Assert.AreEqual("Three", desc);
            }
        
        }

        public enum SuperEnum
        {
            MinusOne = -1,
            None = 0,
            One,
            Two,
            Three,
            Ten = 10,
            Eleven,
        }
    }
}
