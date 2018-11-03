namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Generic interface for data objects that can be uniquely identified by an id.
    /// 
    /// The type specified in the interface defines the type of id field.
    /// </summary>
    /// <typeparam name="T">the class type</typeparam>
    /// <example>
    /// <code>
    /// public class MyData: IIdentifiable<String> 
    /// {
    ///     string id {get; set;}
    ///     string field1;
    ///     int field2;
    ///     ...
    /// }
    /// </code>
    /// </example>
    public interface IIdentifiable<T>
    {
        /** The unique object identifier of type T. */
        T Id { get; set; }
    }
}