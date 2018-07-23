using System.Collections.Generic;

namespace PipServices.Commons.Data
{
    public class FilterParams : StringValueMap
    {
        public FilterParams() { }

        public FilterParams(IDictionary<string, string> map)
        {
            Append(map);
        }

        public FilterParams(AnyValueMap map)
        {
            Append(map);
        }

        public new static FilterParams FromTuples(params object[] values)
        {
            var map = StringValueMap.FromTuples(values);
            return new FilterParams(map);
        }

        public new static FilterParams FromString(string line)
        {
            var map = StringValueMap.FromString(line);
            return new FilterParams(map);
        }

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