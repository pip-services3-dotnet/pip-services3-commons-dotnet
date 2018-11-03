using System;
using Xunit;

namespace PipServices3.Commons.Convert
{
    //[TestClass]
    public class DateTimeConverterTest
    {
        [Fact]
        public void TestToDateTime()
        {
            //var emptyDate = new DateTime(0, 0, 0, 0, 0, 0, DateTimeKind.Utc);
            var emptyDate = default(DateTime);
            Assert.Equal(emptyDate, DateTimeConverter.ToDateTime(null));

            DateTime date1 = new DateTime(1975, 4, 8, 0, 0, 0);
            Assert.Equal(date1, DateTimeConverter.ToDateTimeWithDefault(null, date1));
            Assert.Equal(date1, DateTimeConverter.ToDateTime(new DateTime(1975, 4, 8)));

            DateTime date2 = new DateTime(123456);
            Assert.Equal(date2, DateTimeConverter.ToDateTime(123456));

            DateTime date3 = new DateTime(1975, 4, 8, 0, 0, 0, DateTimeKind.Utc);
            Assert.Equal(date3, DateTimeConverter.ToDateTime("1975-04-08T00:00:00Z"));
            Assert.Equal(date1, DateTimeConverter.ToDateTime("1975/04/08"));

            Assert.Equal(emptyDate, DateTimeConverter.ToDateTime("XYZ"));
        }
    }
}
