// -----------------------------------------------------------------------
// <copyright file="OpenGraphNameTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SrkToolkit.Web.Open;

    [TestClass]
    public class OpenGraphNameTests
    {
        [TestMethod]
        public void HasDefinedNameAndNamespace()
        {
            var name = "toto";
            var nsName = "super";
            var ns = new OpenGraphNamespace(nsName, "a");
            var obj = new OpenGraphName(ns, name);
            Assert.AreEqual(nsName, obj.NamespaceName);
            Assert.AreEqual(name, obj.Name);
        }

        ////[TestMethod]
        ////public void StringImplicitOperatorGivesOgNamespaceName()
        ////{
        ////    var name = "toto";
        ////    OpenGraphName obj = name;
        ////    Assert.AreEqual("og", obj.NamespaceName);
        ////    Assert.AreEqual(name, obj.Name);
        ////}

        [TestMethod]
        public void NameCtorGivesOgNamespaceName()
        {
            var name = "toto";
            OpenGraphName obj = new OpenGraphName(name);
            Assert.AreEqual("og", obj.NamespaceName);
            Assert.AreEqual(name, obj.Name);
        }
    }
}
