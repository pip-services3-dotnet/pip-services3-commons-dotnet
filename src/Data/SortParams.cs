using System.Collections.Generic;
using System.Linq;

namespace PipServices.Commons.Data
{
    public class SortParams : List<SortField>
    {
        public SortParams(IEnumerable<SortField> fields = null)
        {
            if (fields != null)
            {
                AddRange(fields);
            }
        }

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