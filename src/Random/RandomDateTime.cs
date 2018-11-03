using System;

namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for Date time values.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RandomDateTime.NextDate(2010, 0);    // Possible result: 2008-01-03
    /// var value2 = RandomDateTime.NextDateTime(2017, 0);// Possible result: 20017-03-11 11:20:32
    /// </code>
    /// </example>
    public class RandomDateTime
    {
        /// <summary>
        /// Generates a random DateTime in the range ['2000, 1, 1', 'maxYear']. This
        /// method generate dates without time(or time set to 00:00:00)
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static DateTime NextDate(DateTime max)
        {
            return NextDate(new DateTime(2000, 1, 1), max);
        }

        /// <summary>
        /// Generates a random DateTime in the range ['minYear', 'maxYear']. This
        /// method generate dates without time(or time set to 00:00:00)
        /// </summary>
        /// <param name="min">(optional) minimum range value</param>
        /// <param name="max">max range value</param>
        /// <returns>a random DateTime value.</returns>
        public static DateTime NextDate(DateTime min, DateTime max)
        {
            var diff = RandomDouble.NextDouble(max.Subtract(min).TotalSeconds);
            var date = min.AddSeconds(diff);
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, min.Kind);
        }

        /// <summary>
        /// Generates a random TimeSpan in the range ['0', 'maxTime']. 
        /// </summary>
        /// <param name="max">max range value</param>
        /// <returns>a random TimeSpan value.</returns>
        public static TimeSpan NextTime(long max)
        {
            return NextTime(0, max);
        }

        /// <summary>
        /// Generates a random TimeSpan in the range ['minTime', 'maxTime']. 
        /// </summary>
        /// <param name="min">minimum range value</param>
        /// <param name="max">max range value</param>
        /// <returns>a random TimeSpan value.</returns>
        public static TimeSpan NextTime(long min, long max)
        {
            var duration = RandomLong.NextLong(min, max);
            return TimeSpan.FromMilliseconds(duration);
        }

        /// <summary>
        /// Generates a random DateTime and time in the range ['2000, 1, 1', 'maxYear']. 
        /// This method generate dates without time(or time set to 00:00:00)
        /// </summary>
        /// <param name="max">max range value</param>
        /// <returns>a random DateTime value.</returns>
        public static DateTime NextDateTime(DateTime max)
        {
            return NextDateTime(new DateTime(2000, 1, 1), max);
        }

        /// <summary>
        /// Generates a random DateTime and time in the range ['minYear', 'maxYear']. 
        /// This method generate dates without time(or time set to 00:00:00)
        /// </summary>
        /// <param name="min">minimum range value</param>
        /// <param name="max">max range value</param>
        /// <returns>a random DateTime value.</returns>
        public static DateTime NextDateTime(DateTime min, DateTime max)
        {
            var diff = RandomDouble.NextDouble(max.Subtract(min).TotalSeconds);
            return min.AddSeconds(diff);
        }

        /// <summary>
        /// Updates (drifts) a DateTime value.
        /// </summary>
        /// <param name="value">a DateTime value to drift.</param>
        /// <returns>an updated ZonedDateTime and time value.</returns>
        public static DateTime UpdateDateTime(DateTime value)
        {
            return UpdateDateTime(value, 0);
        }

        /// <summary>
        /// Updates (drifts) a ZonedDateTime value within specified range defined
        /// </summary>
        /// <param name="value">a DateTime value to drift.</param>
        /// <param name="range">(optional) a range in milliseconds. Default: 10 days</param>
        /// <returns>an updated DateTime and time value.</returns>
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
