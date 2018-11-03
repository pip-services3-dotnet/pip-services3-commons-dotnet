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
    /// See <see cref="SortParams"/>
    public class SortField
    {
        /// <summary>
        /// Creates a new instance and assigns its values.
        /// </summary>
        /// <param name="name">the name of the field to sort by.</param>
        /// <param name="ascending">true to sort in ascending order, and false to sort in descending order.</param>
        public SortField(string name = null, bool ascending = true)
        {
            Name = name;
            Ascending = ascending;
        }

        /** The field name to sort by */
        public string Name { get; set; }
        /** The flag to define sorting order. True to sort ascending, false to sort descending */
        public bool Ascending { get; set; }
    }
}
