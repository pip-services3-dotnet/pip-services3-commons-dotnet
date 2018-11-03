using System.Collections;
using System.Collections.Generic;

namespace PipServices3.Commons.Convert
{
    /// <summary>
    /// Converts arbitrary values into array objects.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = ArrayConverter.ToArray(1);		// Result: [1]
    /// var value2 = ArrayConverter.ToArray(new int[]{1, 2, 3}); // Result: [1, 2, 3]
    /// </code>
    /// </example>
    public class ArrayConverter
    {
        /// <summary>
        /// Converts value into array object. Single values are converted into arrays with a single element.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>array object or null when value is null.</returns>
        public static IList<object> ToNullableArray(object value)
        {
            // Return null when nothing found
            if (value == null)
            {
                return null;
            }

            // Convert enumerable
            if (value is IEnumerable)
            {
                var array = new List<object>();
                foreach (var item in (IEnumerable)value)
                    array.Add(item);
                return array;
            }
            else
            {
                // Convert single values
                var array = new List<object>();
                array.Add(value);
                return array;
            }
        }

        /// <summary>
        /// Converts value into array object with empty array as default. Single values
        /// are converted into arrays with single element.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <returns>array object or empty array when value is null.</returns>
        /// See <see cref="ArrayConverter.ToNullableArray(object)"/>
        public static IList<object> ToArray(object value)
        {
            var result = ToNullableArray(value);
            return result ?? new List<object>();
        }

        /// <summary>
        /// Converts value into array object with empty array as default. Single values 
        /// are converted into arrays with single element.
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <param name="defaultValue">default array object.</param>
        /// <returns>array object or empty array when value is null.</returns>
        /// See <see cref="ArrayConverter.ToNullableArray(object)"/>
        public static IList<object> ToArrayWithDefault(object value, IList<object> defaultValue)
        {
            var result = ToNullableArray(value);
            return result ?? defaultValue;
        }
    }
}