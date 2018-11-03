using System;
using System.Collections;
using System.Collections.Generic;

namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// The most basic implementation of IReferences to store and locate component references.
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
    ///         new Descriptor("mygroup", "persistence", "*", "*", "1.0") );
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
    /// new Descriptor("mygroup", "controller", "default", "default", "1.0"), controller);
    /// controller.SetReferences(references);
    /// </code>
    /// </example>
    /// See <see cref="IReferences"/>
    public class References : IReferences
    {
        protected readonly List<Reference> _references = new List<Reference>();
        protected readonly object _lock = new object();

        public References() { }

        /// <summary>
        /// Creates a new instance of references and initializes it with references.
        /// </summary>
        /// <param name="tuples">(optional) a list of values where odd elements are locators and
        /// the following even elements are component references</param>
        public References(object[] tuples)
        {
            if (tuples != null)
            {
                for (int index = 0; index < tuples.Length; index += 2)
                {
                    if (index + 1 >= tuples.Length) break;

                    Put(tuples[index], tuples[index + 1]);
                }
            }
        }

        /// <summary>
        /// Puts a new reference into this reference map.
        /// </summary>
        /// <param name="locator">a locator to find the reference by.</param>
        /// <param name="component">a component reference to be added.</param>
        public virtual void Put(object locator, object component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            lock (_lock)
            {
                _references.Add(new Reference(locator, component));
            }
        }

        /// <summary>
        /// Removes a previously added reference that matches specified locator. If many
        /// references match the locator, it removes only the first one.When all
        /// references shall be removed, use removeAll() method instead.
        /// </summary>
        /// <param name="locator">a locator to remove reference</param>
        /// <returns>the removed component reference.</returns>
        /// See <see cref="RemoveAll(object)"/>
        public virtual object Remove(object locator)
        {
            if (locator == null) return null;

            lock (_lock)
            {
                for (int index = _references.Count - 1; index >= 0; index--)
                {
                    var reference = _references[index];
                    if (reference.Match(locator))
                    {
                        // Remove from the set
                        _references.RemoveAt(index);
                        return reference.GetComponent();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Removes all component references that match the specified locator.
        /// </summary>
        /// <param name="locator">the locator to remove references by.</param>
        /// <returns>a list, containing all removed references.</returns>
        public virtual List<object> RemoveAll(object locator)
        {
            var components = new List<object>();

            lock (_lock)
            {
                for (int index = _references.Count - 1; index >= 0; index--)
                {
                    var reference = _references[index];
                    if (reference.Match(locator))
                    {
                        // Remove from the set
                        _references.RemoveAt(index);
                        components.Add(reference.GetComponent());
                    }
                }
            }

            return components;
        }

        /// <summary>
        /// Gets locators for all registered component references in this reference map.
        /// </summary>
        /// <returns>a list with component locators.</returns>
        public virtual List<object> GetAllLocators()
        {
            var locators = new List<object>();
            foreach (var reference in _references)
                locators.Add(reference.GetLocator());
            return locators;
        }

        /// <summary>
        /// Gets all component references registered in this reference map.
        /// </summary>
        /// <returns>a list with component references.</returns>
        public virtual List<object> GetAll()
        {
            var components = new List<object>();
            foreach (var reference in _references)
                components.Add(reference.GetComponent());
            return components;
        }

        /// <summary>
        /// Gets an optional component reference that matches specified locator.
        /// </summary>
        /// <param name="locator">a locator to find a reference</param>
        /// <returns>a found component reference or <code>null</code> if nothing was found</returns>
        public virtual object GetOneOptional(object locator)
        {
            var components = Find<object>(locator, false);
            return components.Count > 0 ? components[0] : null;
        }

        /// <summary>
        /// Gets an optional component reference that matches specified locator and
        /// matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">a locator to find a reference</param>
        /// <returns>a found component reference or <code>null</code> if nothing was found</returns>
        public virtual T GetOneOptional<T>(object locator)
        {
            var components = Find<T>(locator, false);
            return components.Count > 0 ? components[0] : default(T);
        }

        /// <summary>
        /// Gets a required component reference that matches specified locator.
        /// </summary>
        /// <param name="locator">a locator to find a reference</param>
        /// <returns>a matching component reference.</returns>
        public virtual object GetOneRequired(object locator)
        {
            var components = Find<object>(locator, true);
            return components.Count > 0 ? components[0] : null;
        }

        /// <summary>
        /// Gets a required component reference that matches specified locator and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">a locator to find a reference</param>
        /// <returns>a found component reference</returns>
        public virtual T GetOneRequired<T>(object locator)
        {
            var components = Find<T>(locator, true);
            return components.Count > 0 ? components[0] : default(T);
        }

        /// <summary>
        /// Gets all component references that match specified locator.
        /// </summary>
        /// <param name="locator">a locator to find references by</param>
        /// <returns>a list with matching component references or empty list if nothing was found.</returns>
        public virtual List<object> GetOptional(object locator)
        {
            return Find<object>(locator, false);
        }

        /// <summary>
        /// Gets all component references that match specified locator and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">the locator to find references by.</param>
        /// <returns>a list with matching component references or empty list if nothing was found.</returns>
        public virtual List<T> GetOptional<T>(object locator)
        {
            return Find<T>(locator, false);
        }

        /// <summary>
        /// Gets all component references that match specified locator. At least one
        /// component reference must be present.
        /// </summary>
        /// <param name="locator">a locator to find references</param>
        /// <returns>a list with found component references</returns>
        public virtual List<object> GetRequired(object locator)
        {
            return Find<object>(locator, true);
        }

        /// <summary>
        /// Gets all component references that match specified locator. At least one
        /// component reference must be present and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">a locator to find references</param>
        /// <returns>a list with found component references</returns>
        public virtual List<T> GetRequired<T>(object locator)
        {
            return Find<T>(locator, true);
        }

        /// <summary>
        /// Gets all component references that match specified locator.
        /// </summary>
        /// <param name="locator">the locator to find a reference by.</param>
        /// <param name="required">forces to raise exception is no reference is found</param>
        /// <returns>a list with matching component references.</returns>
        public virtual List<object> Find(object locator, bool required)
        {
            return Find<object>(locator, required);
        }

        /// <summary>
        /// Gets all component references that match specified locator and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="locator">the locator to find a reference by.</param>
        /// <param name="required">forces to raise exception is no reference is found</param>
        /// <returns>a list with matching component references.</returns>
        public virtual List<T> Find<T>(object locator, bool required)
        {
            if (locator == null)
            {
                throw new ArgumentNullException(nameof(locator));
            }

            var components = new List<T>();

            lock (_lock)
            {
                // Search all references
                for (var index = _references.Count - 1; index >= 0; index--)
                {
                    var reference = _references[index];
                    if (reference.Match(locator))
                    {
                        var component = reference.GetComponent();
                        if (component is T)
                        {
                            components.Add((T)component);
                        }
                    }
                }
            }

            if (components.Count == 0 && required)
            {
                throw new ReferenceException(null, locator);
            }

            return components;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _references.Clear();
            }
        }

        public static References FromTuples(params object[] tuples)
        {
            return new References(tuples);
        }
    }
}