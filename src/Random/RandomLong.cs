using System.Collections.Generic;

namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for long values.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RandomLong.NextLong(5, 10);     // Possible result: 7
    /// var value2 = RandomLong.NextLong(10);        // Possible result: 3
    /// var value3 = RandomLong.UpdateLong(10, 3);   // Possible result: 9
    /// </code>
    /// </example>
    public static class RandomLong
    {
        private static readonly System.Random _random = new System.Random();

        /// <summary>
        /// Generates a random long value in the range to "max".
        /// </summary>
        /// <param name="maxValue">max range value</param>
        /// <returns>a random long value.</returns>
        public static long NextLong(long maxValue)
        {
            return _random.Next((int)maxValue);
        }

        /// <summary>
        /// Generates a random long value in the range ["min", "max"].
        /// </summary>
        /// <param name="minValue">(optional) minimum range value</param>
        /// <param name="maxValue">max range value</param>
        /// <returns>a random long value.</returns>
        public static long NextLong(long minValue, long maxValue)
        {
            if (maxValue - minValue <= 0)
                return minValue;

            return minValue + _random.Next((int)(maxValue - minValue));
        }

        /// <summary>
        /// Updates (drifts) a long value without specified range defined
        /// </summary>
        /// <param name="value">a long value to drift.</param>
        /// <returns>updated random long value.</returns>
        public static long UpdateLong(long value)
        {
            return UpdateLong(value, 0);
        }

        /// <summary>
        /// Updates (drifts) a long value within specified range defined
        /// </summary>
        /// <param name="value">a long value to drift.</param>
        /// <param name="range">(optional) a range. Default: 10% of the value</param>
        /// <returns>updated random long value.</returns>
        public static long UpdateLong(long value, long range)
        {
            range = range == 0 ? (long)(0.1 * value) : range;
            long minValue = value - range;
            long maxValue = value + range;
            return NextLong(minValue, maxValue);
        }

        /// <summary>
        /// Generates a random sequence of longs starting from 0 like: [0,1,2,3...??]
        /// </summary>
        /// <param name="size">size of sequence</param>
        /// <returns>generated array of longs.</returns>
        public static List<long> Sequence(long size)
        {
            return Sequence(size, size);
        }

        /// <summary>
        /// Generates a random sequence of longs starting from 0 like: [0,1,2,3...??]
        /// </summary>
        /// <param name="min">minimum value of the long that will be generated. If 'max' is
        /// omitted, then 'max' is set to 'min' and 'min' is set to 0.</param>
        /// <param name="max">(optional) maximum value of the long that will be generated.
        /// Defaults to 'min' if omitted.</param>
        /// <returns>generated array of longs.</returns>
        public static List<long> Sequence(long min, long max)
        {
            long count = NextLong(min, max);

            List<long> result = new List<long>();
            for (long i = 0; i < count; i++)
                result.Add(i);

            return result;
        }
    }
}
