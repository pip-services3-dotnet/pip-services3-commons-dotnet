using System;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class LongConverterTest
    {
        [Fact]
        public void TestToLong()
        {
            DateTime date = new DateTime();
            Assert.Equal(date.Ticks, LongConverter.ToLong(date));
            Assert.Equal(100, LongConverter.ToLong(new TimeSpan(100)));
            Assert.Equal(123, LongConverter.ToLong(123));
            Assert.Equal(123, LongConverter.ToLong((short)123));
            Assert.Equal(123, LongConverter.ToLong(123.456));
            Assert.Equal(124, LongConverter.ToLong(123.999));
            Assert.Equal(123, LongConverter.ToLong(DoubleConverter.ToDouble(123.456)));
            Assert.Equal(123, LongConverter.ToLong("123"));
            Assert.Equal(123, LongConverter.ToLong("123.465"));
            Assert.Equal(123, LongConverter.ToLong("123.999"));
            Assert.Equal(0, LongConverter.ToLong(null));
        }

        [Fact]
        public void TestToLongWithDefault()
        {
            Assert.Equal(123, LongConverter.ToLongWithDefault(null, 123));
            Assert.Equal(0, LongConverter.ToLongWithDefault(false, 123));
            Assert.Equal(123, LongConverter.ToLongWithDefault("ABC", 123));
        }
    }
}
