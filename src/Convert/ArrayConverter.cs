using System.Collections;
using System.Collections.Generic;

namespace PipServices.Commons.Convert
{
    /// <summary>
    /// Converts objects to an array.
    /// </summary>
    public class ArrayConverter
    {
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

        public static IList<object> ToArray(object value)
        {
            var result = ToNullableArray(value);
            return result ?? new List<object>();
        }

        public static IList<object> ToArrayWithDefault(object value, IList<object> defaultValue)
        {
            var result = ToNullableArray(value);
            return result ?? defaultValue;
        }
    }
}