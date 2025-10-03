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

using SrkToolkit.Domain.Tests.Models;

namespace SrkToolkit.Domain.Tests
{
    using SrkToolkit.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class BasicResultTests
    {
        public class AddExtension
        {
            [Fact]
            public void EnumValue()
            {
                var value = Lalala.One;
                var resourceManager = Strings.ResourceManager;
                var expected = "Hello World";
                IList<BasicResultError> list = new List<BasicResultError>();
                list.Add(value, resourceManager);
                var result = list[0].DisplayMessage;
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void SucceedWhenForced()
        {
            var targetResult = new BasicResult<Error1>();
            Assert.True(targetResult.Succeed);
            targetResult.Errors.Add(Error1.Error42, "The detail.", "The error to all failed algorithms.");
            Assert.False(targetResult.Succeed);
            targetResult.Succeed = true;
            Assert.True(targetResult.Succeed);
            targetResult.Succeed = false;
            Assert.False(targetResult.Succeed);
        }

        public enum Lalala
        {
            None,
            One,
            Many,
            Infinity,
        }
    }
}
