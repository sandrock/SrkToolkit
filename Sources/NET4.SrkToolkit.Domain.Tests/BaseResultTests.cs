﻿
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
    public class BaseResultTests
    {
        [TestClass]
        public class Serialization
        {
            [TestMethod]
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
                Assert.AreEqual(targetResult.Succeed, unserialized.Succeed);
                Assert.AreEqual(targetResult.Id, unserialized.Id);
                Assert.AreEqual(targetResult.Request.Id, unserialized.Request.Id);
                Assert.AreEqual(targetResult.Errors.Count, unserialized.Errors.Count);
                Assert.AreEqual(targetResult.Errors[0].Code, unserialized.Errors[0].Code);
                Assert.AreEqual(targetResult.Errors[0].DisplayMessage, unserialized.Errors[0].DisplayMessage);
                Assert.AreEqual(targetResult.Errors[0].Detail, unserialized.Errors[0].Detail);
            }
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
