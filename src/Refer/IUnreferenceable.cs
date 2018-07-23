namespace PipServices.Commons.Refer
{
    /// <summary>
    /// Interface for components that require clear of references to other components
    /// </summary>
    public interface IUnreferenceable
    {
        /// <summary>
        /// Unsets previously set references to other components.
        /// </summary>
        void UnsetReferences();
    }
}
