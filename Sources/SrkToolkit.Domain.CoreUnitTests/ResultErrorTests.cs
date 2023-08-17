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

namespace SrkToolkit.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Resources;
    using Xunit;

    public class ResultErrorTests
    {
        public class Ctor
        {
            [Fact]
            public void WithResourceManager()
            {
                var value = Lalala.One;
                var resourceManager = Strings.ResourceManager;
                var expected = "Hello World";
                var result = new ResultError<Lalala>(value, resourceManager);
                Assert.Equal(value, result.Code);
                Assert.Equal(expected, result.DisplayMessage);
            }

            [Fact]
            public void WithoutResourceManager()
            {
                var value = Lalala.One;
                var resourceManager = "Hello World";
                var expected = "Hello World";
                var result = new ResultError<Lalala>(value, resourceManager);
                Assert.Equal(value, result.Code);
                Assert.Equal(expected, result.DisplayMessage);
            }

            [Fact]
            public void WithResourceManagerAndFormat()
            {
                var value = Lalala.Infinity;
                var resourceManager = Strings.ResourceManager;
                var expected = "here is a format => aabb <=";
                var result = new ResultError<Lalala>(value, resourceManager, "aabb");
                Assert.Equal(value, result.Code);
                Assert.Equal(expected, result.DisplayMessage);
            }

            [Fact]
            public void WithoutResourceManagerAndFormat()
            {
                var value = Lalala.Infinity;
                var resourceManager = "here is a format => {0} <=";
                var expected = "here is a format => aabb <=";
                var result = new ResultError<Lalala>(value, resourceManager, "aabb");
                Assert.Equal(value, result.Code);
                Assert.Equal(expected, result.DisplayMessage);
            }

            [Fact]
            public void WithNestedType()
            {
                var value = NestedClass.Lululu.One;
                var resourceManager = Strings.ResourceManager;
                var expected = "Hello World";
                var result = new ResultError<NestedClass.Lululu>(value, resourceManager);
                Assert.Equal(value, result.Code);
                Assert.Equal(expected, result.DisplayMessage);
            }
        }

        public class AddExtension
        {
            [Fact]
            public void ThrowsIfArg0IsNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    ResultErrorExtensions.Add(default(IList<ResultError<Lalala>>), Lalala.Many, Strings.ResourceManager);
                });
            }

            [Fact]
            public void Works()
            {
                var value = Lalala.One;
                var resourceManager = Strings.ResourceManager;
                var expected = "Hello World";
                var list = new List<ResultError<Lalala>>();
                list.Add(value, resourceManager);
                var result = list[0].DisplayMessage;
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Format()
            {
                var value = Lalala.Infinity;
                var resourceManager = Strings.ResourceManager;
                var expected = "here is a format => test <=";
                var list = new List<ResultError<Lalala>>();
                list.Add(value, resourceManager, "test");
                var result = list[0].DisplayMessage;
                Assert.Equal(expected, result);
            }
        }

        public class ContainsErrorExtension
        {
            [Fact]
            public void ReturnsFalseIfCodeIsNotPresent()
            {
                var errors = new List<ResultError<Lalala>>();
                errors.Add(new ResultError<Lalala>(Lalala.One, Strings.ResourceManager));
                Assert.False(ResultErrorExtensions.ContainsError(errors, Lalala.Many));
            }

            [Fact]
            public void ReturnsTrueIfCodeIsNotPresent()
            {
                var errors = new List<ResultError<Lalala>>();
                errors.Add(new ResultError<Lalala>(Lalala.One, Strings.ResourceManager));
                Assert.True(ResultErrorExtensions.ContainsError(errors, Lalala.One));
            }

            [Fact]
            public void ThrowsIfArg0IsNull()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    ResultErrorExtensions.ContainsError(default(IList<ResultError<Lalala>>), Lalala.Many);
                });
            }
        }

        public class WithPostProcessExtension
        {
            [Fact]
            public void Works()
            {
                var value = Lalala.One;
                var resourceManager = Strings.ResourceManager;
                var expected = "[Hello World]";
                var list = new List<ResultError<Lalala>>();
                list.WithPostProcess(str => "[" + str + "]").Add(value, resourceManager);
                var result = list[0].DisplayMessage;
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Format()
            {
                var value = Lalala.Infinity;
                var resourceManager = Strings.ResourceManager;
                var expected = "[here is a format => test <=]";
                var list = new List<ResultError<Lalala>>();
                list.WithPostProcess(str => "[" + str + "]").Add(value, resourceManager, "test");
                var result = list[0].DisplayMessage;
                Assert.Equal(expected, result);
            }
        }

        public enum Lalala
        {
            None,
            One,
            Many,
            Infinity,
        }

        public enum Lololo
        {
            Unknown,
            Known,
            Other,
        }

        public class NestedClass
        {
            public enum Lululu
            {
                None,
                One,
                Many,
                Infinity,
            }
        }
    }
}
