using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Validation rule to check that value is included into the list of constants.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new Schema().WithRule(new IncludedRule(1, 2, 3));
    /// 
    /// schema.Validate(2);      // Result: no errors
    /// schema.Validate(10);     // Result: 10 must be one of 1, 2, 3
    /// </code>
    /// </example>
    /// See <see cref="IValidationRule"/>
    public class IncludedRule : IValidationRule
    {
        private readonly object[] _values;

        /// <summary>
        /// Creates a new validation rule and sets its values.
        /// </summary>
        /// <param name="values">a list of constants that value must be included to</param>
        public IncludedRule(params object[] values)
        {
            _values = values;
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
            bool found = false;

            foreach (var thisValue in _values)
            {
                if (thisValue != null && thisValue.Equals(value))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "VALUE_NOT_INCLUDED",
                        name + " must be one of " + _values,
                        _values,
                        value
                    )
                );
            }
        }
    }
}
