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

namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class SrkMvcCollectionExtensionsTests
    {
        [TestClass]
        public class ToSelectListMethod
        {
            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void NullCollectionThrows()
            {
                List<object> collection = null;
                var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.ToString(), o => o.ToString(), o => false);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void NullValueSelectorThrows()
            {
                List<object> collection = null;
                var result = SrkMvcCollectionExtensions.ToSelectList(collection, null, o => o.ToString(), o => false);
            }

            [TestMethod, ExpectedException(typeof(ArgumentNullException))]
            public void NullDisplaySelectorThrows()
            {
                List<object> collection = null;
                var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.ToString(), null, o => false);
            }

            [TestMethod]
            public void NullSelectedSelectorDoesNotThrow()
            {
                List<object> collection = new List<object>();
                var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.ToString(), o => o.ToString(), null);
            }

            [TestMethod]
            public void SimpleListAndNullSelectedSelector()
            {
                List<Tuple<int, string>> collection = GetSimpleList();
                var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.Item1.ToString(), o => o.Item2, null);
                Verify(collection, result, null);
            }

            [TestMethod]
            public void SimpleListWithSelectedSelector()
            {
                List<Tuple<int, string>> collection = GetSimpleList();
                var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.Item1.ToString(), o => o.Item2, o => o.Item1 == 2);
                Verify(collection, result, 2);
            }

            private static List<Tuple<int, string>> GetSimpleList()
            {
                List<Tuple<int, string>> collection = null;
                collection = new List<Tuple<int, string>>()
                {
                    new Tuple<int, string>(1, "hello 1"),
                    new Tuple<int, string>(2, "hello 2"),
                    new Tuple<int, string>(3, "hello 3"),
                    new Tuple<int, string>(4, "hello 4"),
                };
                return collection;
            }

            private static void Verify(List<Tuple<int, string>> collection, IList<SelectListItem> result, int? selected)
            {
                Assert.AreEqual(collection.Count, result.Count());
                for (int i = 0; i < collection.Count; i++)
                {
                    var item = result.Skip(i).First();
                    Assert.AreEqual(collection[i].Item1.ToString(), item.Value);
                    Assert.AreEqual(collection[i].Item2.ToString(), item.Text);

                    if (selected != null && selected.Value == collection[i].Item1)
                        Assert.IsTrue(item.Selected);
                    else
                        Assert.IsFalse(item.Selected);
                }
            }
        }
    }
}
