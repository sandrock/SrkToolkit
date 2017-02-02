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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Domain;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;

    [TestClass]
    public class BaseRequestTests
    {
        [TestClass]
        public class IsValidProperty
        {
            [TestMethod]
            public void ReturnsTrueForEmptyModel()
            {
                // prepare
                var request = new Request1();

                // execute
                var result = request.IsValid;

                // verify
                Assert.IsTrue(result);
            }
        
        }

        [TestClass]
        public class AddValidationErrorMethod
        {
            [TestMethod]
            public void AddingErrorWorks()
            {
                // prepare
                var request = new Request1();
                
                // execute
                request.AddValidationError("test");

                // verify
                Assert.AreEqual(1, request.AllValidationErrors.Count());
            }

            [TestMethod]
            public void AddingErrorBeforeValidationKeepsTheErrorAfterValidation()
            {
                // prepare
                var request = new Request1();
                
                // execute
                request.AddValidationError("test");

                // verify
                Assert.IsFalse(request.IsValid);
                Assert.AreEqual(1, request.AllValidationErrors.Count());
            }
        }

        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void DataContractJsonSerializerSerializesAllProperties()
            {
                var target = new Request1();
                target.Id = "42";
                var serializer = new DataContractJsonSerializer(typeof(Request1));
                var stream = new MemoryStream();
                serializer.WriteObject(stream, target);
                stream.Seek(0L, SeekOrigin.Begin);
                var unserialized = (Request1)serializer.ReadObject(stream);
                Assert.AreEqual(target.Id, unserialized.Id);
            }
        }

        public class Request1 : BaseRequest
        {
            public string Id { get; set; }
        }
    }
}
