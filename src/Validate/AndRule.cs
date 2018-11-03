using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Validation rule to combine rules with AND logical operation.
    /// When all rules returns no errors, than this rule also returns no errors.
    /// When one of the rules return errors, than the rules returns all errors.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new Schema().WithRule(new AndRule(
    /// new ValueComparisonRule("GTE", 1),
    /// new ValueComparisonRule("LTE", 10)
    /// ));
    /// 
    /// schema.Validate(0);          // Result: 0 must be greater or equal to 1
    /// schema.Validate(5);          // Result: no error
    /// schema.Validate(20);         // Result: 20 must be letter or equal 10
    /// </code>
    /// </example>
    /// See <see cref="IValidationRule"/>
    public class AndRule : IValidationRule
    {
        private readonly IValidationRule[] _rules;

        /// <summary>
        /// Creates a new validation rule and sets its values.
        /// </summary>
        /// <param name="rules">a list of rules to join with AND operator</param>
        public AndRule(params IValidationRule[] rules)
        {
            _rules = rules;
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
            if (_rules == null) return;

            foreach (var rule in _rules)
                rule.Validate(path, schema, value, results);
        }

    }
}
