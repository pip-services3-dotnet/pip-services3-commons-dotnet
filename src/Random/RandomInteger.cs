using System.Collections.Generic;

namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for integer values.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RandomInteger.NextInteger(5, 10);     // Possible result: 7
    /// var value2 = RandomInteger.NextInteger(10);        // Possible result: 3
    /// var value3 = RandomInteger.UpdateInteger(10, 3);   // Possible result: 9
    /// </code>
    /// </example>
    public static class RandomInteger
    {
        private static readonly System.Random _random = new System.Random();

        /// <summary>
        /// Generates a random integer value in the range to "max".
        /// </summary>
        /// <param name="maxValue">max range value</param>
        /// <returns>a random integer value.</returns>
        public static int NextInteger(int maxValue)
        {
            return _random.Next(maxValue);
        }

        /// <summary>
        /// Generates a random integer value in the range ["min", "max"].
        /// </summary>
        /// <param name="minValue">(optional) minimum range value</param>
        /// <param name="maxValue">max range value</param>
        /// <returns>a random integer value.</returns>
        public static int NextInteger(int minValue, int maxValue)
        {
            if (maxValue - minValue <= 0)
                return minValue;

            return minValue + _random.Next(maxValue - minValue);
        }

        /// <summary>
        /// Updates (drifts) a integer value without specified range defined
        /// </summary>
        /// <param name="value">a integer value to drift.</param>
        /// <returns>updated random integer value.</returns>
        public static int UpdateInteger(int value)
        {
            return UpdateInteger(value, 0);
        }

        /// <summary>
        /// Updates (drifts) a integer value within specified range defined
        /// </summary>
        /// <param name="value">a integer value to drift.</param>
        /// <param name="range">(optional) a range. Default: 10% of the value</param>
        /// <returns>updated random integer value.</returns>
        public static int UpdateInteger(int value, int range)
        {
            range = range == 0 ? (int)(0.1 * value) : range;
            int minValue = value - range;
            int maxValue = value + range;
            return NextInteger(minValue, maxValue);
        }

        /// <summary>
        /// Generates a random sequence of integers starting from 0 like: [0,1,2,3...??]
        /// </summary>
        /// <param name="size">size of sequence</param>
        /// <returns>generated array of integers.</returns>
        public static List<int> Sequence(int size)
        {
            return Sequence(size, size);
        }

        /// <summary>
        /// Generates a random sequence of integers starting from 0 like: [0,1,2,3...??]
        /// </summary>
        /// <param name="min">minimum value of the integer that will be generated. If 'max' is
        /// omitted, then 'max' is set to 'min' and 'min' is set to 0.</param>
        /// <param name="max">(optional) maximum value of the integer that will be generated.
        /// Defaults to 'min' if omitted.</param>
        /// <returns>generated array of integers.</returns>
        public static List<int> Sequence(int min, int max)
        {
            int count = NextInteger(min, max);

            List<int> result = new List<int>();
            for (int i = 0; i < count; i++)
                result.Add(i);

            return result;
        }
    }
}
