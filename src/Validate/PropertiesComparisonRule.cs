using PipServices3.Commons.Reflect;
using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Validation rule that compares two object properties.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new ObjectSchema().WithRule(new PropertyComparisonRule("field1", "NE", "field2"));
    /// 
    /// schema.Validate({ field1: 1, field2: 2 });       // Result: no errors
    /// schema.Validate({ field1: 1, field2: 1 });       // Result: field1 shall not be equal to field2
    /// schema.Validate({ });                             // Result: no errors
    /// </code>
    /// </example>
    /// See <see cref="IValidationRule"/>
    public class PropertiesComparisonRule : IValidationRule
    {
        private readonly string _property1;
        private readonly string _property2;
        private readonly string _operation;

        /// <summary>
        /// Creates a new validation rule and sets its arguments.
        /// </summary>
        /// <param name="property1">a name of the first property to compare.</param>
        /// <param name="operation">a comparison operation.</param>
        /// <param name="property2">a name of the second property to compare.</param>
        public PropertiesComparisonRule(string property1, string operation, string property2)
        {
            _property1 = property1;
            _operation = operation;
            _property2 = property2;
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
            var value1 = ObjectReader.GetProperty(value, _property1);
            var value2 = ObjectReader.GetProperty(value, _property2);

            if (!ObjectComparator.Compare(value1, _operation, value2))
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "PROPERTIES_NOT_MATCH",
                        name + " must have " + _property1 + " " + _operation + " " + _property2,
                        value2,
                        value1
                    )
                );
            }
        }
    }
}
