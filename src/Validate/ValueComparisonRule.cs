using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Validation rule that compares value to a constant.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new Schema().WithRule(new ValueComparisonRule("EQ", 1));
    /// 
    /// schema.Validate(1);          // Result: no errors
    /// schema.Validate(2);          // Result: 2 is not equal to 1
    /// </code>
    /// </example>
    public class ValueComparisonRule : IValidationRule
    {
        private readonly string _operation;
        private readonly object _value;

        /// <summary>
        /// Creates a new validation rule and sets its values.
        /// </summary>
        /// <param name="operation">a comparison operation.</param>
        /// <param name="value">a constant value to compare to</param>
        public ValueComparisonRule(string operation, object value)
        {
            _operation = operation;
            _value = value;
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

            if (!ObjectComparator.Compare(value, _operation, _value))
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "BAD_VALUE",
                        name + " must have " + _operation + " " + _value + " but found " + value,
                        _operation + " " + _value,
                        value
                    )
                );
            }
        }
    }
}
