namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// Interface for components that depends on other components. 
    /// 
    /// If component requires explicit notification to unset references
    /// it shall additionally implement IUnreferenceable interface.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyController: IReferenceable 
    /// {
    ///     public IMyPersistence _persistence;
    ///     ...    
    ///     public void SetReferences(IReferences references)
    ///     {
    ///         this._persistence = references.getOneRequired<IMyPersistence>(
    ///         new Descriptor("mygroup", "persistence", "*", "*", "1.0")
    ///         );
    ///     }
    ///     ...
    /// }
    /// </code>
    /// </example>
    /// See <see cref="IReferences"/>, <see cref="IUnreferenceable"/>, <see cref="Referencer"/>
    public interface IReferenceable
    {
        /// <summary>
        /// Sets references to dependent components.
        /// </summary>
        /// <param name="references">references to locate the component dependencies.</param>
        /// See <see cref="IReferences"/>
        void SetReferences(IReferences references);
    }
}
