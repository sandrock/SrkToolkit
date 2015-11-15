
namespace SrkToolkit.Domain.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class ResultErrorExtensionsTests
    {
        [TestClass]
        public class AddMethod
        {
            [TestMethod]
            public void Overload1()
            {
                var result = new Result1(new Request1());
                var code = Error1.Hello;
                var message = "bad error";
                result.Errors.Add(code, message);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(code, result.Errors[0].Code);
                Assert.AreEqual(message, result.Errors[0].DisplayMessage);
                Assert.IsNull(result.Errors[0].Detail);
            }

            [TestMethod]
            public void Overload2()
            {
                var result = new Result1(new Request1());
                var code = Error1.Hello;
                var message = "Error1 Hello";
                result.Errors.Add(code, Strings.ResourceManager);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(code, result.Errors[0].Code);
                Assert.AreEqual(message, result.Errors[0].DisplayMessage);
                Assert.IsNull(result.Errors[0].Detail);
            }

            [TestMethod]
            public void Overload1_Format()
            {
                var result = new Result1(new Request1());
                var code = Error1.World;
                var format = "Mlivej";
                var message = @"WorldCode """ + format + @"""";
                result.Errors.Add(code, @"WorldCode ""{0}""", format);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(code, result.Errors[0].Code);
                Assert.AreEqual(message, result.Errors[0].DisplayMessage);
                Assert.IsNull(result.Errors[0].Detail);
            }

            [TestMethod]
            public void Overload2_Format()
            {
                var result = new Result1(new Request1());
                var code = Error1.World;
                var format = "Mlivej";
                var message = @"WorldCode """ + format + @"""";
                result.Errors.Add(code, Strings.ResourceManager, format);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(code, result.Errors[0].Code);
                Assert.AreEqual(message, result.Errors[0].DisplayMessage);
                Assert.IsNull(result.Errors[0].Detail);
            }
        }
            
        [TestClass]
        public class AddDetailMethod
        {
            [TestMethod]
            public void Overload1()
            {
                var result = new Result1(new Request1());
                var code = Error1.Hello;
                var message = "bad error";
                var detail = "Glarg";
                result.Errors.AddDetail(code, detail, message);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(code, result.Errors[0].Code);
                Assert.AreEqual(message, result.Errors[0].DisplayMessage);
                Assert.AreEqual(detail, result.Errors[0].Detail);
            }

            [TestMethod]
            public void Overload2()
            {
                var result = new Result1(new Request1());
                var code = Error1.Hello;
                var message = "Error1 Hello";
                var detail = "Glarg";
                result.Errors.AddDetail(code, detail, Strings.ResourceManager);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(code, result.Errors[0].Code);
                Assert.AreEqual(message, result.Errors[0].DisplayMessage);
                Assert.AreEqual(detail, result.Errors[0].Detail);
            }

            [TestMethod]
            public void Overload1_Format()
            {
                var result = new Result1(new Request1());
                var code = Error1.World;
                var format = "Mlivej";
                var message = @"WorldCode """ + format + @"""";
                var detail = "Glarg";
                result.Errors.AddDetail(code, detail, @"WorldCode ""{0}""", format);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(code, result.Errors[0].Code);
                Assert.AreEqual(message, result.Errors[0].DisplayMessage);
                Assert.AreEqual(detail, result.Errors[0].Detail);
            }

            [TestMethod]
            public void Overload2_Format()
            {
                var result = new Result1(new Request1());
                var code = Error1.World;
                var format = "Mlivej";
                var message = @"WorldCode """ + format + @"""";
                var detail = "Glarg";
                result.Errors.AddDetail(code, detail, Strings.ResourceManager, format);
                Assert.AreEqual(1, result.Errors.Count);
                Assert.AreEqual(code, result.Errors[0].Code);
                Assert.AreEqual(message, result.Errors[0].DisplayMessage);
                Assert.AreEqual(detail, result.Errors[0].Detail);
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
