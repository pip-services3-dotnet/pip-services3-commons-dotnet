using System.Collections.Generic;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Data transfer object used to pass filter parameters as simple key-value pairs.
    /// </summary>
    /// <example>
    /// <code>
    /// var filter = FilterParams.fromTuples(
    /// "type", "Type1",
    /// "from_create_time", new Date(2000, 0, 1),
    /// "to_create_time", new Date(),
    /// "completed", true );
    /// 
    /// var paging = new PagingParams(0, 100);
    /// 
    /// myDataClient.GetDataByFilter(filter, paging);
    /// </code>
    /// </example>
    /// See <see cref="StringValueMap"/>
    public class FilterParams : StringValueMap
    {
        /// <summary>
        /// Creates a new instance and initalizes it.
        /// </summary>
        public FilterParams() { }

        /// <summary>
        /// Creates a new instance and initalizes it with elements from the specified map.
        /// </summary>
        /// <param name="map">a map to initialize this instance.</param>
        public FilterParams(IDictionary<string, string> map)
        {
            Append(map);
        }

        /// <summary>
        /// Creates a new instance and initalizes it with elements from the specified map.
        /// </summary>
        /// <param name="map">a map to initialize this instance.</param>
        public FilterParams(AnyValueMap map)
        {
            Append(map);
        }

        /// <summary>
        /// Creates a new FilterParams from a list of key-value pairs called tuples.
        /// </summary>
        /// <param name="values">a list of values where odd elements are keys and the following
        /// even elements are values</param>
        /// <returns>a newly created FilterParams.</returns>
        public new static FilterParams FromTuples(params object[] values)
        {
            var map = StringValueMap.FromTuples(values);
            return new FilterParams(map);
        }

        /// <summary>
        /// Parses semicolon-separated key-value pairs and returns them as a FilterParams.
        /// </summary>
        /// <param name="line">semicolon-separated key-value list to initialize FilterParams.</param>
        /// <returns>a newly created FilterParams.</returns>
        public new static FilterParams FromString(string line)
        {
            var map = StringValueMap.FromString(line);
            return new FilterParams(map);
        }

        /// <summary>
        /// Converts specified value into FilterParams.
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>a newly created FilterParams.</returns>
        public new static FilterParams FromValue(object value)
        {
            if (value is FilterParams)
                return (FilterParams)value;

            var map = AnyValueMap.FromValue(value);
            return new FilterParams(map);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            var filterParams = obj as FilterParams;

            return filterParams != null &&
                Keys.Count == filterParams.Keys.Count &&
                Values.Count == filterParams.Values.Count &&
                ToString().Equals(filterParams.ToString());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}