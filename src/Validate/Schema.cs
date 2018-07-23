using System.Linq;
using System.Collections.Generic;
using PipServices.Commons.Reflect;

namespace PipServices.Commons.Validate
{
    public class Schema
    {
        public Schema() { }

        public Schema(bool required, List<IValidationRule> rules)
        {
            IsRequired = required;
            Rules = rules;
        }

        public bool IsRequired { get; set; }

        public List<IValidationRule> Rules { get; set; }

        public Schema MakeRequired()
        {
            IsRequired = true;
            return this;
        }

        public Schema MakeOptional()
        {
            IsRequired = false;
            return this;
        }

        public Schema WithRule(IValidationRule rule)
        {
            Rules = Rules ?? new List<IValidationRule>();
            Rules.Add(rule);
            return this;
        }

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

        public List<ValidationResult> Validate(object value)
        {
            var results = new List<ValidationResult>();
            PerformValidation("", value, results);
            return results;
        }

        public void ValidateAndThrowException(string correlationId, object value, bool strict = false)
        {
            var results = Validate(value);

            ValidationException.ThrowExceptionIfNeeded(correlationId, results, strict);
        }
    }
}