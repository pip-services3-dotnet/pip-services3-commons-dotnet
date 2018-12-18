using System;
using System.Linq;

namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for enum values.
    /// </summary>
    public class RandomEnum
    {
        /// <summary>
        /// Generate a random enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">T must be an enumerated type</exception>
        public static T NextEnum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            Type etype = typeof(T);
            T[] vals = etype.GetEnumValues().Cast<T>().ToArray();
            return RandomArray.Pick(vals);
        }
    }
}
