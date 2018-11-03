using System;
using Xunit;
using PipServices3.Commons.Reflect;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;

namespace PipServices3.Commons.Test.Reflect
{
    //[TestClass]
    public sealed class ObjectWriterTest
    {
        [Fact]
        public void TestSetObjectProperty()
        {
            var obj = new TestClass();

            ObjectWriter.SetProperty(obj, "_privateField", "XYZ");

            ObjectWriter.SetProperty(obj, "PublicField", "AAAA");
            Assert.Equal("AAAA", obj.PublicField);

            var now = DateTime.UtcNow;
            ObjectWriter.SetProperty(obj, "PublicProp", now);
            Assert.Equal(now, obj.PublicProp);

            ObjectWriter.SetProperty(obj, "PublicProp", "BBBB");
            Assert.Equal(now, obj.PublicProp);
        }

        [Fact]
        public void TestSetMapProperty()
        {
            AnyValueMap map = AnyValueMap.FromTuples(
                "key1", 123,
                "key2", "ABC"
            );

            ObjectWriter.SetProperty(map, "key3", "AAAA");
            Assert.Equal("AAAA", map.Get("key3"));

            ObjectWriter.SetProperty(map, "Key1", 5555);
            Assert.Equal(5555, map.Get("Key1"));

            ObjectWriter.SetProperty(map, "Key2", "BBBB");
            Assert.Equal("BBBB", map.Get("Key2"));
        }

        [Fact]
        public void TestSetArrayProperty()
        {
            var list = AnyValueArray.FromValues(123, "ABC");

            ObjectWriter.SetProperty(list, "3", "AAAA");
            Assert.Equal(4, list.Count);
            Assert.Equal("AAAA", list[3]);

            ObjectWriter.SetProperty(list, "0", 1111);
            Assert.Equal(1111, list[0]);

            ObjectWriter.SetProperty(list, "1", "BBBB");
            Assert.Equal("BBBB", list[1]);

            var array = new object[] { 123, "ABC" };

            ObjectWriter.SetProperty(array, "3", "AAAA");
            Assert.Equal(2, array.Length);

            ObjectWriter.SetProperty(array, "0", 1111);
            Assert.Equal(1111, array[0]);

            ObjectWriter.SetProperty(array, "1", "BBBB");
            Assert.Equal("BBBB", array[1]);
        }
    }
}
