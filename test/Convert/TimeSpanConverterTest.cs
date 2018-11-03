using System;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class TimeSpanConverterTest
    {
        [Fact]
        public void TestToNullableDuration()
        {
            Assert.Null(TimeSpanConverter.ToNullableTimeSpan(null));
            Assert.True(TimeSpanConverter.ToNullableTimeSpan((int)6000).Value.Seconds == 6);
            Assert.True(TimeSpanConverter.ToNullableTimeSpan((short)6000).Value.Seconds == 6);
            Assert.True(TimeSpanConverter.ToNullableTimeSpan(6000.5).Value.Seconds == 6);
            //Assert.True(TimeSpanConverter.ToNullableTimeSpan(-600).Value.Seconds == -1);
            Assert.True(TimeSpanConverter.ToNullableTimeSpan(0).Value.Seconds == 0);
        }

        [Fact]
        public void TestToDateTime()
        {
            //Assert.Equal(default(TimeSpan), TimeSpanConverter.ToTimeSpan(null));
            Assert.True(TimeSpanConverter.ToTimeSpan((int)6000).Seconds == 6);
            Assert.True(TimeSpanConverter.ToTimeSpan((short)6000).Seconds == 6);
            Assert.True(TimeSpanConverter.ToTimeSpan(6000.5).Seconds == 6);
            //Assert.True(TimeSpanConverter.ToTimeSpan(-600).Seconds == -1);
            Assert.True(TimeSpanConverter.ToTimeSpan(0).Seconds == 0);
        }
    }
}
