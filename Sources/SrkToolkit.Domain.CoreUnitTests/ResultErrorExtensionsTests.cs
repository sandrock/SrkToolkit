﻿// 
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
    using Xunit;

    public class ResultErrorExtensionsTests
    {
        public class AddMethod
        {
            [Fact]
            public void Overload1()
            {
                var result = new Result1(new Request1());
                var code = Error1.Hello;
                var message = "bad error";
                result.Errors.Add(code, message);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(code, result.Errors[0].Code);
                Assert.Equal(message, result.Errors[0].DisplayMessage);
                Assert.Null(result.Errors[0].Detail);
            }

            [Fact]
            public void Overload2()
            {
                var result = new Result1(new Request1());
                var code = Error1.Hello;
                var message = "Error1 Hello";
                result.Errors.Add(code, Strings.ResourceManager);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(code, result.Errors[0].Code);
                Assert.Equal(message, result.Errors[0].DisplayMessage);
                Assert.Null(result.Errors[0].Detail);
            }

            [Fact]
            public void Overload1_Format()
            {
                var result = new Result1(new Request1());
                var code = Error1.World;
                var format = "Mlivej";
                var message = @"WorldCode """ + format + @"""";
                result.Errors.Add(code, @"WorldCode ""{0}""", format);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(code, result.Errors[0].Code);
                Assert.Equal(message, result.Errors[0].DisplayMessage);
                Assert.Null(result.Errors[0].Detail);
            }

            [Fact]
            public void Overload2_Format()
            {
                var result = new Result1(new Request1());
                var code = Error1.World;
                var format = "Mlivej";
                var message = @"WorldCode """ + format + @"""";
                result.Errors.Add(code, Strings.ResourceManager, format);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(code, result.Errors[0].Code);
                Assert.Equal(message, result.Errors[0].DisplayMessage);
                Assert.Null(result.Errors[0].Detail);
            }
        }
            
        public class AddDetailMethod
        {
            [Fact]
            public void Overload1()
            {
                var result = new Result1(new Request1());
                var code = Error1.Hello;
                var message = "bad error";
                var detail = "Glarg";
                result.Errors.AddDetail(code, detail, message);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(code, result.Errors[0].Code);
                Assert.Equal(message, result.Errors[0].DisplayMessage);
                Assert.Equal(detail, result.Errors[0].Detail);
            }

            [Fact]
            public void Overload2()
            {
                var result = new Result1(new Request1());
                var code = Error1.Hello;
                var message = "Error1 Hello";
                var detail = "Glarg";
                result.Errors.AddDetail(code, detail, Strings.ResourceManager);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(code, result.Errors[0].Code);
                Assert.Equal(message, result.Errors[0].DisplayMessage);
                Assert.Equal(detail, result.Errors[0].Detail);
            }

            [Fact]
            public void Overload1_Format()
            {
                var result = new Result1(new Request1());
                var code = Error1.World;
                var format = "Mlivej";
                var message = @"WorldCode """ + format + @"""";
                var detail = "Glarg";
                result.Errors.AddDetail(code, detail, @"WorldCode ""{0}""", format);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(code, result.Errors[0].Code);
                Assert.Equal(message, result.Errors[0].DisplayMessage);
                Assert.Equal(detail, result.Errors[0].Detail);
            }

            [Fact]
            public void Overload2_Format()
            {
                var result = new Result1(new Request1());
                var code = Error1.World;
                var format = "Mlivej";
                var message = @"WorldCode """ + format + @"""";
                var detail = "Glarg";
                result.Errors.AddDetail(code, detail, Strings.ResourceManager, format);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(code, result.Errors[0].Code);
                Assert.Equal(message, result.Errors[0].DisplayMessage);
                Assert.Equal(detail, result.Errors[0].Detail);
            }
        }

        public class Request1 : BaseRequest
        {
        }

        public class Result1 : BaseResult<Request1, Error1>
        {
            public Result1(Request1 request)
                : base(request)
            {
            }
        }

        public enum Error1
        {
            Hello,
            World,
        }
    }
}
