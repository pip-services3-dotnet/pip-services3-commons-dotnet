using System;
using System.Collections;
using PipServices3.Commons.Data;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Reflect;
using PipServices3.Commons.Config;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Contains map with execution parameters.
    /// 
    /// In general, this map may contain non-serializable values.
    /// And in contrast with other maps, its getters and setters
    /// support dot notation and able to access properties in the entire object graph.
    /// 
    /// This class is often use to pass execution and notification
    /// arguments, and parameterize classes before execution.
    /// </summary>
    /// See <see cref="IParameterized"/>, <see cref="AnyValueMap"/>
    public class Parameters : AnyValueMap
    {
        /// <summary>
        /// Creates a new instance of the map.
        /// </summary>
        public Parameters() { }

        /// <summary>
        /// Creates a new instance of the map and assigns its value.
        /// </summary>
        /// <param name="values">(optional) values to initialize this map.</param>
        public Parameters(IDictionary values) 
            : base(values)
        {
        }

        /// <summary>
        /// Gets a map element specified by its key.
        /// 
        /// The key can be defined using dot notation and allows to recursively access
        /// elements of elements.
        /// </summary>
        /// <param name="path">a key of the element to get.</param>
        /// <returns>the value of the map element.</returns>
        public override object Get(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (path.IndexOf(".", StringComparison.Ordinal) > 0)
                return RecursiveObjectReader.GetProperty(this, path);
            else
                return base.Get(path);
        }

        /// <summary>
        /// Sets a new value into map element specified by its key.
        /// 
        /// The key can be defined using dot notation and allows to recursively access
        /// elements of elements.
        /// </summary>
        /// <param name="path">a path of the element to set.</param>
        /// <param name="value">a new value for map element.</param>
        public override void Set(string path, object value)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            if (path.IndexOf(".", StringComparison.Ordinal) > 0)
                RecursiveObjectWriter.SetProperty(this, path, value);
            else
                base.Set(path, value);
        }

        /// <summary>
        /// Converts map element into an Parameters or returns null if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>Parameters value of the element or null if conversion is not supported.</returns>
        public Parameters GetAsNullableParameters(string key)
        {
            var value = GetAsNullableMap(key);
            return value != null ? new Parameters(value) : null;
        }

        /// <summary>
        /// Converts map element into an Parameters or returns empty Parameters if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <returns>Parameters value of the element or empty Parameters if conversion is not supported.</returns>
        public Parameters GetAsParameters(string key)
        {
            var value = GetAsMap(key);
            return new Parameters(value);
        }

        /// <summary>
        /// Converts map element into an Parameters or returns default value if conversion is not possible.
        /// </summary>
        /// <param name="key">a key of element to get.</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>Parameters value of the element or default value if conversion is not supported.</returns>
        public Parameters GetAsParametersWithDefault(string key, Parameters defaultValue)
        {
            var result = GetAsNullableParameters(key);
            return result ?? defaultValue;
        }

        /// <summary>
        /// Checks if this map contains an element with specified key.
        /// 
        /// The key can be defined using dot notation and allows to recursively access 
        /// elements of elements.
        /// </summary>
        /// <param name="key">a key to be checked</param>
        /// <returns>true if this map contains the key or false otherwise.</returns>
        public new bool ContainsKey(string key)
        {
            return RecursiveObjectReader.HasProperty(this, key);
        }

        /// <summary>
        /// Overrides parameters with new values from specified Parameters and returns a
        /// new Parameters object.
        /// </summary>
        /// <param name="parameters">Parameters with parameters to override the current values.</param>
        /// <returns>a new Parameters object.</returns>
        public Parameters Override(Parameters parameters)
        {
            return Override(parameters, false);
        }

        /// <summary>
        /// Overrides parameters with new values from specified Parameters and returns a
        /// new Parameters object.
        /// </summary>
        /// <param name="parameters">Parameters with parameters to override the current values.</param>
        /// <param name="recursive">(optional) true to perform deep copy, and false for shallow copy.Default: false</param>
        /// <returns>a new Parameters object.</returns>
        public Parameters Override(Parameters parameters, bool recursive)
        {
            var result = new Parameters();

            if (recursive)
            {
                RecursiveObjectWriter.CopyProperties(result, this);
                RecursiveObjectWriter.CopyProperties(result, parameters);
            }
            else
            {
                ObjectWriter.SetProperties(result, this);
                ObjectWriter.SetProperties(result, parameters);
            }

            return result;
        }

        /// <summary>
        /// Set default values from specified Parameters and returns a new Parameters object.
        /// </summary>
        /// <param name="defaultParameters">Parameters with default parameter values.</param>
        /// <returns>a new Parameters object.</returns>
        public Parameters SetDefaults(Parameters defaultParameters)
        {
            return SetDefaults(defaultParameters, false);
        }

        /// <summary>
        /// Set default values from specified Parameters and returns a new Parameters object.
        /// </summary>
        /// <param name="defaultParameters">Parameters with default parameter values.</param>
        /// <param name="recursive">(optional) true to perform deep copy, and false for shallow copy.Default: false</param>
        /// <returns>a new Parameters object.</returns>
        public Parameters SetDefaults(Parameters defaultParameters, bool recursive)
        {
            var result = new Parameters();

            if (recursive)
            {
                RecursiveObjectWriter.CopyProperties(result, defaultParameters);
                RecursiveObjectWriter.CopyProperties(result, this);
            }
            else
            {
                ObjectWriter.SetProperties(result, defaultParameters);
                ObjectWriter.SetProperties(result, this);
            }

            return result;
        }

        /// <summary>
        /// Assigns (copies over) properties from the specified value to this map.
        /// </summary>
        /// <param name="value">value whose properties shall be copied over.</param>
        /// See <see cref="RecursiveObjectWriter.CopyProperties(object, object)"/>
        public void AssignTo(object value)
        {
            if (value == null || Count == 0)
                return;

            RecursiveObjectWriter.CopyProperties(value, this);
        }

        /// <summary>
        /// Picks select parameters from this Parameters and returns them as a new Parameters object.
        /// </summary>
        /// <param name="paths">keys to be picked and copied over to new Parameters.</param>
        /// <returns>a new Parameters object.</returns>
        public Parameters Pick(params string[] paths)
        {
            var result = new Parameters();
            foreach(var path in paths)
            {
                if (ContainsKey(path))
                {
                    result[path] = Get(path);
                }
            }
            return result;
        }

        /// <summary>
        /// Omits selected parameters from this Parameters and returns the rest as a new Parameters object.
        /// </summary>
        /// <param name="paths">keys to be omitted from copying over to new Parameters.</param>
        /// <returns>a new Parameters object.</returns>
        public Parameters Omit(params string[] paths)
        {
            var result = new Parameters();
            foreach (var path in paths)
            {
                result.Remove(path);
            }
            return result;
        }

        /// <summary>
        /// Converts this map to JSON object.
        /// </summary>
        /// <returns>a JSON representation of this map.</returns>
        /// See <see cref="JsonConverter.ToJson(object)"/>
        public string ToJson()
        {
            return JsonConverter.ToJson(this);
        }

        /// <summary>
        /// Creates a new Parameters object filled with provided key-value pairs called
        /// tuples.Tuples parameters contain a sequence of key1, value1, key2, value2, ... pairs.
        /// </summary>
        /// <param name="tuples">the tuples to fill a new Parameters object.</param>
        /// <returns>a new Parameters object.</returns>
        /// See <see cref="AnyValueMap.FromTuples(object[])"/>
        public new static Parameters FromTuples(params object[] tuples)
        {
            return new Parameters(AnyValueMap.FromTuples(tuples));
        }

        /// <summary>
        /// Merges two or more Parameters into one. The following Parameters override
        /// previously defined parameters.
        /// </summary>
        /// <param name="parameters">a list of Parameters objects to be merged.</param>
        /// <returns>a new Parameters object.</returns>
        public static Parameters MergeParams(params Parameters[] parameters)
        {
            return new Parameters(FromMaps(parameters));
        }

        /// <summary>
        /// Creates new Parameters from JSON object.
        /// </summary>
        /// <param name="json">a JSON string containing parameters.</param>
        /// <returns>a new Parameters object.</returns>
        /// See <see cref="JsonConverter.ToNullableMap(string)"/>
        public static Parameters FromJson(string json)
        {
            var map = JsonConverter.ToNullableMap(json);
            return map != null ? new Parameters((IDictionary)map): new Parameters();
        }

        /// <summary>
        /// Creates new Parameters from ConfigMap object.
        /// </summary>
        /// <param name="config">a ConfigParams that contain parameters.</param>
        /// <returns>a new Parameters object.</returns>
        /// See <see cref="ConfigParams"/>
        public static Parameters FromConfig(ConfigParams config)
        {
            return config != null ? new Parameters(config) : new Parameters();
        }
    }
}