using PipServices3.Commons.Reflect;
using System.Collections;
using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Schema to validate maps.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new MapSchema(TypeCode.String, TypeCode.Integer);
    /// 
    /// schema.Validate({ "key1": "A", "key2": "B" });       // Result: no errors
    /// schema.Validate({ "key1": 1, "key2": 2 });           // Result: element type mismatch
    /// schema.Validate(new int[]{ 1, 2, 3 });                        // Result: type mismatch
    /// </code>
    /// </example>
    public class MapSchema : Schema
    {
        /// <summary>
        /// Creates a new instance of validation schema.
        /// </summary>
        public MapSchema() { }

        /// <summary>
        /// Creates a new instance of validation schema and sets its values.
        /// </summary>
        /// <param name="keyType">a type of map keys. Null means that keys may have any type.</param>
        /// <param name="valueType">a type of map values. Null means that values may have any type.</param>
        public MapSchema(object keyType, object valueType)
        {
            KeyType = keyType;
            ValueType = valueType;
        }

        public object KeyType { get; set; }
        public object ValueType { get; set; }

        /// <summary>
        /// Validates a given value against the schema and configured validation rules.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        protected internal override void PerformValidation(string path, object value, List<ValidationResult> results)
        {
            var name = path ?? "value";
            value = ObjectReader.GetValue(value);

            base.PerformValidation(path, value, results);

            if (value == null) return;

            var map = value as IDictionary;
            if (map != null)
            {
                foreach (var key in map.Keys)
                {
                    var elementPath = string.IsNullOrWhiteSpace(path)
                        ? key.ToString() : path + "." + key;

                    PerformTypeValidation(elementPath, KeyType, key, results);
                    PerformTypeValidation(elementPath, ValueType, map[key], results);
                }
            }
            else
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "VALUE_ISNOT_MAP",
                        name + " type must to be Map (Dictionary)",
                        typeof(IDictionary),
                        value.GetType()
                    )
                );
            }
        }
    }
}
