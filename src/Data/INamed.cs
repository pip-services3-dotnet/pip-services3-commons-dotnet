namespace PipServices.Commons.Data
{
    /// <summary>
    /// Interface for data objects that have human-readable name
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// Gets the object name
        /// </summary>
        string Name { get; set; }
    }
}
