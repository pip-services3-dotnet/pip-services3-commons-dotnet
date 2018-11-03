using System.Collections.Generic;

namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// Interface for a map that holds component references and passes them to components
    /// to establish dependencies with each other.
    /// 
    /// Together with IReferenceable and IUnreferenceable interfaces it implements
    /// a Locator pattern that is used by PipServices toolkit for Inversion of Control
    /// to assign external dependencies to components.
    /// 
    /// The IReferences object is a simple map, where keys are locators
    /// and values are component references.It allows to add, remove and find components
    /// by their locators.Locators can be any values like integers, strings or component types.
    /// But most often PipServices toolkit uses Descriptor as locators that match
    /// by 5 fields: group, type, kind, name and version.
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
    /// 
    /// var persistence = new MyMongoDbPersistence();
    /// 
    /// var controller = new MyController();
    /// 
    /// var references = References.FromTuples(
    /// new Descriptor("mygroup", "persistence", "mongodb", "default", "1.0"), persistence,
    /// new Descriptor("mygroup", "controller", "default", "default", "1.0"), controller );
    /// controller.SetReferences(references);
    /// </code>
    /// </example>
    /// See <see cref="Descriptor"/>, <see cref="References"/>
    public interface IReferences
    {
        /// <summary>
        /// Puts a new component reference to the set with explicit locator
        /// </summary>
        /// <param name="locator">a locator to find the reference by</param>
        /// <param name="component">a component reference to be added</param>
        void Put(object locator, object component);

        /// <summary>
        /// Removes a previously added reference that matches specified locator. If many
        /// references match the locator, it removes only the first one.When all
        /// references shall be removed, use removeAll() method instead.
        /// </summary>
        /// <param name="locator">a locator to find the reference to remove</param>
        /// <returns>a removed component reference</returns>
        /// See <see cref="RemoveAll(object)"/>
        object Remove(object locator);

        /// <summary>
        /// Removes all component references that match the specified locator.
        /// </summary>
        /// <param name="locator">a locator to remove the reference by  </param>
        /// <returns>a list, containing all removed references.</returns>
        List<object> RemoveAll(object locator);

        /// <summary>
        /// Gets locators for all registered component references in this reference map.
        /// </summary>
        /// <returns>a list with component locators</returns>
        List<object> GetAllLocators();

        /// <summary>
        /// Gets all component references registered in this reference map.
        /// </summary>
        /// <returns>a list with component references</returns>
        List<object> GetAll();

        /// <summary>
        /// Gets all component references that match specified locator.
        /// </summary>
        /// <param name="locator">a locator to find references by</param>
        /// <returns>a list with matching component references or empty list if nothing was found.</returns>
        List<object> GetOptional(object locator);

        /// <summary>
        /// Gets all component references that match specified locator and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">the locator to find references by.</param>
        /// <returns>a list with matching component references or empty list if nothing was found.</returns>
        List<T> GetOptional<T>(object locator);

        /// <summary>
        /// Gets all component references that match specified locator. At least one
        /// component reference must be present.If it doesn't the method throws an error.
        /// </summary>
        /// <param name="locator">a locator to find references</param>
        /// <returns>a list with found component references</returns>
        List<object> GetRequired(object locator);

        /// <summary>
        /// Gets all component references that match specified locator. At least one
        /// component reference must be present and matching to the specified type.
        /// 
        /// If it doesn't the method throws an error.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">a locator to find references</param>
        /// <returns>a list with found component references</returns>
        List<T> GetRequired<T>(object locator);

        /// <summary>
        /// Gets an optional component reference that matches specified locator.
        /// </summary>
        /// <param name="locator">a locator to find a reference</param>
        /// <returns>a found component reference or <code>null</code> if nothing was found</returns>
        object GetOneOptional(object locator);

        /// <summary>
        /// Gets an optional component reference that matches specified locator and
        /// matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">a locator to find a reference</param>
        /// <returns>a found component reference or <code>null</code> if nothing was found</returns>
        T GetOneOptional<T>(object locator);

        /// <summary>
        /// Gets a required component reference that matches specified locator.
        /// </summary>
        /// <param name="locator">a locator to find a reference</param>
        /// <returns>a matching component reference.</returns>
        object GetOneRequired(object locator);

        /// <summary>
        /// Gets a required component reference that matches specified locator and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">a locator to find a reference</param>
        /// <returns>a found component reference</returns>
        T GetOneRequired<T>(object locator);

        /// <summary>
        /// Gets all component references that match specified locator.
        /// </summary>
        /// <param name="locator">the locator to find a reference by.</param>
        /// <param name="required">forces to raise exception is no reference is found</param>
        /// <returns>a list with matching component references.</returns>
        List<object> Find(object locator, bool required);

        /// <summary>
        /// Gets all component references that match specified locator and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">the locator to find a reference by.</param>
        /// <param name="required">forces to raise exception is no reference is found</param>
        /// <returns>a list with matching component references.</returns>
        List<T> Find<T>(object locator, bool required);

    }
}