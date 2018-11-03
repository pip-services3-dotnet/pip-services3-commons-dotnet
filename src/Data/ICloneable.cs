namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Interface for data objects that are able to create their full binary copy.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyClass: IMyClass, ICloneable 
    /// {
    ///     MyClass() { };
    ///     
    ///     public object clone()
    ///     {
    ///         var cloneObj = new Object(this);
    ///         // Copy every attribute from this to cloneObj here.
    ///         ...
    ///         return cloneObj;
    ///     }
    /// }
    /// </code>
    /// </example>
    public interface ICloneable
    {
        /// <summary>
        /// Creates a binary clone of this object.
        /// </summary>
        /// <returns>a clone of this object.</returns>
        object Clone();
    }
}