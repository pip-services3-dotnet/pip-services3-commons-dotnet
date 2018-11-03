using PipServices3.Commons.Reflect;
using System;
using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Schema to validate user defined objects.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new ObjectSchema().WithOptionalProperty("id", TypeCode.String)
    ///                                .WithRequiredProperty("name", TypeCode.String);
    ///                                         
    /// schema.Validate({ id: "1", name: "ABC" });       // Result: no errors
    /// schema.Validate({ name: "ABC" });                // Result: no errors
    /// schema.Validate({ id: 1, name: "ABC" });         // Result: id type mismatch
    /// schema.Validate({ id: 1, _name: "ABC" });        // Result: name is missing, unexpected _name
    /// schema.Validate("ABC");                          // Result: type mismatch
    /// </code>
    /// </example>
    public class ObjectSchema : Schema
    {
        public List<PropertySchema> Properties { get; set; }

        public bool IsUndefinedAllowed { get; set; }

        /// <summary>
        /// Sets flag to allow undefined properties.
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <param name="value">true to allow undefined properties and false to disallow.</param>
        /// <returns>this validation schema.</returns>
        public ObjectSchema AllowUndefined(bool value)
        {
            IsUndefinedAllowed = value;
            return this;
        }

        /// <summary>
        /// Adds a validation schema for an object property.
        /// 
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <param name="schema">a property validation schema to be added.</param>
        /// <returns>this validation schema.</returns>
        /// See <see cref="PropertySchema"/>
        public ObjectSchema WithProperty(PropertySchema schema)
        {
            Properties = Properties ?? new List<PropertySchema>();
            Properties.Add(schema);
            return this;
        }

        /// <summary>
        /// Adds a validation schema for a required object property.
        /// </summary>
        /// <param name="name">a property name.</param>
        /// <param name="type">(optional) a property schema or type.</param>
        /// <param name="rules">(optional) a list of property validation rules.</param>
        /// <returns>the validation schema</returns>
        public ObjectSchema WithRequiredProperty(string name, object type, params IValidationRule[] rules)
        {
            Properties = Properties ?? new List<PropertySchema>();
            var schema = new PropertySchema(name, type)
            {
                Rules = new List<IValidationRule>(rules)
            };
            schema.MakeRequired();
            return WithProperty(schema);
        }

        /// <summary>
        /// Adds a validation schema for an optional object property.
        /// </summary>
        /// <param name="name">a property name.</param>
        /// <param name="type">(optional) a property schema or type.</param>
        /// <param name="rules">(optional) a list of property validation rules.</param>
        /// <returns>the validation schema</returns>
        public ObjectSchema WithOptionalProperty(string name, object type, params IValidationRule[] rules)
        {
            Properties = Properties ?? new List<PropertySchema>();
            var schema = new PropertySchema(name, type)
            {
                Rules = new List<IValidationRule>(rules)
            };
            schema.MakeOptional();
            return WithProperty(schema);
        }

        /// <summary>
        /// Validates a given value against the schema and configured validation rules.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        protected internal override void PerformValidation(string path, object value, List<ValidationResult> results)
        {
            base.PerformValidation(path, value, results);

            if (value == null) return;

            var name = path ?? "value";
            var properties = ObjectReader.GetProperties(value);

            // Process defined properties
            if (Properties != null)
            {
                foreach (var propertySchema in Properties)
                {
                    string processedName = null;

                    foreach (var entry in properties)
                    {
                        string propertyName = entry.Key;
                        object propertyValue = entry.Value;
                        // Find properties case insensitive
                        if (propertyName.Equals(propertySchema.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            propertySchema.PerformValidation(path, propertyValue, results);
                            processedName = propertyName;
                            break;
                        }
                    }

                    if (processedName == null)
                        propertySchema.PerformValidation(path, null, results);
                    else
                        properties.Remove(processedName);
                }
            }

            // Process unexpected properties
            foreach (var entry in properties)
            {
                string propertyPath = string.IsNullOrWhiteSpace(path)
                    ? entry.Key : path + "." + entry.Key;

                results.Add(
                    new ValidationResult(
                        propertyPath,
                        ValidationResultType.Warning,
                        "UNEXPECTED_PROPERTY",
                        name + " contains unexpected property " + entry.Key,
                        null,
                        entry.Key
                    )
                );
            }
        }
    }
}
