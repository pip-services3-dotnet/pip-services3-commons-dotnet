using System;
using System.Collections;
using System.Collections.Generic;

namespace PipServices.Commons.Refer
{
    /// <summary>
    /// Basic implementation of IReferences that stores component as a flat list
    /// </summary>
    public class References : IReferences
    {
        protected readonly List<Reference> _references = new List<Reference>();
        protected readonly object _lock = new object();

        public References() { }

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

        public virtual void Put(object locator, object component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            lock (_lock)
            {
                _references.Add(new Reference(locator, component));
            }
        }

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

        public virtual List<object> GetAllLocators()
        {
            var locators = new List<object>();
            foreach (var reference in _references)
                locators.Add(reference.GetLocator());
            return locators;
        }

        public virtual List<object> GetAll()
        {
            var components = new List<object>();
            foreach (var reference in _references)
                components.Add(reference.GetComponent());
            return components;
        }

        public virtual object GetOneOptional(object locator)
        {
            var components = Find<object>(locator, false);
            return components.Count > 0 ? components[0] : null;
        }

        public virtual T GetOneOptional<T>(object locator)
        {
            var components = Find<T>(locator, false);
            return components.Count > 0 ? components[0] : default(T);
        }

        public virtual object GetOneRequired(object locator)
        {
            var components = Find<object>(locator, true);
            return components.Count > 0 ? components[0] : null;
        }

        public virtual T GetOneRequired<T>(object locator)
        {
            var components = Find<T>(locator, true);
            return components.Count > 0 ? components[0] : default(T);
        }

        public virtual List<object> GetOptional(object locator)
        {
            return Find<object>(locator, false);
        }

        public virtual List<T> GetOptional<T>(object locator)
        {
            return Find<T>(locator, false);
        }

        public virtual List<object> GetRequired(object locator)
        {
            return Find<object>(locator, true);
        }

        public virtual List<T> GetRequired<T>(object locator)
        {
            return Find<T>(locator, true);
        }

        public virtual List<object> Find(object locator, bool required)
        {
            return Find<object>(locator, required);
        }

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