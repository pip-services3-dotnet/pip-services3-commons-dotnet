using PipServices3.Commons.Reflect;
using System.Collections.Generic;
using Xunit;

namespace PipServices3.Commons.Test.Reflect
{
    //[TestClass]
    public class PropertyReflectorTest
    {
        [Fact]
        public void TestGetProperty()
        {
            TestClass obj = new TestClass();

            object value = PropertyReflector.GetProperty(obj, "privateField");
            Assert.Null(value);

            value = PropertyReflector.GetProperty(obj, "publicField");
            Assert.Equal("ABC", value);

            value = PropertyReflector.GetProperty(obj, "PublicProp");
            Assert.NotNull(value);
        }

        [Fact]
        public void TestGetProperties()
        {
            TestClass obj = new TestClass();
            List<string> names = PropertyReflector.GetPropertyNames(obj);
            Assert.Equal(3, names.Count);
            Assert.True(names.Contains("PublicField"));
            Assert.True(names.Contains("PublicProp"));

            Dictionary<string, object> map = PropertyReflector.GetProperties(obj);
            Assert.Equal(3, map.Count);
            Assert.Equal("ABC", map["PublicField"]);
            Assert.NotNull(map["PublicProp"]);
        }

        [Fact]
        public void TestGetNestedProperties()
        {
            TestClass obj = new TestClass();
            obj.NestedProperty = new TestNestedClass() { IntProperty = 10 };

            Dictionary<string, object> map = PropertyReflector.GetProperties(obj);
            Assert.Equal(3, map.Count);
            Assert.True(map["NestedProperty"] is TestNestedClass);
            Assert.Equal(10, (map["NestedProperty"] as TestNestedClass).IntProperty);
        }

        [Fact]
        public void TestHasProperties()
        {
            TestClass obj = new TestClass();

            Assert.False(PropertyReflector.HasProperty(obj, "123"));
            Assert.True(PropertyReflector.HasProperty(obj, "PublicProp"));
            Assert.True(PropertyReflector.HasProperty(obj, "NestedProperty"));
        }
    }
}
