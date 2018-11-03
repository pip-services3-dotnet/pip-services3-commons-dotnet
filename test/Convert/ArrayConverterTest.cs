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
    }
}
