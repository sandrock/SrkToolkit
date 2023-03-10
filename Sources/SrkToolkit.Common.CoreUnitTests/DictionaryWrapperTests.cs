
namespace SrkToolkit.Common.Tests
{
    using SrkToolkit.Collections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class DictionaryWrapperTests
    {
        public class WrappedHashtableTests
        {
            [Fact]
            public void AddOnce_StringInt()
            {
                string key = "kk";
                int value = 42;
                var target = new DictionaryWrapper<string, int>(new System.Collections.Hashtable());
                target.Add(key, value);
                Assert.True(target.Count == 1);
            }

            [Fact]
            public void AddOnce_StringObject()
            {
                string key = "kk";
                object value = new object();
                var target = new DictionaryWrapper<string, object>(new System.Collections.Hashtable());
                target.Add(key, value);
                Assert.True(target.Count == 1);
            }

            [Fact]
            public void AddTwice_StringInt()
            {
                string key = "kk";
                int value = 42;
                Assert.Throws<ArgumentException>(() =>
                {
                    var target = new DictionaryWrapper<string, int>(new System.Collections.Hashtable());
                    target.Add(key, value);
                    target.Add(key, value);
                });
            }

            [Fact]
            public void AddTwice_StringObject()
            {
                string key = "kk";
                object value = new object();
                Assert.Throws<ArgumentException>(() =>
                {
                    var target = new DictionaryWrapper<string, object>(new System.Collections.Hashtable());
                    target.Add(key, value);
                    target.Add(key, value);
                });
            }

            [Fact]
            public void GetValid_StringInt()
            {
                string key = "kk";
                int value = 42;
                var target = new DictionaryWrapper<string, int>(new System.Collections.Hashtable());
                target.Add(key, value);
                var result = target[key];
                Assert.Equal(result, value);
            }

            [Fact]
            public void GetValid_StringObject()
            {
                string key = "kk";
                object value = new object();
                var target = new DictionaryWrapper<string, object>(new System.Collections.Hashtable());
                target.Add(key, value);
                var result = target[key];
                Assert.Same(result, value);
            }

            [Fact]
            public void GetNoKey_StringInt()
            {
                string key = "kk";
                int value = 42;
                var target = new DictionaryWrapper<string, int>(new System.Collections.Hashtable());
                target.Add(key, value);
                Assert.Throws<ArgumentException>(() =>
                {
                    var result = target["aa"];
                });
            }

            [Fact]
            public void GetNoKey_StringObject()
            {
                string key = "kk";
                object value = new object();
                var target = new DictionaryWrapper<string, object>(new System.Collections.Hashtable());
                target.Add(key, value);
                Assert.Throws<ArgumentException>(() =>
                {
                    var result = target["aa"];
                });
            }
        }
    }
}
