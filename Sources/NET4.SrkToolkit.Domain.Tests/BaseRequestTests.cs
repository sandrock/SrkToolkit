
namespace SrkToolkit.Domain.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Domain;

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

        public class Request1 : BaseRequest
        {
        }
    }
}
