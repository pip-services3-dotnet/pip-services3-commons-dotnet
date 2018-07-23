using System;

namespace PipServices.Commons.Random
{
    public class RandomDateTime
    {
        public static DateTime NextDate()
        {
            return NextDate(0, 0);
        }

        public static DateTime NextDate(int year)
        {
            return NextDate(year, year);
        }

        public static DateTime NextDate(int minYear, int maxYear)
        {
            int currentYear = DateTime.Now.Year;
            minYear = minYear == 0 ? currentYear - RandomInteger.NextInteger(10) : minYear;
            maxYear = maxYear == 0 ? currentYear : maxYear;

            int year = RandomInteger.NextInteger(minYear, maxYear);
            int month = RandomInteger.NextInteger(1, 13);
            int day = RandomInteger.NextInteger(1, 32);

            if (month == 2)
                day = Math.Min(28, day);
            else if (month == 4 || month == 6 || month == 9 || month == 11)
                day = Math.Min(30, day);
            return new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Utc);
        }

        public static TimeSpan NextTime()
        {
            int hour = RandomInteger.NextInteger(0, 24);
            int min = RandomInteger.NextInteger(0, 60);
            int sec = RandomInteger.NextInteger(0, 60);
            int millis = RandomInteger.NextInteger(0, 1000);

            return new TimeSpan(hour, min, sec, millis);
        }

        public static DateTime NextDateTime()
        {
            return NextDateTime(0, 0);
        }

        public static DateTime NextDateTime(int year)
        {
            return NextDateTime(year, year);
        }

        public static DateTime NextDateTime(int minYear, int maxYear)
        {
            return NextDate(minYear, maxYear)
                .AddSeconds(RandomInteger.NextInteger(3600 * 24 * 365));
        }

        public static DateTime UpdateDateTime(DateTime value)
        {
            return UpdateDateTime(value, 0);
        }

        public static DateTime UpdateDateTime(DateTime value, float range)
        {
            range = range != 0 ? range : 10;
            if (range < 0)
                return value;

            float days = RandomFloat.NextFloat(-range, range);
            return value.AddDays((int)days);
        }

    }
}
