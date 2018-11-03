using System.Collections.Generic;
using System.Linq;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Defines a field name and order used to sort query results.
    /// </summary>
    /// <example>
    /// <code>
    /// var filter = FilterParams.fromTuples("type", "Type1");
    /// var paging = new PagingParams(0, 100);
    /// var sorting = new SortingParams(new SortField("create_time", true));
    /// 
    /// myDataClient.GetDataByFilter(filter, paging, sorting);
    /// </code>
    /// </example>
    public class SortParams : List<SortField>
    {
        /// <summary>
        /// Creates a new instance and initializes it with specified sort fields.
        /// </summary>
        /// <param name="fields">a list of fields to sort by.</param>
        public SortParams(IEnumerable<SortField> fields = null)
        {
            if (fields != null)
            {
                AddRange(fields);
            }
        }

        /// <summary>
        /// Creates a new instance and initializes it with specified sort fields.
        /// </summary>
        /// <param name="array">a list of fields to sort by.</param>
        public SortParams(AnyValueArray array)
        {
            if (array == null)
            {
                return;
            }

            for (int index = 0; index < array.Count; index++)
            {
                var value = array.GetAsValue(index);

                if (value != null)
                {
                    var map = value.GetAsMap();

                    if (map != null)
                    {
                        Add(new SortField(map.GetAsStringWithDefault("name", null), map.GetAsBooleanWithDefault("ascending", true)));
                    }
                }
            }
        }

        public static SortParams FromValue(object value)
        {
            if (value is SortParams)
            {
                return (SortParams)value;
            }

            var array = value != null ? AnyValueArray.FromValue(value) : new AnyValueArray();
            return new SortParams(array);
        }

        public override bool Equals(object obj)
        {
            var sortParams = obj as SortParams;

            return sortParams != null &&
                Count == sortParams.Count &&
                this.SequenceEqual(sortParams);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}