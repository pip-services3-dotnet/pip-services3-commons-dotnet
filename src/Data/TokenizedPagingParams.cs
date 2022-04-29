using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Data transfer object to pass tokenized paging parameters for queries.
    /// It can be used for complex paging scenarios, like paging across multiple databases
    /// where the previous state is encoded in a token.The token is usually retrieved from
    /// the previous response. The initial request shall go with token == <code>null</code>
    /// The page is defined by two parameters:
    /// - the<code> token</code> token that defines a starting point for the search.
    /// - the<code> take</code> parameter sets how many items to return in a page.
    /// - additionally, the optional <code>total</code> parameter tells to return total number of items in the query.
    /// 
    /// Remember: not all implementations support the <code>total</code> parameter
    /// because its generation may lead to severe performance implications.
    /// </summary>
    /// <example>
    /// <code>
    /// var filter = FilterParams.FromTuples("type", "Type1");
    /// var paging = new PagingParams(0, 100);
    /// var sorting = new SortingParams(new SortField("create_time", true));
    /// 
    /// myDataClient.GetDataByFilter(filter, paging, sorting);
    /// </code>
    /// </example>
    public class TokenizedPagingParams
    {
        /// <summary>
        /// The start token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The number of items to return.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// The flag to return the total number of items.
        /// </summary>
        public bool Total { get; set; }


        /// <summary>
        /// Creates a new instance and sets its values.
        /// </summary>
        /// <param name="token">token that defines a starting point for the search.</param>
        /// <param name="take">the number of items to return. </param>
        /// <param name="total">true to return the total number of items.</param>
        public TokenizedPagingParams(string token = null, int take=default(int), bool total = default(bool))
        {
            Token = token;
            Take = take;
            Total = !!total;

            /// This is for correctly using PagingParams with gRPC. gRPC defaults to 0 when take is null,
            /// so we have to set it back to null if we get 0 in the constructor.
            if (Take == 0)
                Take = default(int);
        }

        /// <summary>
        /// Gets the number of items to return in a page.
        /// </summary>
        /// <param name="maxTake">the maximum number of items to return.</param>
        /// <returns>the number of items to return.</returns>
        public int GetTake(int maxTake)
        {
            if (Take == default(int)) return maxTake;
            if (Take < 0) return 0;
            if (Take > maxTake) return maxTake;
            return Take;
        }

        /// <summary>
        /// Converts specified value into TokenizedPagingParams.
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>a newly created PagingParams.</returns>
        public static TokenizedPagingParams FromValue(object value)
        {
            if (value is TokenizedPagingParams)
                return (TokenizedPagingParams)value;

            var map = AnyValueMap.FromValue(value);
            return TokenizedPagingParams.FromMap(map);
        }

        /// <summary>
        /// Creates a new TokenizedPagingParams from a list of key-value pairs called tuples.
        /// </summary>
        /// <param name="tuples">a list of values where odd elements are keys and the following even elements are values</param>
        /// <returns>a newly created TokenizedPagingParams.</returns>
        public static TokenizedPagingParams FromTuples(params object[] tuples)
        {
            var map = AnyValueMap.FromTuplesArray(tuples);
            return TokenizedPagingParams.FromMap(map);
        }

        /// <summary>
        /// Creates a new TokenizedPagingParams and sets it parameters from the specified map
        /// </summary>
        /// <param name="map">a AnyValueMap to initialize this TokenizedPagingParams</param>
        /// <returns>a newly created PagingParams.</returns>
        public static TokenizedPagingParams FromMap(AnyValueMap map)
        {
            var token = map.GetAsNullableString("token");
            var take = map.GetAsNullableInteger("take").Value;
            var total = map.GetAsBooleanWithDefault("total", false);
            return new TokenizedPagingParams(token, take, total);
        }

        /// <summary>
        /// Creates a new TokenizedPagingParams and sets it parameters from the specified map
        /// </summary>
        /// <param name="map">a StringValueMap to initialize this TokenizedPagingParams</param>
        /// <returns>a newly created PagingParams.</returns>
        public static TokenizedPagingParams FromMap(StringValueMap map)
        {
            var token = map.GetAsNullableString("token");
            var take = map.GetAsNullableInteger("take").Value;
            var total = map.GetAsBooleanWithDefault("total", false);
            return new TokenizedPagingParams(token, take, total);
        }

    }
}
