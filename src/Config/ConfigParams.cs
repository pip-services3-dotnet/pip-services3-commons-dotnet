using System;
using System.Collections.Generic;

using PipServices3.Commons.Data;
using PipServices3.Commons.Reflect;

namespace PipServices3.Commons.Config
{
    /// <summary>
    ///Contains a key-value map with configuration parameters.
    /// All values stored as strings and can be serialized as JSON or string forms.
    /// When retrieved the values can be automatically converted on read using GetAsXXX methods.
    /// 
    /// The keys are case-sensitive, so it is recommended to use consistent C-style as: <c>"my_param"</c>
    /// 
    /// Configuration parameters can be broken into sections and subsections using dot notation as:
    /// <c>"section1.subsection1.param1"</c>. Using GetSection method all parameters from specified section
    /// can be extracted from a ConfigMap.
    /// 
    /// The ConfigParams supports serialization from/to plain strings as:
    /// <c>"key1=123;key2=ABC;key3=2016-09-16T00:00:00.00Z"</c>
    /// ConfigParams are used to pass configurations to IConfigurable objects. 
    /// They also serve as a basis for more concrete configurations such as <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-components-dotnet/master/doc/api/class_pip_services_1_1_components_1_1_connect_1_1_connection_params.html">ConnectionParams</a>
    /// or <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-components-dotnet/master/doc/api/class_pip_services_1_1_components_1_1_auth_1_1_credential_params.html">CredentialParams</a>.
    /// </summary>
    /// <example>
    /// <code>
    /// var config = ConfigParams.fromTuples( "section1.key1", "AAA",
    /// "section1.key2", 123,
    /// "section2.key1", true );
    /// 
    /// config.GetAsString("section1.key1"); // Result: AAA
    /// config.GetAsInteger("section1.key1"); // Result: 0
    /// 
    /// var section1 = config.GetSection("section1");
    /// section1.ToString(); // Result: key1=AAA;key2=123
    /// </code>
    /// </example>
    /// See <see cref="IConfigurable"/>, <see cref="StringValueMap"/>
    public class ConfigParams : StringValueMap
    {
        /// <summary>
        /// Creates an instance of ConfigParams.
        /// </summary>
        public ConfigParams()
        { }

        /// <summary>
        /// Creates a new ConfigParams and fills it with values.
        /// </summary>
        /// <param name="content">(optional) an object to be converted into key-value pairs to initialize this config map.</param>
        /// See <see cref="StringValueMap"/>
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

        /// <summary>
        /// Gets parameters from specific section stored in this ConfigMap. The section name is removed from parameter keys.
        /// </summary>
        /// <param name="section">name of the section to retrieve configuration parameters from.</param>
        /// <returns>all configuration parameters that belong to the section named 'section'.</returns>
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

        /// <summary>
        /// Adds parameters into this ConfigParams under specified section. Keys for the new parameters are appended with section dot prefix.
        /// </summary>
        /// <param name="section">name of the section where add new parameters</param>
        /// <param name="sectionParams">new parameters to be added.</param>        
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

        /// <summary>
        /// Overrides parameters with new values from specified ConfigParams and returns a new ConfigParams object.
        /// </summary>
        /// <param name="configParams">ConfigMap with parameters to override the current values.</param>
        /// <returns>a new ConfigParams object.</returns>
        /// See <see cref="ConfigParams.SetDefaults(ConfigParams)"/>
        public ConfigParams Override(ConfigParams configParams)
        {
            var map = FromMaps(this, configParams);
            return new ConfigParams(map);
        }

        /// <summary>
        /// Set default values from specified ConfigParams and returns a new ConfigParams object.
        /// </summary>
        /// <param name="defaultConfigParams">ConfigMap with default parameter values.</param>
        /// <returns>a new ConfigParams object.</returns>
        /// See <see cref="ConfigParams.Override(ConfigParams)"/>
        public ConfigParams SetDefaults(ConfigParams defaultConfigParams)
        {
            var map = FromMaps(defaultConfigParams, this);
            return new ConfigParams(map);
        }

        /// <summary>
        /// Creates a new ConfigParams object filled with key-value pairs from specified object.
        /// </summary>
        /// <param name="value">an object with key-value pairs used to initialize a new ConfigParams.</param>
        /// <returns>a new ConfigParams object.</returns>
        /// See <see cref="RecursiveObjectReader.GetProperties(object)"/>
        public new static ConfigParams FromValue(object value)
        {
            var map = RecursiveObjectReader.GetProperties(value);
            var result = new ConfigParams();
            result.Append(map);
            return result;
        }

        /// <summary>
        /// Creates a new ConfigParams object filled with provided key-value pairs called tuples.
        /// Tuples parameters contain a sequence of key1, value1, key2, value2, ... pairs
        /// </summary>
        /// <param name="tuples">the tuples to fill a new ConfigParams object.</param>
        /// <returns>a new ConfigParams object.</returns>
        /// See <see cref="StringValueMap.FromTuples(object[])"/>
        public new static ConfigParams FromTuples(params object[] tuples)
        {
            var map = StringValueMap.FromTuples(tuples);
            return new ConfigParams(map);
        }

        /// <summary>
        /// Creates a new ConfigParams object filled with key-value pairs serialized as a string.
        /// </summary>
        /// <param name="line">a string with serialized key-value pairs as
        /// "key1=value1;key2=value2;..." 
        /// Example: "Key1=123;Key2=ABC;Key3=2016-09-16T00:00:00.00Z"</param>
        /// <returns>a new ConfigParams object.</returns>
        /// See <see cref="StringValueMap.FromString(string)"/>
        public new static ConfigParams FromString(string line)
        {
            var map = StringValueMap.FromString(line);
            return new ConfigParams(map);
        }

        /// <summary>
        /// Merges two or more ConfigParams into one. The following ConfigParams override previously defined parameters.
        /// </summary>
        /// <param name="configs">a list of ConfigParams objects to be merged.</param>
        /// <returns>a new ConfigParams object.</returns>
        /// See <see cref="StringValueMap.FromMaps(IDictionary{string, string}[])"/>
        public static ConfigParams MergeConfigs(params IDictionary<string, string>[] configs)
        {
            var map = FromMaps(configs);
            return new ConfigParams(map);
        }
    }
}