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

    public class BaseRequestTests
    {
        public class IsValidProperty
        {
            [Fact]
            public void ReturnsTrueForEmptyModel()
            {
                // prepare
                var request = new Request1();

                // execute
                var result = request.IsValid;

                // verify
                Assert.True(result);
            }
        
        }

        public class AddValidationErrorMethod
        {
            [Fact]
            public void AddingErrorWorks()
            {
                // prepare
                var request = new Request1();
                
                // execute
                request.AddValidationError("test");

                // verify
                Assert.Equal(1, request.AllValidationErrors.Count());
            }

            [Fact]
            public void AddingErrorBeforeValidationKeepsTheErrorAfterValidation()
            {
                // prepare
                var request = new Request1();
                
                // execute
                request.AddValidationError("test");

                // verify
                Assert.False(request.IsValid);
                Assert.Equal(1, request.AllValidationErrors.Count());
            }
        }

        public class Serialization
        {
            [Fact]
            public void DataContractJsonSerializerSerializesAllProperties()
            {
                var target = new Request1();
                target.Id = "42";
                var serializer = new DataContractJsonSerializer(typeof(Request1));
                var stream = new MemoryStream();
                serializer.WriteObject(stream, target);
                stream.Seek(0L, SeekOrigin.Begin);
                var unserialized = (Request1)serializer.ReadObject(stream);
                Assert.Equal(target.Id, unserialized.Id);
            }
        }

        public class Request1 : BaseRequest
        {
            public string Id { get; set; }
        }
    }
}
