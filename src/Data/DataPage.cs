using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Data transfer object that is used to pass results of paginated queries.
    /// It contains items of retrieved page and optional total number of items.
    /// 
    /// Most often this object type is used to send responses to paginated queries.
    /// Pagination parameters are defined by PagingParams object.
    /// The <c>skip</c> parameter in the PagingParams there means how many items to skip.
    /// The <c>takes</c> parameter sets number of items to return in the page.
    /// And the optional <c>total</c> parameter tells to return total number of items in the query.
    /// 
    /// Remember: not all implementations support <c>total</c> parameter
    /// because its generation may lead to severe performance implications.
    /// </summary>
    /// <typeparam name="T">the class type</typeparam>
    /// <example>
    /// <code>
    /// myDataClient.GetDataByFilter(
    ///     "123", 
    ///     FilterParams.FromTuples("completed", true),
    ///     new PagingParams(0, 100, true),
    ///     async(DataPage<MyData> page) => {
    ///     Console.WriteLine("Items: ");
    ///     for (MyData item : page.getData()) {
    ///         Console.WriteLine(item);
    ///     }
    ///     Console.WriteLine("Total items: " + page.getTotal());
    ///     };
    /// );
    /// </code>
    /// </example>
    /// See <see cref="PagingParams"/>
    [DataContract]
    public class DataPage<T>
    {
        /// <summary>
        /// Creates a new instance of data page.
        /// </summary>
        public DataPage() { }

        /// <summary>
        /// Creates a new instance of data page and assigns its values.
        /// </summary>
        /// <param name="data">a list of items from the retrieved page.</param>
        /// <param name="total">(optional) .</param>
        public DataPage(List<T> data, long? total = null)
        {
            Data = data;
            Total = total;
        }
        
        /** The total amount of items in a request. */
        [DataMember(Name = "total")]
        public long? Total { get; set; }

        /** The items of the retrieved page. */
        [DataMember(Name = "data")]
        public List<T> Data { get; set; }
    }
}