using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Schema to validate object properties
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new ObjectSchema().WithProperty(new PropertySchema("id", TypeCode.String));
    /// 
    /// schema.Validate({ id: "1", name: "ABC" });       // Result: no errors
    /// schema.Validate({ name: "ABC" });                // Result: no errors
    /// schema.Validate({ id: 1, name: "ABC" });         // Result: id type mismatch
    /// </code>
    /// </example>
    public class PropertySchema : Schema
    {
        /// <summary>
        /// Creates a new validation schema.
        /// </summary>
        public PropertySchema() { }

        /// <summary>
        /// Creates a new validation schema and sets its values.
        /// </summary>
        /// <param name="name">(optional) a property name</param>
        /// <param name="type">(optional) a property type</param>
        public PropertySchema(string name, object type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public object Type { get; set; }

        /// <summary>
        /// Validates a given value against the schema and configured validation rules.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        protected internal override void PerformValidation(string path, object value, List<ValidationResult> results)
        {
            path = string.IsNullOrWhiteSpace(path) ? Name : path + "." + Name;

            base.PerformValidation(path, value, results);
            PerformTypeValidation(path, Type, value, results);
        }
    }
}