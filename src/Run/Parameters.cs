using System;
using System.Collections;
using PipServices.Commons.Data;
using PipServices.Commons.Convert;
using PipServices.Commons.Reflect;
using PipServices.Commons.Config;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Parameters represent hierarchical map that uses simple keys and stores complex objects.
    /// It allows hierarchical access to stored data using dot-notation.
    /// 
    /// All keys stored in the map are case-insensitive.
    /// </summary>
    public class Parameters : AnyValueMap
    {
        public Parameters() { }

        public Parameters(IDictionary values) 
            : base(values)
        { }

        public override object Get(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (path.IndexOf(".", StringComparison.Ordinal) > 0)
                return RecursiveObjectReader.GetProperty(this, path);
            else
                return base.Get(path);
        }

        public override void Set(string path, object value)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            if (path.IndexOf(".", StringComparison.Ordinal) > 0)
                RecursiveObjectWriter.SetProperty(this, path, value);
            else
                base.Set(path, value);
        }

        public Parameters GetAsNullableParameters(string key)
        {
            var value = GetAsNullableMap(key);
            return value != null ? new Parameters(value) : null;
        }

        public Parameters GetAsParameters(string key)
        {
            var value = GetAsMap(key);
            return new Parameters(value);
        }

        public Parameters GetAsParametersWithDefault(string key, Parameters defaultValue)
        {
            var result = GetAsNullableParameters(key);
            return result ?? defaultValue;
        }

        public new bool ContainsKey(string key)
        {
            return RecursiveObjectReader.HasProperty(this, key);
        }

        public Parameters Override(Parameters parameters)
        {
            return Override(parameters, false);
        }

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

        public Parameters SetDefaults(Parameters defaultParameters)
        {
            return SetDefaults(defaultParameters, false);
        }

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

        public void AssignTo(object value)
        {
            if (value == null || Count == 0)
                return;

            RecursiveObjectWriter.CopyProperties(value, this);
        }

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

        public Parameters Omit(params string[] paths)
        {
            var result = new Parameters();
            foreach (var path in paths)
            {
                result.Remove(path);
            }
            return result;
        }

        public string ToJson()
        {
            return JsonConverter.ToJson(this);
        }

        public new static Parameters FromTuples(params object[] tuples)
        {
            return new Parameters(AnyValueMap.FromTuples(tuples));
        }

        public static Parameters MergeParams(params Parameters[] parameters)
        {
            return new Parameters(FromMaps(parameters));
        }

        public static Parameters FromJson(string json)
        {
            var map = JsonConverter.ToNullableMap(json);
            return map != null ? new Parameters((IDictionary)map): new Parameters();
        }

        public static Parameters FromConfig(ConfigParams config)
        {
            return config != null ? new Parameters(config) : new Parameters();
        }
    }
}