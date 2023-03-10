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
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Xunit;

    public class SrkMvcCollectionExtensionsTests
    {
        public class ToSelectListMethod
        {
            [Fact]
            public void NullCollectionThrows()
            {
                List<object> collection = null;

                Assert.Throws<ArgumentNullException>(() =>
                {
                    var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.ToString(), o => o.ToString(), o => false);
                });
            }

            [Fact]
            public void NullValueSelectorThrows()
            {
                List<object> collection = null;
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var result = SrkMvcCollectionExtensions.ToSelectList(collection, null, o => o.ToString(), o => false);
                });
            }

            [Fact]
            public void NullDisplaySelectorThrows()
            {
                List<object> collection = null;
                Assert.Throws<ArgumentNullException>(() =>
                {
                    var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.ToString(), null, o => false);
                });
            }

            [Fact]
            public void NullSelectedSelectorDoesNotThrow()
            {
                List<object> collection = new List<object>();
                var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.ToString(), o => o.ToString(), null);
            }

            [Fact]
            public void SimpleListAndNullSelectedSelector()
            {
                List<Tuple<int, string>> collection = GetSimpleList();
                var result = SrkMvcCollectionExtensions.ToSelectList(collection, o => o.Item1.ToString(), o => o.Item2, null);
                Verify(collection, result, null);
            }

            [Fact]
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
                Assert.Equal(collection.Count, result.Count());
                for (int i = 0; i < collection.Count; i++)
                {
                    var item = result.Skip(i).First();
                    Assert.Equal(collection[i].Item1.ToString(), item.Value);
                    Assert.Equal(collection[i].Item2.ToString(), item.Text);

                    if (selected != null && selected.Value == collection[i].Item1)
                        Assert.True(item.Selected);
                    else
                        Assert.False(item.Selected);
                }
            }
        }
    }
}
