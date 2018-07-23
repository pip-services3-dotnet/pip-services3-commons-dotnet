using PipServices.Commons.Reflect;
using System;
using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public class ObjectSchema : Schema
    {
        public List<PropertySchema> Properties { get; set; }

        public bool IsUndefinedAllowed { get; set; }

        public ObjectSchema AllowUndefined(bool value)
        {
            IsUndefinedAllowed = value;
            return this;
        }

        public ObjectSchema WithProperty(PropertySchema schema)
        {
            Properties = Properties ?? new List<PropertySchema>();
            Properties.Add(schema);
            return this;
        }

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
