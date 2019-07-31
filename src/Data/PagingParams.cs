using PipServices3.Commons.Convert;

using System.Runtime.Serialization;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Data transfer object to pass paging parameters for queries.
    /// The page is defined by two parameters.
    /// The <c>skip</c> parameter defines number of items to skip.
    /// The Paging parameter sets how many items to return in a page.
    /// And the optional <c>total</c> parameter tells to return total number of items in the query.
    /// 
    /// Remember: not all implementations support <c>total</c> parameter
    /// because its generation may lead to severe performance implications.
    /// </summary>
    /// <example>
    /// <code>
    /// var filter = FilterParams.FromTuples("type", "Type1");
    /// var paging = new PagingParams(0, 100);
    /// 
    /// myDataClient.GetDataByFilter(filter, paging);
    /// </code>
    /// </example>
    [DataContract]
    public class PagingParams
    {
        /// <summary>
        /// Creates a new instance and sets its values.
        /// </summary>
        public PagingParams() {}

        /// <summary>
        /// Creates a new instance and sets its values.
        /// </summary>
        /// <param name="skip">the number of items to skip.</param>
        /// <param name="take">the number of items to return.</param>
        /// <param name="total">true to return the total number of items.</param>
        public PagingParams(object skip, object take, object total = null)
        {
            Skip = LongConverter.ToNullableLong(skip);
            Take = LongConverter.ToNullableLong(take);
            Total = BooleanConverter.ToBooleanWithDefault(total, false);
        }

        /** The number of items to skip. */
        [DataMember]
        public long? Skip { get; set; }

        /** The number of items to return. */
        [DataMember]
        public long? Take { get; set; }

        /** The flag to return the total number of items. */
        [DataMember]
        public bool Total { get; set; }

        /// <summary>
        /// Gets the number of items to skip.
        /// </summary>
        /// <param name="minSkip">the minimum number of items to skip.</param>
        /// <returns>the number of items to skip.</returns>
        public long GetSkip(long minSkip = 0)
        {
            if (Skip == null) return minSkip;
            if (Skip.Value < minSkip) return minSkip;
            return Skip.Value;
        }

        /// <summary>
        /// Gets the number of items to return in a page.
        /// </summary>
        /// <param name="maxTake">the maximum number of items to return.</param>
        /// <returns>the number of items to return.</returns>
        public long GetTake(long maxTake)
        {
            if (Take == null) return maxTake;
            if (Take.Value < 0) return 0;
            if (Take.Value > maxTake) return maxTake;
            return Take.Value;
        }

        /// <summary>
        /// Converts specified value into PagingParams.
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>a newly created PagingParams.</returns>
        public static PagingParams FromValue(object value)
        {
            if (value is PagingParams)
                return (PagingParams)value;

            var map = AnyValueMap.FromValue(value);
            return FromMap(map);
        }

        /// <summary>
        /// Creates a new PagingParams from a list of key-value pairs called tuples.
        /// </summary>
        /// <param name="tuples">a list of values where odd elements are keys and the following
        /// even elements are values</param>
        /// <returns>a newly created PagingParams.</returns>
        public static PagingParams FromTuples(params object[] tuples)
        {
            var map = AnyValueMap.FromTuples(tuples);
            return FromMap(map);
        }

        /// <summary>
        /// Creates a new PagingParams and sets it parameters from the AnyValueMap map
        /// </summary>
        /// <param name="map">a AnyValueMap to initialize this PagingParams</param>
        /// <returns>a newly created PagingParams.</returns>
        public static PagingParams FromMap(AnyValueMap map)
        {
            var skip = map.GetAsNullableLong("skip");
            var take = map.GetAsNullableLong("take");
            var total = map.GetAsBooleanWithDefault("total", false);
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
