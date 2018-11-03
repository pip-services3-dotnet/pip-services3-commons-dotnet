namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// Interface for components that require explicit clearing of references to dependent components.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyController: IReferenceable, IUnreferenceable 
    /// {
    ///     public IMyPersistence _persistence;
    ///     ...    
    ///     public void SetReferences(IReferences references)
    ///     {
    ///         this._persistence = references.getOneRequired<IMyPersistence>(
    ///         new Descriptor("mygroup", "persistence", "*", "*", "1.0") );
    ///     }
    ///     
    ///     public void UnsetReferences()
    ///     {
    ///         this._persistence = null;
    ///     }
    ///     ...
    /// }
    /// </code>
    /// </example>
    /// See <see cref="IReferences"/>, <see cref="IReferenceable"/>
    public interface IUnreferenceable
    {
        /// <summary>
        /// Unsets (clears) previously set references to dependent components. 
        /// </summary>
        void UnsetReferences();
    }
}
