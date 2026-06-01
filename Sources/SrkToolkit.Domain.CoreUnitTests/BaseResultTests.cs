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
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using Xunit;

    public class BaseResultTests
    {
        public class Serialization
        {
            [Fact]
            public void DataContractJsonSerializerSerializesAllProperties()
            {
                var targetRequest = new Request1();
                targetRequest.Id = "42";
                var targetResult = new Result1(targetRequest);
                targetResult.Id = "42";
                targetResult.Succeed = true;
                targetResult.Errors.AddDetail(Error1.Error42, "The detail.", "The error to all failed algorithms.");
                var serializer = new DataContractJsonSerializer(typeof(Result1));
                var stream = new MemoryStream();
                serializer.WriteObject(stream, targetResult);
                stream.Seek(0L, SeekOrigin.Begin);
                var unserialized = (Result1)serializer.ReadObject(stream);
                Assert.Equal(targetResult.Succeed, unserialized.Succeed);
                Assert.Equal(targetResult.Id, unserialized.Id);
                Assert.Equal(targetResult.Request.Id, unserialized.Request.Id);
                Assert.Equal(targetResult.Errors.Count, unserialized.Errors.Count);
                Assert.Equal(targetResult.Errors[0].Code, unserialized.Errors[0].Code);
                Assert.Equal(targetResult.Errors[0].DisplayMessage, unserialized.Errors[0].DisplayMessage);
                Assert.Equal(targetResult.Errors[0].Detail, unserialized.Errors[0].Detail);
            }
        }

        public class DefaultConstructor
        {
            [Fact]
            public void SucceedDefaultsFalse()
            {
                var target = new BaseResult<Request1, Error1>();
                Assert.False(target.Succeed);
            }

            [Fact]
            public void RequestIsNull()
            {
                var target = new BaseResult<Request1, Error1>();
                Assert.Null(target.Request);
            }

            [Fact]
            public void ErrorsIsEmpty()
            {
                var target = new BaseResult<Request1, Error1>();
                Assert.Empty(target.Errors);
            }
        }

        public class RequestConstructor
        {
            [Fact]
            public void StoresRequest()
            {
                var request = new Request1 { Id = "42" };
                var target = new BaseResult<Request1, Error1>(request);
                Assert.Same(request, target.Request);
            }

            [Fact]
            public void SucceedDefaultsFalse()
            {
                var request = new Request1();
                var target = new BaseResult<Request1, Error1>(request);
                Assert.False(target.Succeed);
            }
        }

        public class SucceedProperty
        {
            [Fact]
            public void CanBeSetToTrue()
            {
                var target = new BaseResult<Request1, Error1>();
                target.Succeed = true;
                Assert.True(target.Succeed);
            }

            [Fact]
            public void CanBeSetBackToFalse()
            {
                var target = new BaseResult<Request1, Error1>();
                target.Succeed = true;
                target.Succeed = false;
                Assert.False(target.Succeed);
            }
        }

        public class RequestProperty
        {
            [Fact]
            public void CanBeSetViaSetter()
            {
                var target = new BaseResult<Request1, Error1>();
                var request = new Request1 { Id = "99" };
                target.Request = request;
                Assert.Same(request, target.Request);
            }

            [Fact]
            public void CanBeOverwritten()
            {
                var request1 = new Request1 { Id = "1" };
                var request2 = new Request1 { Id = "2" };
                var target = new BaseResult<Request1, Error1>(request1);
                target.Request = request2;
                Assert.Same(request2, target.Request);
            }
        }

        public class ErrorsProperty
        {
            [Fact]
            public void LazilyInitialized()
            {
                var target = new BaseResult<Request1, Error1>();
                var first = target.Errors;
                var second = target.Errors;
                Assert.Same(first, second);
            }

            [Fact]
            public void CanAddDirectly()
            {
                var target = new BaseResult<Request1, Error1>();
                target.Errors.Add(new ResultError<Error1>(Error1.Error42, "msg"));
                Assert.Equal(1, target.Errors.Count);
            }

            [Fact]
            public void CanAddMultiple()
            {
                var target = new BaseResult<Request1, Error1>();
                target.Errors.Add(new ResultError<Error1>(Error1.Error42, "first"));
                target.Errors.Add(new ResultError<Error1>(Error1.IAmNotATeapot, "second"));
                Assert.Equal(2, target.Errors.Count);
            }

            [Fact]
            public void CanSetNewList()
            {
                var target = new BaseResult<Request1, Error1>();
                var list = new List<ResultError<Error1>> { new ResultError<Error1>(Error1.Error42, "msg") };
                target.Errors = list;
                Assert.Equal(1, target.Errors.Count);
                Assert.Equal(Error1.Error42, target.Errors[0].Code);
            }
        }

        public class IBaseResultProxy
        {
            [Fact]
            public void ReflectsDirectlyAddedErrors()
            {
                var target = new BaseResult<Request1, Error1>();
                target.Errors.Add(new ResultError<Error1>(Error1.Error42, "msg"));
                IBaseResult proxy = target;
                Assert.Equal(1, proxy.Errors.Count);
            }

            [Fact]
            public void ReflectsErrorCode()
            {
                var target = new BaseResult<Request1, Error1>();
                target.Errors.Add(new ResultError<Error1>(Error1.IAmNotATeapot, "msg"));
                IBaseResult proxy = target;
                Assert.Equal(Error1.IAmNotATeapot.ToString(), proxy.Errors[0].Code);
            }

            [Fact]
            public void ProxyResetAfterSetErrors()
            {
                var target = new BaseResult<Request1, Error1>();
                target.Errors.Add(new ResultError<Error1>(Error1.Unknown, "old"));
                var _ = ((IBaseResult)target).Errors; // force proxy creation
                var newList = new List<ResultError<Error1>> { new ResultError<Error1>(Error1.Error42, "new") };
                target.Errors = newList;
                IBaseResult proxy = target;
                Assert.Equal(1, proxy.Errors.Count);
                Assert.Equal(Error1.Error42.ToString(), proxy.Errors[0].Code);
            }
        }

        // Legacy flat tests kept for regression coverage
        [Fact]
        public void AddError_Direct()
        {
            var target = new BaseResult<Request1, Error1>();
            target.Errors.Add(new ResultError<Error1>());
            Assert.Equal(1, target.Errors.Count);
        }

        [Fact]
        public void AddError_Indirect()
        {
            IBaseResult target = new BaseResult<Request1, Error1>();
            target.Errors.Add(new ResultError<Error1>());
            Assert.Equal(1, target.Errors.Count);
        }

        public class Request1 : BaseRequest
        {
            public string Id { get; set; }
        }

        public class Result1 : BaseResult<Request1, Error1>
        {
            public Result1()
            {
            }

            public Result1(Request1 request)
                : base(request)
            {
            }

            public string Id { get; set; }
        }

        public enum Error1
        {
            Unknown,
            Error42,
            IAmNotATeapot,
        }
    }
}
