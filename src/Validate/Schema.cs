using System.Linq;
using System.Collections.Generic;
using PipServices3.Commons.Reflect;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Basic schema that validates values against a set of validation rules.
    /// 
    /// This schema is used as a basis for specific schemas to validate
    /// objects, project properties, arrays and maps.
    /// </summary>
    /// See <see cref="ObjectSchema"/>, <see cref="PropertySchema"/>, <see cref="ArraySchema"/>, <see cref="MapSchema"/>
    public class Schema
    {
        /// <summary>
        /// Creates a new instance of validation schema.
        /// </summary>
        public Schema() { }

        /// <summary>
        /// Creates a new instance of validation schema and sets its values.
        /// </summary>
        /// <param name="required">(optional) true to always require non-null values.</param>
        /// <param name="rules">(optional) a list with validation rules.</param>
        public Schema(bool required, List<IValidationRule> rules)
        {
            IsRequired = required;
            Rules = rules;
        }

        public bool IsRequired { get; set; }

        public List<IValidationRule> Rules { get; set; }

        /// <summary>
        /// Makes validated values always required (non-null). For null values the schema will raise errors.
        /// 
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <returns>this validation schema</returns>
        public Schema MakeRequired()
        {
            IsRequired = true;
            return this;
        }

        /// <summary>
        /// Makes validated values optional. Validation for null values will be skipped.
        /// 
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <returns>this validation schema</returns>
        public Schema MakeOptional()
        {
            IsRequired = false;
            return this;
        }

        /// <summary>
        /// Adds validation rule to this schema.
        /// This method returns reference to this exception to implement Builder pattern
        /// to chain additional calls.
        /// </summary>
        /// <param name="rule">a validation rule to be added.</param>
        /// <returns>this validation schema.</returns>
        public Schema WithRule(IValidationRule rule)
        {
            Rules = Rules ?? new List<IValidationRule>();
            Rules.Add(rule);
            return this;
        }

        /// <summary>
        /// Validates a given value against the schema and configured validation rules.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        protected internal virtual void PerformValidation(string path, object value, List<ValidationResult> results)
        {
            var name = path ?? "value";

            if (value == null)
            {
                // Check for required values
                if (IsRequired)
                    results.Add(
                        new ValidationResult(
                            path,
                            ValidationResultType.Error,
                            "VALUE_IS_NULL",
                            name + " cannot be null",
                            "NOT NULL",
                            null
                        )
                    );
            }
            else
            {
                value = ObjectReader.GetValue(value);

                // Check validation rules
                if (Rules != null)
                {
                    foreach (var rule in Rules)
                        rule.Validate(path, this, value, results);
                }
            }
        }

        /// <summary>
        /// Validates a given value to match specified type. The type can be defined as a
        /// Schema, type, a type name or TypeCode When type is a Schema, it executes
        /// validation recursively against that Schema.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="type">a type to match the value type</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        protected void PerformTypeValidation(string path, object type, object value, List<ValidationResult> results)
        {
            // If type it not defined then skip
            if (type == null) return;

            // Perform validation against schema
            var schema = type as Schema;
            if (schema != null)
            {
                schema.PerformValidation(path, value, results);
                return;
            }

            // If value is null then skip
            value = ObjectReader.GetValue(value);
            if (value == null) return;

            // Match types
            if (TypeMatcher.MatchType(type, value.GetType()))
                return;

            var name = path ?? "value";
            var valueType = value.GetType();

            // Generate type mismatch error
            results.Add(
                new ValidationResult(
                    path,
                    ValidationResultType.Error,
                    "TYPE_MISMATCH",
                    name + " type must be " + type + " but found " + valueType.Name,
                    type,
                    valueType.Name
                )
            );
        }

        /// <summary>
        /// Validates the given value and results validation results.
        /// </summary>
        /// <param name="value">a value to be validated.</param>
        /// <returns>a list with validation results.</returns>
        public List<ValidationResult> Validate(object value)
        {
            var results = new List<ValidationResult>();
            PerformValidation("", value, results);
            return results;
        }

        /// <summary>
        /// Validates the given value and returns a ValidationException if errors were found.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="strict">true to treat warnings as errors.</param>
        public void ValidateAndThrowException(string correlationId, object value, bool strict = false)
        {
            var results = Validate(value);

            ValidationException.ThrowExceptionIfNeeded(correlationId, results, strict);
        }
    }
}