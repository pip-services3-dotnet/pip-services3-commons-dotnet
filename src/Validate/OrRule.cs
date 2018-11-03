using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Validation rule to combine rules with OR logical operation.
    /// When one of rules returns no errors, than this rule also returns no errors.
    /// When all rules return errors, than the rule returns all errors.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new Schema().WithRule(new OrRule(new ValueComparisonRule("LT", 1), 
    ///                                               new ValueComparisonRule("GT", 10)));
    /// 
    /// schema.Validate(0);          // Result: no error
    /// schema.Validate(5);          // Result: 5 must be less than 1 or 5 must be more than 10
    /// schema.Validate(20);         // Result: no error
    /// </code>
    /// </example>
    /// See <see cref="IValidationRule"/>
    public class OrRule : IValidationRule
    {
        private readonly IValidationRule[] _rules;

        /// <summary>
        /// Creates a new validation rule and sets its values.
        /// </summary>
        /// <param name="rules">a list of rules to join with OR operator</param>
        public OrRule(params IValidationRule[] rules)
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
            if (_rules == null || _rules.Length == 0)
                return;

            var localResults = new List<ValidationResult>();

            foreach (var rule in _rules)
            {
                var resultCount = localResults.Count;

                rule.Validate(path, schema, value, localResults);

                if (resultCount == localResults.Count)
                    return;
            }

            results.AddRange(localResults);
        }
    }
}
