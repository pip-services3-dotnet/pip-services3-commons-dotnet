using System;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class DoubleConverterTest
    {
        [Fact]
        public void TestToDouble()
        {
            Assert.Null(DoubleConverter.ToNullableDouble(null));
            Assert.True(Math.Abs(123 - DoubleConverter.ToDouble(123)) < 0.001);
            Assert.True(Math.Abs(123.456 - DoubleConverter.ToDouble(123.456)) < 0.001);
            Assert.True(Math.Abs(123.456 - DoubleConverter.ToDouble("123.456")) < 0.001);

            Assert.True(Math.Abs(123 - DoubleConverter.ToDoubleWithDefault(null, 123)) < 0.001);
            Assert.True(Math.Abs(0 - DoubleConverter.ToDoubleWithDefault(false, 123)) < 0.001);
            Assert.True(Math.Abs(123 - DoubleConverter.ToDoubleWithDefault("ABC", 123)) < 0.001);
        }
    }
}
