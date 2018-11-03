using System;
using Xunit;

namespace PipServices3.Commons.Random
{
   //[TestClass]
    public class RandomDateTimeTest
    {
        [Fact]
        public void TestNextDate()
        {
            var date1 = new DateTime(2015, 1, 1);
            var date2 = new DateTime(2016, 1, 1);
            var date = RandomDateTime.NextDate(date1, date2);
            Assert.True(date.Year == 2015 || date.Year == 2016);

            date = RandomDateTime.NextDate(date2);
            Assert.True(date >= new DateTime(2000, 1, 1) && date <= date2);
        }

        [Fact]
        public void TestUpdateDateTime()
        {
            DateTime oldDate = new DateTime(2016, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime date;

            date = RandomDateTime.UpdateDateTime(oldDate);
            Assert.True(date.DayOfYear >= oldDate.DayOfYear - 10
                || date.DayOfYear <= oldDate.DayOfYear + 10);

            date = RandomDateTime.UpdateDateTime(oldDate, 3);
            Assert.True(date.DayOfYear >= oldDate.DayOfYear - 3
                || date.DayOfYear <= oldDate.DayOfYear + 3);

            date = RandomDateTime.UpdateDateTime(oldDate, -3);
            Assert.True(date.DayOfYear == oldDate.DayOfYear);
        }

    }
}
