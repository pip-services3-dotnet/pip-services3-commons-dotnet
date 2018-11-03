using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using System;
using System.Collections.Generic;

namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// Helper class for resolving component dependencies.
    /// 
    /// The resolver is configured to resolve named dependencies by specific locator.
    /// During deployment the dependency locator can be changed.
    /// 
    /// This mechanism can be used to clarify specific dependency among several alternatives.
    /// Typically components are configured to retrieve the first dependency that matches
    /// logical group, type and version.But if container contains more than one instance
    /// and resolution has to be specific about those instances, they can be given a unique
    /// name and dependency resolvers can be reconfigured to retrieve dependencies by their name.
    /// 
    /// Configuration parameters:
    /// 
    /// dependencies:
    /// - [dependency name 1]: [dependency 1 locator (descriptor)]
    /// - ...
    /// - [dependency name N]: [dependency N locator (descriptor)]
    /// 
    /// References:
    /// - [references that match configured dependencies]
    /// </summary>
    /// <example>
    /// <code>
    /// class MyComponent: IConfigurable, IReferenceable 
    /// {
    ///     private DependencyResolver _dependencyResolver = new DependencyResolver();
    ///     private IMyPersistence _persistence;
    ///     ...
    ///     
    ///     public MyComponent()
    ///     {
    ///         this._dependencyResolver.Put("persistence", new Descriptor("mygroup", "persistence", "*", "*", "1.0"));
    ///     }
    ///     
    ///     public void Configure(ConfigParams config)
    ///     {
    ///         this._dependencyResolver.Configure(config);
    ///     }
    ///     
    ///     public void SetReferences(IReferences references)
    ///     {
    ///         this._dependencyResolver.SetReferences(references);
    ///         this._persistence = this._dependencyResolver.GetOneRequired<IMyPersistence>("persistence");
    ///     }
    /// }
    /// 
    /// // Create mycomponent and set specific dependency out of many
    /// var component = new MyComponent();
    /// component.Configure(ConfigParams.fromTuples(
    /// "dependencies.persistence", "mygroup:persistence:*:persistence2:1.0" // Override default persistence dependency
    /// ));
    /// component.SetReferences(References.fromTuples(
    /// new Descriptor("mygroup","persistence","*","persistence1","1.0"), new MyPersistence(),
    /// new Descriptor("mygroup","persistence","*","persistence2","1.0"), new MyPersistence()  // This dependency shall be set
    /// ));
    /// </code>
    /// </example>
    /// See <see cref="IReferences"/>
    public class DependencyResolver : IReferenceable, IReconfigurable
    {
        private Dictionary<string, object> _dependencies = new Dictionary<string, object>();
        private IReferences _references;

        /// <summary>
        /// Creates a new instance of the dependency resolver.
        /// </summary>
        public DependencyResolver() { }

        /// <summary>
        /// Creates a new instance of the dependency resolver.
        /// </summary>
        /// <param name="config">(optional) default configuration where key is dependency name
        /// and value is locator(descriptor)</param>
        /// See <see cref="ConfigParams"/>, <see cref="Configure(ConfigParams)"/>, <see cref="IReferences"/>, <see cref="SetReferences(IReferences)"/>
        public DependencyResolver(ConfigParams config)
        {
            Configure(config);
        }

        /// <summary>
        /// Configures the component with specified parameters.
        /// </summary>
        /// <param name="config">configuration parameters to set.</param>
        /// See <see cref="ConfigParams"/>
        public void Configure(ConfigParams config)
        {
            ConfigParams dependencies = config.GetSection("dependencies");
            foreach (string name in dependencies.Keys)
            {
                string locator = dependencies.Get(name);
                if (locator == null) continue;

                try
                {
                    Descriptor descriptor = Descriptor.FromString(locator);
                    if (descriptor != null)
                        _dependencies[name] = descriptor;
                    else
                        _dependencies[name] = locator;
                }
                catch (Exception)
                {
                    _dependencies[name] = locator;
                }
            }
        }

        /// <summary>
        /// Sets the component references
        /// </summary>
        /// <param name="references">references to set.</param>
        public void SetReferences(IReferences references)
        {
            _references = references;
        }

        /// <summary>
        /// Adds a new dependency into this resolver.
        /// </summary>
        /// <param name="name">the dependency's name.</param>
        /// <param name="locator">the locator to find the dependency by.</param>
        public void Put(string name, object locator)
        {
            _dependencies[name] = locator;
        }

        /// <summary>
        /// Gets a dependency locator by its name.
        /// </summary>
        /// <param name="name">the name of the dependency to locate.</param>
        /// <returns>the dependency locator or null if locator was not configured.</returns>
        private object Locate(string name)
        {
            if (name == null)
                throw new NullReferenceException("Dependency name cannot be null");
            if (_references == null)
                throw new NullReferenceException("References shall be set");

            object locator = null;
            _dependencies.TryGetValue(name, out locator);
            return locator;
        }

        /// <summary>
        /// Gets all optional dependencies by their name.
        /// </summary>
        /// <param name="name">the dependency name to locate.</param>
        /// <returns>a list with found dependencies or empty list of no dependencies was found.</returns>
        public List<object> GetOptional(string name)
        {
            object locator = Locate(name);
            return locator != null ? _references.GetOptional(locator) : null;
        }

        /// <summary>
        /// Gets all optional dependencies by their name.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="name">the dependency name to locate.</param>
        /// <returns>a list with found dependencies or empty list of no dependencies was found.</returns>
        public List<T> GetOptional<T>(string name)
        {
            object locator = Locate(name);
            return locator != null ? _references.GetOptional<T>(locator) : null;
        }

        /// <summary>
        /// Gets all required dependencies by their name. At least one dependency must 
        /// present.If no dependencies was found it throws a ReferenceException
        /// </summary>
        /// <param name="name">the dependency name to locate.</param>
        /// <returns>a list with found dependencies.</returns>
        public List<object> GetRequired(string name)
        {
            object locator = Locate(name);
            if (locator == null)
                throw new ReferenceException(null, name);

            return _references.GetRequired(locator);
        }

        /// <summary>
        /// Gets all required dependencies by their name. At least one dependency must
        /// present.If no dependencies was found it throws a ReferenceException
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="name">the dependency name to locate.</param>
        /// <returns>a list with found dependencies.</returns>
        public List<T> GetRequired<T>(string name)
        {
            object locator = Locate(name);
            if (locator == null)
                throw new ReferenceException(null, name);

            return _references.GetRequired<T>(locator);
        }

        /// <summary>
        /// Gets one optional dependency by its name.
        /// </summary>
        /// <param name="name">the dependency name to locate.</param>
        /// <returns>a dependency reference or null of the dependency was not found</returns>
        public object GetOneOptional(string name)
        {
            object locator = Locate(name);
            return locator != null ? _references.GetOneOptional(locator) : null;
        }

        /// <summary>
        /// Gets one optional dependency by its name and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="name">the dependency name to locate.</param>
        /// <returns>a dependency reference or null of the dependency was not found</returns>
        public T GetOneOptional<T>(string name)
        {
            object locator = Locate(name);
            return locator != null ? _references.GetOneOptional<T>(locator) : default(T);
        }

        /// <summary>
        /// Gets one required dependency by its name. At least one dependency must
        /// present.If the dependency was found it throws a ReferenceException
        /// </summary>
        /// <param name="name">the dependency name to locate.</param>
        /// <returns>a dependency reference</returns>
        public object GetOneRequired(string name)
        {
            object locator = Locate(name);
            if (locator == null)
                throw new ReferenceException(null, name);

            return _references.GetOneRequired(locator);
        }

        /// <summary>
        /// Gets one required dependency by its name and matching to the specified type.
        /// At least one dependency must present.If the dependency was found it throws a ReferenceException
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="name">the dependency name to locate.</param>
        /// <returns>a dependency reference</returns>
        public T GetOneRequired<T>(string name)
        {
            object locator = Locate(name);
            if (locator == null)
                throw new ReferenceException(null, name);

            return _references.GetOneRequired<T>(locator);
        }

        /// <summary>
        /// Finds all matching dependencies by their name.
        /// </summary>
        /// <param name="name">the dependency name to locate.</param>
        /// <param name="required">true to raise an exception when no dependencies are found.</param>
        /// <returns>a list of found dependencies</returns>
        public List<object> Find(string name, bool required)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            object locator = Locate(name);
            if (locator == null)
            {
                if (required)
                {
                    throw new ReferenceException(null, name);
                }

                return null;
            }

            return _references.Find(locator, required);
        }

        /// <summary>
        /// Finds all matching dependencies by their name and matching to the specified type.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="name">the dependency name to locate.</param>
        /// <param name="required">true to raise an exception when no dependencies are found.</param>
        /// <returns>a list of found dependencies</returns>
        public List<T> Find<T>(string name, bool required)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            object locator = Locate(name);
            if (locator == null)
            {
                if (required)
                {
                    throw new ReferenceException(null, name);
                }

                return null;
            }

            return _references.Find<T>(locator, required);
        }

        /// <summary>
        /// Creates a new DependencyResolver from a list of key-value pairs called tuples
        /// where key is dependency name and value the depedency locator(descriptor).
        /// </summary>
        /// <param name="tuples">a list of values where odd elements are dependency name and the
        /// following even elements are dependency locator(descriptor)</param>
        /// <returns>a newly created DependencyResolver.</returns>
        public static DependencyResolver FromTuples(params object[] tuples)
        {
            DependencyResolver result = new DependencyResolver();
            if (tuples == null || tuples.Length == 0)
                return result;

            for (int index = 0; index < tuples.Length; index += 2)
            {
                if (index + 1 >= tuples.Length) break;

                string name = StringConverter.ToString(tuples[index]);
                object locator = tuples[index + 1];

                result.Put(name, locator);
            }

            return result;
        }
    }
}
