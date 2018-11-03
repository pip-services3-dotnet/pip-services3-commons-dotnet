namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for float values.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RandomFloat.NextFloat(5, 10);     // Possible result: 7.3
    /// var value2 = RandomFloat.NextFloat(10);        // Possible result: 3.7
    /// var value3 = RandomFloat.UpdateFloat(10, 3);   // Possible result: 9.2
    /// </code>
    /// </example>
    public class RandomFloat
    {
        private static readonly System.Random _random = new System.Random();

        /// <summary>
        /// Generates a random float value in the range to "max".
        /// </summary>
        /// <param name="maxValue">max range value</param>
        /// <returns>a random float value.</returns>
        public static float NextFloat(int maxValue)
        {
            return (float)_random.NextDouble() * maxValue;
        }

        /// <summary>
        /// Generates a random float value in the range ["min", "max"].
        /// </summary>
        /// <param name="minValue">(optional) minimum range value</param>
        /// <param name="maxValue">max range value</param>
        /// <returns>a random float value.</returns>
        public static float NextFloat(float minValue, float maxValue)
        {
            return (float)(minValue + _random.NextDouble() * (maxValue - minValue));
        }

        /// <summary>
        /// Updates (drifts) a float value without specified range defined
        /// </summary>
        /// <param name="value">a float value to drift.</param>
        /// <returns>updated random float value.</returns>
        public static float UpdateFloat(float value)
        {
            return UpdateFloat(value, 0);
        }

        /// <summary>
        /// Updates (drifts) a float value within specified range defined
        /// </summary>
        /// <param name="value">a float value to drift.</param>
        /// <param name="range">(optional) a range. Default: 10% of the value</param>
        /// <returns>updated random float value.</returns>
        public static float UpdateFloat(float value, float range)
        {
            range = range == 0 ? (float)(0.1 * value) : range;
            float minValue = value - range;
            float maxValue = value + range;
            return NextFloat(minValue, maxValue);
        }
    }
}
