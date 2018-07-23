using PipServices.Commons.Config;
using PipServices.Commons.Convert;
using System;
using System.Collections.Generic;

namespace PipServices.Commons.Refer
{
    public class DependencyResolver: IReferenceable, IReconfigurable {
        private Dictionary<string, object> _dependencies = new Dictionary<string, object>();
        private IReferences _references;

        public DependencyResolver() { }

        public DependencyResolver(ConfigParams config)
        {
            Configure(config);
        }

        public void Configure(ConfigParams config)
        {
            ConfigParams dependencies = config.GetSection("dependencies");
	        foreach (string name in dependencies.Keys) {
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

        public void SetReferences(IReferences references)
        {
            _references = references;
        }

        public void Put(string name, object locator)
        {
            _dependencies[name] = locator;
        }

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

        public List<object> GetOptional(string name)
        {
            object locator = Locate(name);
            return locator != null ? _references.GetOptional(locator) : null;
        }

        public List<T> GetOptional<T>(string name)
        {
            object locator = Locate(name);
            return locator != null ? _references.GetOptional<T>(locator) : null;
        }

        public List<object> GetRequired(string name)
        {
            object locator = Locate(name);
	        if (locator == null)
		        throw new ReferenceException(null, name);
		
	        return _references.GetRequired(locator);
        }

        public List<T> GetRequired<T>(string name)
        {
            object locator = Locate(name);
	        if (locator == null)
		        throw new ReferenceException(null, name);
		
	        return _references.GetRequired<T>(locator);
        }

        public object GetOneOptional(string name)
        {
            object locator = Locate(name);
            return locator != null ? _references.GetOneOptional(locator) : null;
        }

        public T GetOneOptional<T>(string name)
        {
            object locator = Locate(name);
            return locator != null ? _references.GetOneOptional<T>(locator) : default(T);
        }

        public object GetOneRequired(string name)
        {
            object locator = Locate(name);
	        if (locator == null)
		        throw new ReferenceException(null, name);
		
	        return _references.GetOneRequired(locator);
        }

        public T GetOneRequired<T>(string name)
        {
            object locator = Locate(name);
	        if (locator == null)
		        throw new ReferenceException(null, name);
		
	        return _references.GetOneRequired<T>(locator);
        }

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
