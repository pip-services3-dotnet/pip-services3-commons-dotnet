using PipServices.Commons.Convert;

using System.Runtime.Serialization;

namespace PipServices.Commons.Data
{
    [DataContract]
    public class PagingParams
    {
        public PagingParams() {}

        public PagingParams(object skip, object take, object total = null)
        {
            Skip = LongConverter.ToNullableLong(skip);
            Take = LongConverter.ToNullableLong(take);
            Total = BooleanConverter.ToBooleanWithDefault(total, false);
        }

        [DataMember]
        public long? Skip { get; set; }

        [DataMember]
        public long? Take { get; set; }

        [DataMember]
        public bool Total { get; set; }

        public long GetSkip(long minSkip = 0)
        {
            if (Skip == null) return minSkip;
            if (Skip.Value < minSkip) return minSkip;
            return Skip.Value;
        }

        public long GetTake(long maxTake)
        {
            if (Take == null) return maxTake;
            if (Take.Value < 0) return 0;
            if (Take.Value > maxTake) return maxTake;
            return Take.Value;
        }

        public static PagingParams FromValue(object value)
        {
            if (value is PagingParams)
                return (PagingParams)value;

            var map = AnyValueMap.FromValue(value);
            return FromMap(map);
        }

        public static PagingParams FromTuples(params object[] tuples)
        {
            var map = AnyValueMap.FromTuples(tuples);
            return FromMap(map);
        }

        public static PagingParams FromMap(AnyValueMap map)
        {
            var skip = map.GetAsNullableLong("skip");
            var take = map.GetAsNullableLong("take");
            var total = map.GetAsBooleanWithDefault("total", true);
            return new PagingParams(skip, take, total);
        }
       
        public override bool Equals(object obj)
        {
            var pagingParams = obj as PagingParams;

            return pagingParams != null &&
                Skip == pagingParams.Skip &&
                Take == pagingParams.Take &&
                Total == pagingParams.Total;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}