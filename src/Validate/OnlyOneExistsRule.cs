using PipServices3.Commons.Reflect;
using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Validation rule that check that at exactly one of the object properties is not null.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new Schema().WithRule(new OnlyOneExistsRule("field1", "field2"));
    /// 
    /// schema.Validate({ field1: 1, field2: "A" });     // Result: only one of properties field1, field2 must exist
    /// schema.Validate({ field1: 1 });                  // Result: no errors
    /// schema.Validate({ });                            // Result: only one of properties field1, field2 must exist
    /// </code>
    /// </example>
    /// See <see cref="IValidationRule"/>
    public class OnlyOneExistsRule : IValidationRule
    {
        private readonly string[] _properties;

        /// <summary>
        /// Creates a new validation rule and sets its values
        /// </summary>
        /// <param name="properties">a list of property names where at only one property must exist</param>
        public OnlyOneExistsRule(params string[] properties)
        {
            _properties = properties;
        }

        /// <summary>
        /// Validates a given value against this rule.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="schema">a schema this rule is called from</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        public void Validate(string path, Schema schema, object value, List<ValidationResult> results)
        {
            var name = path ?? "value";
            var found = new List<string>();

            foreach (var property in _properties)
            {
                var propertyValue = ObjectReader.GetProperty(value, property);
                if (propertyValue != null)
                    found.Add(property);
            }

            if (found.Count == 0)
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "VALUE_NULL",
                        name + " must hae at least one property from " + _properties,
                        _properties,
                        null
                    )
                );
            }
            else if (found.Count > 1)
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "VALUE_ONLY_ONE",
                        name + " must have only one property from " + _properties,
                        _properties,
                        found.ToArray()
                    )
                );
            }
        }
    }
}
