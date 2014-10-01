
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SrkToolkit.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DictionaryWrapperTests
    {
        [TestClass]
        public class WrappedHashtableTests
        {
            [TestMethod]
            public void AddOnce_StringInt()
            {
                string key = "kk";
                int value = 42;
                var target = new DictionaryWrapper<string, int>(new System.Collections.Hashtable());
                target.Add(key, value);
                Assert.IsTrue(target.Count == 1);
            }

            [TestMethod]
            public void AddOnce_StringObject()
            {
                string key = "kk";
                object value = new object();
                var target = new DictionaryWrapper<string, object>(new System.Collections.Hashtable());
                target.Add(key, value);
                Assert.IsTrue(target.Count == 1);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void AddTwice_StringInt()
            {
                string key = "kk";
                int value = 42;
                var target = new DictionaryWrapper<string, int>(new System.Collections.Hashtable());
                target.Add(key, value);
                target.Add(key, value);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void AddTwice_StringObject()
            {
                string key = "kk";
                object value = new object();
                var target = new DictionaryWrapper<string, object>(new System.Collections.Hashtable());
                target.Add(key, value);
                target.Add(key, value);
            }

            [TestMethod]
            public void GetValid_StringInt()
            {
                string key = "kk";
                int value = 42;
                var target = new DictionaryWrapper<string, int>(new System.Collections.Hashtable());
                target.Add(key, value);
                var result = target[key];
                Assert.AreEqual(result, value);
            }

            [TestMethod]
            public void GetValid_StringObject()
            {
                string key = "kk";
                object value = new object();
                var target = new DictionaryWrapper<string, object>(new System.Collections.Hashtable());
                target.Add(key, value);
                var result = target[key];
                Assert.AreSame(result, value);
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void GetNoKey_StringInt()
            {
                string key = "kk";
                int value = 42;
                var target = new DictionaryWrapper<string, int>(new System.Collections.Hashtable());
                target.Add(key, value);
                var result = target["aa"];
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void GetNoKey_StringObject()
            {
                string key = "kk";
                object value = new object();
                var target = new DictionaryWrapper<string, object>(new System.Collections.Hashtable());
                target.Add(key, value);
                var result = target["aa"];
            }
        }
    }
}
