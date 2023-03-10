
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Globalization;
    using Xunit;

    public class EnumToolsTests
    {
        private static CultureInfo defaultCulture = new CultureInfo("en-US");
        private static CultureInfo frenchCulture = new CultureInfo("fr-FR");

        public class GetDescription
        {
            [Fact]
            public void HasDescription_ThreadCulture_AssembliesCulture()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);

                // verify
                Assert.Equal("Description of SuperEnum.Two", desc);
            }

            [Fact]
            public void NoDescription_ThreadCulture_AssembliesCulture()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Three;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);

                // verify
                Assert.Equal("Three", desc);
            }

            [Fact]
            public void HasDescription_SpecifiedCulture_AssembliesCulture()
            {
                Thread.CurrentThread.CurrentUICulture = frenchCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, defaultCulture);

                // verify
                Assert.Equal("Description of SuperEnum.Two", desc);
            }

            [Fact]
            public void NoDescription_SpecifiedCulture_AssembliesCulture()
            {
                Thread.CurrentThread.CurrentUICulture = frenchCulture;

                // prepare
                var value = SuperEnum.Three;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, defaultCulture);

                // verify
                Assert.Equal("Three", desc);
            }

            [Fact]
            public void HasDescription_ThreadCulture_FrenchCulture()
            {
                Thread.CurrentThread.CurrentUICulture = frenchCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);

                // verify
                Assert.Equal("Description de SuperEnum.Two", desc);
            }

            [Fact]
            public void NoDescription_ThreadCulture_FrenchCulture()
            {
                Thread.CurrentThread.CurrentUICulture = frenchCulture;

                // prepare
                var value = SuperEnum.Three;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager);

                // verify
                Assert.Equal("Three", desc);
            }

            [Fact]
            public void HasDescription_SpecifiedCulture_FrenchCulture()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, frenchCulture);

                // verify
                Assert.Equal("Description de SuperEnum.Two", desc);
            }

            [Fact]
            public void NoDescription_SpecifiedCulture_FrenchCulture()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Three;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, frenchCulture);

                // verify
                Assert.Equal("Three", desc);
            }

            [Fact]
            public void HasDescription_ThreadCulture_AssembliesCulture_Suffix()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, null, "_Desc");

                // verify
                Assert.Equal("Super long description of the SuperEnum.Two enum value. lalalalala allalalal alallalalal alal allaall allalallalala.", desc);
            }

            [Fact]
            public void HasDescription_ThreadCulture_AssembliesCulture_Prefix()
            {
                Thread.CurrentThread.CurrentUICulture = defaultCulture;

                // prepare
                var value = SuperEnum.Two;

                // execute
                var desc = EnumTools.GetDescription(value, EnumStrings.ResourceManager, "Def_", null);

                // verify
                Assert.Equal("Description of SuperEnum.Two blah", desc);
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
