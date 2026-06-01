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

        public class SucceedProperty
        {
            [Fact]
            public void DefaultsToFalse()
            {
                var target = new BasicResult();
                Assert.False(target.Succeed);
            }

            [Fact]
            public void CanBeSetToTrue()
            {
                var target = new BasicResult();
                target.Succeed = true;
                Assert.True(target.Succeed);
            }

            [Fact]
            public void DefaultsToFalse_Generic()
            {
                var target = new BasicResult<Lalala>();
                Assert.False(target.Succeed);
            }

            [Fact]
            public void CanBeSetToTrue_Generic()
            {
                var target = new BasicResult<Lalala>();
                target.Succeed = true;
                Assert.True(target.Succeed);
            }
        }

        public class ErrorsProperty
        {
            [Fact]
            public void EmptyByDefault()
            {
                var target = new BasicResult();
                Assert.Empty(target.Errors);
            }

            [Fact]
            public void LazilyInitialized()
            {
                var target = new BasicResult();
                var first = target.Errors;
                var second = target.Errors;
                Assert.Same(first, second);
            }

            [Fact]
            public void CanSetNewList()
            {
                var target = new BasicResult();
                var list = new List<BasicResultError> { new BasicResultError("code", "msg") };
                target.Errors = list;
                Assert.Equal(1, target.Errors.Count);
            }

            [Fact]
            public void EmptyByDefault_Generic()
            {
                var target = new BasicResult<Lalala>();
                Assert.Empty(target.Errors);
            }
        }

        public class AddErrorMethod
        {
            [Fact]
            public void AddsOneError()
            {
                var target = new BasicResult();
                var error = new BasicResultError("ERR01", "Something went wrong", "detail text");
                target.AddError(error);
                Assert.Equal(1, target.Errors.Count);
            }

            [Fact]
            public void CopiesCode()
            {
                var target = new BasicResult();
                target.AddError(new BasicResultError("ERR01", "msg", "detail"));
                Assert.Equal("ERR01", target.Errors[0].Code);
            }

            [Fact]
            public void CopiesDisplayMessage()
            {
                var target = new BasicResult();
                target.AddError(new BasicResultError("ERR01", "Something went wrong", "detail"));
                Assert.Equal("Something went wrong", target.Errors[0].DisplayMessage);
            }

            [Fact]
            public void CopiesDetail()
            {
                var target = new BasicResult();
                target.AddError(new BasicResultError("ERR01", "msg", "detail text"));
                Assert.Equal("detail text", target.Errors[0].Detail);
            }

            [Fact]
            public void CreatesNewInstance()
            {
                var target = new BasicResult();
                var original = new BasicResultError("ERR01", "msg", "detail");
                target.AddError(original);
                Assert.NotSame(original, target.Errors[0]);
            }

            [Fact]
            public void CanAddMultiple()
            {
                var target = new BasicResult();
                target.AddError(new BasicResultError("ERR01", "First error", null));
                target.AddError(new BasicResultError("ERR02", "Second error", null));
                Assert.Equal(2, target.Errors.Count);
                Assert.Equal("ERR01", target.Errors[0].Code);
                Assert.Equal("ERR02", target.Errors[1].Code);
            }

            [Fact]
            public void NullFieldsAreCopied()
            {
                var target = new BasicResult();
                target.AddError(new BasicResultError(null, null, null));
                Assert.Single(target.Errors);
                Assert.Null(target.Errors[0].Code);
                Assert.Null(target.Errors[0].DisplayMessage);
                Assert.Null(target.Errors[0].Detail);
            }
        }

        public class AddErrorMethod_Generic
        {
            [Fact]
            public void AddsOneError()
            {
                var target = new BasicResult<Lalala>();
                target.AddError(Lalala.One, "Something went wrong", "detail");
                Assert.Equal(1, target.Errors.Count);
            }

            [Fact]
            public void CopiesCode()
            {
                var target = new BasicResult<Lalala>();
                target.AddError(Lalala.Infinity, "msg", "detail");
                Assert.Equal(Lalala.Infinity, target.Errors[0].Code);
            }

            [Fact]
            public void CopiesDisplayMessage()
            {
                var target = new BasicResult<Lalala>();
                target.AddError(Lalala.One, "Something went wrong", "detail");
                Assert.Equal("Something went wrong", target.Errors[0].DisplayMessage);
            }

            [Fact]
            public void CopiesDetail()
            {
                var target = new BasicResult<Lalala>();
                target.AddError(Lalala.One, "msg", "detail text");
                Assert.Equal("detail text", target.Errors[0].Detail);
            }
        }

        public class IBaseResultProxy
        {
            [Fact]
            public void ReflectsDirectlyAddedErrors()
            {
                var target = new BasicResult();
                target.Errors.Add(new BasicResultError("ERR01", "msg", null));
                IBaseResult proxy = target;
                Assert.Equal(1, proxy.Errors.Count);
            }

            [Fact]
            public void ReflectsAddErrorMethod()
            {
                var target = new BasicResult();
                target.AddError(new BasicResultError("ERR01", "msg", "detail"));
                IBaseResult proxy = target;
                Assert.Equal(1, proxy.Errors.Count);
                Assert.Equal("ERR01", proxy.Errors[0].Code);
            }

            [Fact]
            public void ReflectsDirectlyAddedErrors_Generic()
            {
                var target = new BasicResult<Lalala>();
                target.Errors.Add(new ResultError<Lalala>());
                IBaseResult proxy = target;
                Assert.Equal(1, proxy.Errors.Count);
            }

            [Fact]
            public void ProxyResetAfterSetErrors()
            {
                var target = new BasicResult();
                target.Errors.Add(new BasicResultError("OLD", "old", null));
                var _ = ((IBaseResult)target).Errors; // force proxy creation
                var newList = new List<BasicResultError> { new BasicResultError("NEW", "new", null) };
                target.Errors = newList;
                IBaseResult proxy = target;
                Assert.Equal(1, proxy.Errors.Count);
                Assert.Equal("NEW", proxy.Errors[0].Code);
            }

            [Fact]
            public void ReplaceByIndex_Generic_WithBasicResultError_ConvertsViaCode()
            {
                // Consumer pattern: result.Errors[i] = new BasicResultError(code, localizedMessage, detail)
                // where result is IBaseResult backed by BasicResult<TResultCode>.
                var target = new BasicResult<Lalala>();
                target.Errors.Add(new ResultError<Lalala>(Lalala.One, "original"));
                IBaseResult proxy = target;
                proxy.Errors[0] = new BasicResultError("One", "localized", "detail");
                Assert.Equal("One", proxy.Errors[0].Code);
                Assert.Equal("localized", proxy.Errors[0].DisplayMessage);
                Assert.Equal("detail", proxy.Errors[0].Detail);
            }

            [Fact]
            public void AddViaProxy_Generic_WithBasicResultError_ConvertsViaCode()
            {
                var target = new BasicResult<Lalala>();
                IBaseResult proxy = target;
                proxy.Errors.Add(new BasicResultError("Many", "msg", null));
                Assert.Equal(1, proxy.Errors.Count);
                Assert.Equal("Many", proxy.Errors[0].Code);
            }
        }

        // Legacy flat tests kept for regression coverage
        [Fact]
        public void AddError_NoErrorCode_Direct()
        {
            var target = new BasicResult();
            var error = new BasicResultError();
            target.Errors.Add(error);
            Assert.Equal(1, target.Errors.Count);
        }

        [Fact]
        public void AddError_ErrorCode_Direct()
        {
            var target = new BasicResult<Lalala>();
            var error = new ResultError<Lalala>();
            target.Errors.Add(error);
            Assert.Equal(1, target.Errors.Count);
        }

        [Fact]
        public void AddError_NoErrorCode_Indirect()
        {
            IBaseResult target = new BasicResult();
            var error = new BasicResultError();
            target.Errors.Add(error);
            Assert.Equal(1, target.Errors.Count);
        }

        [Fact]
        public void AddError_ErrorCode_Indirect()
        {
            IBaseResult target = new BasicResult<Lalala>();
            var error = new ResultError<Lalala>();
            target.Errors.Add(error);
            Assert.Equal(1, target.Errors.Count);
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
