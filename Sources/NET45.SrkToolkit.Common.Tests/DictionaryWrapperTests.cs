
namespace SrkToolkit.Common.Tests
{
    using SrkToolkit.Collections;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    public class DictionaryWrapperTests
    {
        [Fact]
        public void Works()
        {
            var source = new Hashtable();
            source.Add("a", "b");
            source.Add("b", "c");
            var target = new DictionaryWrapper<string, string>(source);
            Assert.Equal(source.Count, target.Count);
            Assert.Equal("b", target["a"]);
            Assert.Equal("c", target["b"]);

            target.Add("x", "y");
            Assert.Equal(3, source.Count);
        }
    }
}
