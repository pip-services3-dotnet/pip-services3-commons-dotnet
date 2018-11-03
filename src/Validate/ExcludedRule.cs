using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Validation rule to check that value is excluded from the list of constants.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new Schema().WithRule(new ExcludedRule(1, 2, 3));
    /// 
    /// schema.Validate(2);      // Result: 2 must not be one of 1, 2, 3
    /// schema.Validate(10);     // Result: no errors
    /// </code>
    /// </example>
    public class ExcludedRule : IValidationRule
    {
        private readonly object[] _values;

        /// <summary>
        /// Creates a new validation rule and sets its values.
        /// </summary>
        /// <param name="values">a list of constants that value must be excluded from</param>
        public ExcludedRule(params object[] values)
        {
            _values = values;
        }

        /// <summary>
        /// Validates the given value. None of the values set in this ExcludedRule object
        /// must exist in the value that is given for validation to pass.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="schema">a schema this rule is called from</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        public void Validate(string path, Schema schema, object value, List<ValidationResult> results)
        {
            var name = path ?? "value";
            var found = false;

            foreach (var thisValue in _values)
            {
                if (thisValue != null && thisValue.Equals(value))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "VALUE_INCLUDED",
                        name + " must not be one of " + _values,
                        _values,
                        value
                    )
                );
            }
        }
    }
}
