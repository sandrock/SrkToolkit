// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

namespace SrkToolkit.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class StringComparerExTests
    {
        [TestMethod]
        public void StringComparerEx_AccentedStringAreEqual1()
        {
            string a = "écoûtèr", b = "ëcöutêr";
            var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            bool result = stringComparer.Equals(a, b);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StringComparerEx_AccentedStringAreEqual2()
        {
            string a = "écoûtèr", b = "écoûtèr";
            var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            bool result = stringComparer.Equals(a, b);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StringComparerEx_AccentedStringAreEqual3()
        {
            string a = "écoûtèr", b = "ecouter";
            var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            bool result = stringComparer.Equals(a, b);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StringComparerEx_DifferentStringAreDifferent()
        {
            string a = "acoutur", b = "ëcöutêr";
            var stringComparer = new StringComparerEx(CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            bool result = stringComparer.Equals(a, b);
            Assert.IsFalse(result);
        }
    }
}
