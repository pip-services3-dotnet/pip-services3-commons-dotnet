using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class BooleanConverterTest
    {
        [Fact]
        public void TestToBoolean()
        {
            Assert.True(BooleanConverter.ToBoolean(true));
            Assert.True(BooleanConverter.ToBoolean(1));
            Assert.True(BooleanConverter.ToBoolean("True"));
            Assert.True(BooleanConverter.ToBoolean("yes"));
            Assert.True(BooleanConverter.ToBoolean("1"));
            Assert.True(BooleanConverter.ToBoolean("Y"));

            Assert.False(BooleanConverter.ToBoolean(false));
            Assert.False(BooleanConverter.ToBoolean(0));
            Assert.False(BooleanConverter.ToBoolean("False"));
            Assert.False(BooleanConverter.ToBoolean("no"));
            Assert.False(BooleanConverter.ToBoolean("0"));
            Assert.False(BooleanConverter.ToBoolean("N"));

            Assert.False(BooleanConverter.ToBoolean(123));
            Assert.False(BooleanConverter.ToBoolean(null));
            Assert.True(BooleanConverter.ToBooleanWithDefault("XYZ", true));
        }
    }
}
