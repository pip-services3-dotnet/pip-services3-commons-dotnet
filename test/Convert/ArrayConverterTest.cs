using System.Collections.Generic;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class ArrayConverterTest
    {
        [Fact]
        public void TestToNullableArray()
        {
            Assert.Null(ArrayConverter.ToNullableArray(null));

            Assert.True(ArrayConverter.ToNullableArray(2).Count == 1);

            List<int> array = new List<int>();
            array.Add(1);
            array.Add(2);
            Assert.True(ArrayConverter.ToNullableArray(array).Count == 2);

            string[] stringArray = { "ab", "cd" };
            Assert.True(ArrayConverter.ToNullableArray(stringArray).Count == 2);
        }

        [Fact]
        public void ToArray()
        {
            var value = ArrayConverter.ToArray(null);
            Assert.IsType<List<object>>(value);
            Assert.True(value.Count == 0);

            value = ArrayConverter.ToArray(123);
            Assert.IsType<List<object>>(value);
            Assert.True(value.Count == 1);
            Assert.Equal(123, value[0]);

            value = ArrayConverter.ToArray(new List<int> { 123 });
            Assert.IsType<List<object>>(value);
            Assert.True(value.Count == 1);
            Assert.Equal(123, value[0]);

            value = ArrayConverter.ToArray("123");
            Assert.IsType<List<object>>(value);
            Assert.True(value.Count == 1);
            Assert.Equal("123", value[0]);

            value = ArrayConverter.ListToArray("123,456");
            Assert.IsType<List<object>>(value);
            Assert.True(value.Count == 2);
            Assert.Equal("123", value[0]);
            Assert.Equal("456", value[1]);
        }
    }
}
