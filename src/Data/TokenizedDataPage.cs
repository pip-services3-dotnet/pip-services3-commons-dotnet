using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Data transfer object that is used to pass results of paginated queries.
    /// It contains items of retrieved page and optional total number of items.
    /// 
    /// Most often this object type is used to send responses to paginated queries.
    /// Pagination parameters are defined by[[TokenizedPagingParams]] object.
    /// The<code> token</code> parameter in the TokenizedPagingParams there means where to start the searxh.
    /// The<code> takes</code> parameter sets number of items to return in the page.
    /// And the optional<code> total</code> parameter tells to return total number of items in the query.
    /// 
    /// The data page returns a token that shall be passed to the next search as a starting point.
    /// 
    /// Remember: not all implementations support the<code> total</code> parameter
    /// because its generation may lead to severe performance implications.
    /// </summary>
    /// <example>
    /// <code>
    /// var page = await myDataClient.GetDataByFilterAsync(
    ///     "123",
    ///     FilterParams.FromTuples("completed": true),
    ///     new TokenizedPagingParams(null, 100, true)
    /// );
    /// 
    /// myDataClient.GetDataByFilterAsync(filter, paging, projection);
    /// </code>
    /// </example>
    /// <typeparam name="T"></typeparam>
    public class TokenizedDataPage<T>
    {
        /// <summary>
        /// The items of the retrieved page.
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// The starting point for the next search.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The total amount of items in a request.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Creates a new instance of data page and assigns its values.
        /// </summary>
        /// <param name="data"> a list of items from the retrieved page.</param>
        /// <param name="token">(optional) a token to define astarting point for the next search.</param>
        /// <param name="total">(optional) a total number of objects in the result.</param>
        public TokenizedDataPage(List<T> data = null, string token=null, int total = default(int))
        {
            Total = total;
            Token = token;
            Data = data;
        }
    }
}
