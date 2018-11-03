namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Interface for data objects that have human-readable name.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyData: IStringIdentifiable, INamed 
    /// {
    ///     string id {get; set;}
    ///     string name {get; set;}
    ///     string field1;
    ///     int field2;
    ///     ...
    /// }
    /// </code>
    /// </example>
    public interface INamed
    {
        /** The object's humand-readable name. */
        string Name { get; set; }
    }
}
