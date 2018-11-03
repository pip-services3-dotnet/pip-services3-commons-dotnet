using System;
using System.Collections.Generic;
using Xunit;

namespace PipServices3.Commons.Reflect
{
    //[TestClass]
    public class TypeMatcherTest
    {
        [Fact]
        public void MatchInteger()
        {
            Assert.True(TypeMatcher.MatchValueByName("int", 123));
            Assert.True(TypeMatcher.MatchValueByName("Integer", 123));
            Assert.True(TypeMatcher.MatchValue(typeof(int), 123));
	    }

        [Fact]
        public void MatchLong()
        {
            Assert.True(TypeMatcher.MatchValueByName("long", 123L));
            Assert.True(TypeMatcher.MatchValue(typeof(long), 123L));
	    }

        [Fact]
        public void MatchBoolean()
        {
            Assert.True(TypeMatcher.MatchValueByName("bool", true));
            Assert.True(TypeMatcher.MatchValueByName("Boolean", true));
            Assert.True(TypeMatcher.MatchValue(typeof(bool), true));
	    }

        [Fact]
        public void MatchFloat()
        {
            Assert.True(TypeMatcher.MatchValueByName("float", 123.456f));
            Assert.True(TypeMatcher.MatchValueByName("Float", 123.456f));
            Assert.True(TypeMatcher.MatchValue(typeof(float), 123.456f));
	    }

        [Fact]
        public void MatchDouble()
        {
            Assert.True(TypeMatcher.MatchValueByName("double", 123.456));
            Assert.True(TypeMatcher.MatchValueByName("Double", 123.456));
            Assert.True(TypeMatcher.MatchValue(typeof(double), 123.456));
	    }

        [Fact]
        public void MatchString()
        {
            Assert.True(TypeMatcher.MatchValueByName("string", "ABC"));
            Assert.True(TypeMatcher.MatchValue(typeof(string), "ABC"));
	    }

        [Fact]
        public void MatchDateTime()
        {
            Assert.True(TypeMatcher.MatchValueByName("date", new DateTime()));
            Assert.True(TypeMatcher.MatchValueByName("DateTime", DateTime.Now));
            Assert.True(TypeMatcher.MatchValue(typeof(DateTime), new DateTime()));
	    }

        [Fact]
        public void MatchDuration()
        {
            Assert.True(TypeMatcher.MatchValueByName("duration", 123));
            Assert.True(TypeMatcher.MatchValueByName("TimeSpan", 123));
        }

        [Fact]
        public void MatchMap()
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            Assert.True(TypeMatcher.MatchValueByName("map", map));
            Assert.True(TypeMatcher.MatchValueByName("dict", map));
            Assert.True(TypeMatcher.MatchValueByName("Dictionary", map));
            Assert.True(TypeMatcher.MatchValue(typeof(Dictionary<string, object>), map));
	    }

        [Fact]
        public void MatchArray()
        {
            List<object> list = new List<object>();
            Assert.True(TypeMatcher.MatchValueByName("list", list));
            Assert.True(TypeMatcher.MatchValueByName("array", list));
            Assert.True(TypeMatcher.MatchValueByName("object[]", list));
            Assert.True(TypeMatcher.MatchValue(typeof(List<object>), list));

		    int[] array = new int[0];
            Assert.True(TypeMatcher.MatchValueByName("list", array));
            Assert.True(TypeMatcher.MatchValueByName("array", array));
            Assert.True(TypeMatcher.MatchValueByName("object[]", array));
	    }
    }
}
