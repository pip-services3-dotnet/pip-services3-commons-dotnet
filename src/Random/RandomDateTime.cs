using System;

namespace PipServices.Commons.Random
{
    public class RandomDateTime
    {
        public static DateTime NextDate(DateTime max)
        {
            return NextDate(new DateTime(2000, 1, 1), max);
        }

        public static DateTime NextDate(DateTime min, DateTime max)
        {
            var diff = RandomDouble.NextDouble(max.Subtract(min).TotalSeconds);
            var date = min.AddSeconds(diff);
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, min.Kind);
        }

        public static TimeSpan NextTime(long max)
        {
            return NextTime(0, max);
        }

        public static TimeSpan NextTime(long min, long max)
        {
            var duration = RandomLong.NextLong(min, max);
            return TimeSpan.FromMilliseconds(duration);
        }

        public static DateTime NextDateTime(DateTime max)
        {
            return NextDateTime(new DateTime(2000, 1, 1), max);
        }

        public static DateTime NextDateTime(DateTime min, DateTime max)
        {
            var diff = RandomDouble.NextDouble(max.Subtract(min).TotalSeconds);
            return min.AddSeconds(diff);
        }

        public static DateTime UpdateDateTime(DateTime value)
        {
            return UpdateDateTime(value, 0);
        }

        public static DateTime UpdateDateTime(DateTime value, long range)
        {
            range = range != 0 ? range : 10L * 24 * 36000000;
            if (range < 0)
                return value;

            var diff = RandomLong.NextLong(-range, range);
            return value.AddMilliseconds(diff);
        }

    }
}
