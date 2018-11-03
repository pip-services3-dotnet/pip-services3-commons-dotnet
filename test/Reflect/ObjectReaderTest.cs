using PipServices3.Commons.Data;
using PipServices3.Commons.Reflect;
using Xunit;

namespace PipServices3.Commons.Test.Reflect
{
    //[TestClass]
    public sealed class ObjectReaderTest
    {
        [Fact]
        public void TestGetObjectProperty()
        {
            var obj = new TestClass();

            var value = ObjectReader.GetProperty(obj, "_privateField");
            Assert.Null(value);

            value = ObjectReader.GetProperty(obj, "PublicField");
            Assert.Equal("ABC", value);

            value = ObjectReader.GetProperty(obj, "PublicProp");
            Assert.NotNull(value);
        }

        [Fact]
        public void TestGetMapProperty()
        {
            var map = AnyValueMap.FromTuples(
                "key1", 123,
                "key2", "ABC"
            );

            var key3Value = ObjectReader.GetProperty(map, "key3");
            Assert.Null(key3Value);

            var key1Value = ObjectReader.GetProperty(map, "Key1");
            Assert.Equal(123, key1Value);

            var key2Value = ObjectReader.GetProperty(map, "KEY2");
            Assert.Equal("ABC", key2Value);
        }

        [Fact]
        public void TestGetArrayProperty()
        {
            var list = AnyValueArray.FromValues(123, "ABC");

            var value = ObjectReader.GetProperty(list, "3");
            Assert.Null(value);

            value = ObjectReader.GetProperty(list, "0");
            Assert.Equal(123, value);

            value = ObjectReader.GetProperty(list, "1");
            Assert.Equal("ABC", value);

            var array = new object[] { 123, "ABC" };

            value = ObjectReader.GetProperty(array, "3");
            Assert.Null(value);

            value = ObjectReader.GetProperty(array, "0");
            Assert.Equal(123, value);

            value = ObjectReader.GetProperty(array, "1");
            Assert.Equal("ABC", value);
        }

        [Fact]
        public void TestGetObjectProperties()
        {
            var obj = new TestClass();
            var names = ObjectReader.GetPropertyNames(obj);
            Assert.Equal(3, names.Count);
            Assert.True(names.Contains("PublicField"));
            Assert.True(names.Contains("PublicProp"));

            var map = ObjectReader.GetProperties(obj);
            Assert.Equal(3, map.Count);
            Assert.Equal("ABC", map["PublicField"]);
            Assert.NotNull(map["PublicProp"]);
        }

        [Fact]
        public void TestGetMapProperties()
        {
            AnyValueMap map = AnyValueMap.FromTuples(
                "key1", 123,
                "key2", "ABC"
            );
            var names = ObjectReader.GetPropertyNames(map);
            Assert.Equal(2, names.Count);
            Assert.True(names.Contains("key1"));
            Assert.True(names.Contains("key2"));

            var values = ObjectReader.GetProperties(map);
            Assert.Equal(2, values.Count);
            Assert.Equal(123, values["key1"]);
            Assert.Equal("ABC", values["key2"]);
        }

        [Fact]
        public void TestGetArrayProperties()
        {
            AnyValueArray list = AnyValueArray.FromValues(123, "ABC");

            var names = ObjectReader.GetPropertyNames(list);
            Assert.Equal(2, names.Count);
            Assert.True(names.Contains("0"));
            Assert.True(names.Contains("1"));

            var values = ObjectReader.GetProperties(list);
            Assert.Equal(2, values.Count);
            Assert.Equal(123, values["0"]);
            Assert.Equal("ABC", values["1"]);

            var array = new object[] { 123, "ABC" };

            names = ObjectReader.GetPropertyNames(array);
            Assert.Equal(2, names.Count);
            Assert.True(names.Contains("0"));
            Assert.True(names.Contains("1"));

            values = ObjectReader.GetProperties(array);
            Assert.Equal(2, values.Count);
            Assert.Equal(123, values["0"]);
            Assert.Equal("ABC", values["1"]);
        }
    }
}
