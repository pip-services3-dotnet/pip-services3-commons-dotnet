using System;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class FloatConverterTest
    {
        [Fact]
        public void TestToFloat()
        {
            Assert.True(Math.Abs(123 - FloatConverter.ToFloat(123)) < 0.001);
            Assert.True(Math.Abs(123.456 - FloatConverter.ToFloat(123.456)) < 0.001);
            Assert.True(Math.Abs(123.456 - FloatConverter.ToFloat("123.456")) < 0.001);

            Assert.True(Math.Abs(123 - FloatConverter.ToFloatWithDefault(null, 123)) < 0.001);
            Assert.True(Math.Abs(0 - FloatConverter.ToFloatWithDefault(false, 123)) < 0.001);
            Assert.True(Math.Abs(123 - FloatConverter.ToFloatWithDefault("ABC", 123)) < 0.001);

            double doubleValue = float.MaxValue;
            Assert.True(Math.Abs(doubleValue - FloatConverter.ToFloat(doubleValue)) < 0.001);
            doubleValue = double.MaxValue;
            var test = System.Convert.ToSingle(doubleValue, System.Globalization.CultureInfo.InvariantCulture);
            Assert.Equal(0, FloatConverter.ToFloat(doubleValue));
        }
    }
}
