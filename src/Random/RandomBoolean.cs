using System;

namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for boolean values.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RandomBoolean.NextBoolean();    // Possible result: true
    /// var value2 = RandomBoolean.Chance(1,3);      // Possible result: false
    /// </code>
    /// </example>
    public class RandomBoolean
    {
        private static readonly System.Random _random = new System.Random();

        /// <summary>
        /// Calculates "chance" out of "max chances". Example: 1 chance out of 3 chances (or 33.3%)
        /// </summary>
        /// <param name="chances">a chance proportional to maxChances.</param>
        /// <param name="maxChances">a maximum number of chances</param>
        /// <returns>random boolean value.</returns>
        public static bool Chance(float chances, float maxChances)
        {
            chances = chances >= 0 ? chances : 0;
            maxChances = maxChances >= 0 ? maxChances : 0;
            if (chances == 0 && maxChances == 0)
                return false;

            maxChances = Math.Max(maxChances, chances);
            double start = (maxChances - chances) / 2;
            double end = start + chances;
            double hit = _random.NextDouble() * maxChances;
            return hit >= start && hit <= end;
        }

        /// <summary>
        /// Generates a random boolean value.
        /// </summary>
        /// <returns>random boolean value.</returns>
        public static bool NextBoolean()
        {
            return _random.Next(100) < 50;
        }
    }
}
