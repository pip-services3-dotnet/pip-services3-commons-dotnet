using System;
using System.Collections.Generic;

using PipServices.Commons.Data;
using PipServices.Commons.Reflect;

namespace PipServices.Commons.Config
{
    /// <summary>
    /**
     * Map with configuration parameters that use complex keys with dot notation and simple string values.
     * 
     * Example of values, stored in the configuration parameters:
     * <ul>
     * <li>Section-1.Subsection-1-1.Key-1-1-1=123</li>
     * <li>Section-1.Subsection-1-2.Key-1-2-1="ABC"</li>
     * <li>Section-2.Subsection-1.Key-2-1-1="2016-09-16T00:00:00.00Z"</li>
     * </ul>
     *  
     * Configuration parameters support getting and adding sections from the map.
     * 
     * Also, configuration parameters may come in a form of parameterized string:
     * Key1=123;Key2=ABC;Key3=2016-09-16T00:00:00.00Z
     * 
     * All keys stored in the map are case-insensitive.
     */
    /// </summary>
    public class ConfigParams : StringValueMap
    {
        /// <summary>
        /// Creates an instance of ConfigParams.
        /// </summary>
        public ConfigParams()
        { }

        /// <summary>
        /// Creates an instance of ConfigParams.
        /// </summary>
        /// <param name="content">Existing map to copy keys/values from.</param>
        public ConfigParams(IDictionary<string, string> content)
            : base(content)
        { }

        public IEnumerable<string> GetSectionNames()
        {
            var sections = new List<string>();

            foreach (var key in Keys)
            {
                var pos = key.IndexOf('.');

                var subKey = (pos > 0) ? key.Substring(0, pos) : key;

                // Perform case sensitive search
                var found = false;
                foreach (var section in sections)
                {
                    if (!section.Equals(subKey, StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    found = true;
                    break;
                }

                if (!found)
                    sections.Add(subKey);
            }

            return sections;
        }

        public ConfigParams GetSection(string section)
        {
            var result = new ConfigParams();
            var prefix = section + ".";

            foreach (var entry in this)
            {
                if (entry.Key.StartsWith(prefix))
                {
                    var key = entry.Key.Substring(prefix.Length);
                    var value = entry.Value;
                    result.Add(key, value);
                }
            }

            return result;
        }

        protected bool IsShadowName(string name)
        {
            return string.IsNullOrWhiteSpace(name) || name.StartsWith("#") || name.StartsWith("!");
        }

        public void AddSection(string section, ConfigParams sectionParams)
        {
            // "Shadow" section names starts with # or !
            section = IsShadowName(section) ? string.Empty : section;

            foreach (var entry in sectionParams)
            {
                // Shadow key names
                var key = IsShadowName(entry.Key) ? string.Empty : entry.Key;

                if (!string.IsNullOrWhiteSpace(section) && !string.IsNullOrWhiteSpace(key))
                {
                    key = section + "." + key;
                }
                else if (string.IsNullOrWhiteSpace(key))
                {
                    key = section;
                }

                this[key] = entry.Value;
            }
        }

        public ConfigParams Override(ConfigParams configParams)
        {
            var map = FromMaps(this, configParams);
            return new ConfigParams(map);
        }


        public ConfigParams SetDefaults(ConfigParams defaultConfigParams)
        {
            var map = FromMaps(defaultConfigParams, this);
            return new ConfigParams(map);
        }

        public new static ConfigParams FromValue(object value)
        {
            var map = RecursiveObjectReader.GetProperties(value);
            var result = new ConfigParams();
            result.Append(map);
            return result;
        }

        public new static ConfigParams FromTuples(params object[] tuples)
        {
            var map = StringValueMap.FromTuples(tuples);
            return new ConfigParams(map);
        }

        public new static ConfigParams FromString(string line)
        {
            var map = StringValueMap.FromString(line);
            return new ConfigParams(map);
        }

        public static ConfigParams MergeConfigs(params IDictionary<string, string>[] configs)
        {
            var map = FromMaps(configs);
            return new ConfigParams(map);
        }
    }
}