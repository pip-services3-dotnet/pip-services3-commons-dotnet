using System;
using Xunit;

namespace PipServices.Commons.Random
{
   //[TestClass]
    public class RandomDateTimeTest
    {
        [Fact]
        public void TestNextDate()
        {
            DateTime date;
            date = RandomDateTime.NextDate(2015, 2016);
            Assert.True(date.Year == 2015 || date.Year == 2016);

            date = RandomDateTime.NextDate(0, 0);
            Assert.True(date.Year >= DateTime.Now.Year - 10
                && date.Year <= DateTime.Now.Year);

            date = RandomDateTime.NextDate();
            Assert.True(date.Year >= DateTime.Now.Year - 10
                && date.Year <= DateTime.Now.Year);
        }

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
