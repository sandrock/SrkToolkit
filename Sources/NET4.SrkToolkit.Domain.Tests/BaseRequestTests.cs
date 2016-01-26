
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
