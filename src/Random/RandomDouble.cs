namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for double values.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RandomDouble.NextDouble(5, 10);     // Possible result: 7.3
    /// var value2 = RandomDouble.NextDouble(10);        // Possible result: 3.7
    /// var value3 = RandomDouble.UpdateDouble(10, 3);   // Possible result: 9.2
    /// </code>
    /// </example>
    public class RandomDouble
    {
        private static readonly System.Random _random = new System.Random();

        /// <summary>
        /// Generates a random double value in the range to "max".
        /// </summary>
        /// <param name="maxValue">max range value</param>
        /// <returns>a random double value.</returns>
        public static double NextDouble(double maxValue)
        {
            return _random.NextDouble() * maxValue;
        }

        /// <summary>
        /// Generates a random double value in the range ["min", "max"].
        /// </summary>
        /// <param name="minValue">(optional) minimum range value</param>
        /// <param name="maxValue">max range value</param>
        /// <returns>a random double value.</returns>
        public static double NextDouble(double minValue, double maxValue)
        {
            return minValue + _random.NextDouble() * (maxValue - minValue);
        }

        /// <summary>
        /// Updates (drifts) a double value without specified range defined
        /// </summary>
        /// <param name="value">a double value to drift.</param>
        /// <returns>updated random double value.</returns>
        public static double UpdateDouble(double value)
        {
            return UpdateDouble(value, 0);
        }

        /// <summary>
        /// Updates (drifts) a double value within specified range defined
        /// </summary>
        /// <param name="value">a double value to drift.</param>
        /// <param name="range">(optional) a range. Default: 10% of the value</param>
        /// <returns>updated random double value.</returns>
        public static double UpdateDouble(double value, double range)
        {
            range = range == 0 ? 0.1 * value : range;
            double minValue = value - range;
            double maxValue = value + range;
            return NextDouble(minValue, maxValue);
        }
    }
}
