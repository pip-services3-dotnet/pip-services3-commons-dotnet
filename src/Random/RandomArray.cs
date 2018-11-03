using System.Collections.Generic;

namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for array objects.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RandomArray.Pick(new int []{1, 2, 3, 4}); // Possible result: 3
    /// </code>
    /// </example>
    public class RandomArray
    {
        /// <summary>
        /// Picks a random element from specified array.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="values">an array of any type</param>
        /// <returns>a randomly picked item.</returns>
        public static T Pick<T>(T[] values)
        {
            if (values == null || values.Length == 0)
                return default(T);

            return values[RandomInteger.NextInteger(values.Length)];
        }

        /// <summary>
        /// Picks a random element from specified array.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="values">an list of values of any type</param>
        /// <returns>a randomly picked item.</returns>
        public static T Pick<T>(List<T> values)
        {
            if (values == null || values.Count == 0)
                return default(T);

            int index = RandomInteger.NextInteger(values.Count);
            return values[index];
        }
    }
}
