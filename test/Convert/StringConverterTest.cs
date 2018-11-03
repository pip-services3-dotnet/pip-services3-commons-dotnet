using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class StringConverterTest
    {
        [Fact]
        public void TestToString()
        {
            Assert.Null(StringConverter.ToNullableString(null));
            Assert.Equal("xyz", StringConverter.ToString("xyz"));
            Assert.Equal("123", StringConverter.ToString(123));
            Assert.Equal("True", StringConverter.ToString(true));
            //Assert.Equal("{ prop = xyz }", StringConverter.ToString(new { prop = "xyz" }, "xyz"));

            Assert.Equal("xyz", StringConverter.ToStringWithDefault(null, "xyz"));
        }
    }
}
