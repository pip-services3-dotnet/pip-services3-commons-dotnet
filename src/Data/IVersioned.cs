namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Interface for data objects that can be versioned.
    /// 
    /// Versioning is often used as optimistic concurrency mechanism.
    /// 
    /// The version doesn't have to be a number, but it is recommended to use sequential
    /// values to determine if one object has newer or older version than another one.
    /// 
    /// It is a common pattern to use the time of change as the object version.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyData: IStringIdentifiable, IVersioned 
    /// {
    ///     string id {get; set;}
    ///     string field1;
    ///     int field2;
    ///     string version {get; set;}
    ///     ...
    /// }
    /// public void updateData(string correlationId, MyData item) 
    /// {
    ///     ...
    ///     
    ///     if (item.Version < oldItem.Version) 
    ///     {
    ///         throw new ConcurrencyException(null, "VERSION_CONFLICT", "The change has older version stored value");
    ///     }
    ///     ...
    /// }
    /// </code>
    /// </example>
    public interface IVersioned
    {
        /** The object's version. */
        string Version { get; set; }
    }
}
