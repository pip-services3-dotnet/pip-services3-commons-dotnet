using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class EnumConverterTest
    {
        [Fact]
        public void TestToEnum()
        {
            Assert.Equal(LogLevel.None, EnumConverter.ToEnum<LogLevel>("ABC"));
            Assert.Equal(LogLevel.Fatal, EnumConverter.ToEnum<LogLevel>(1));
            Assert.Equal(LogLevel.Fatal, EnumConverter.ToEnum<LogLevel>(LogLevel.Fatal));
            Assert.Equal(LogLevel.Fatal, EnumConverter.ToEnum<LogLevel>("Fatal"));
        }
    }
}
