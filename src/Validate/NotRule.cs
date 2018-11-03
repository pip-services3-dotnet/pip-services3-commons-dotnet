using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Validation rule negate another rule. 
    /// When embedded rule returns no errors, than this rule return an error.
    /// When embedded rule return errors, than the rule returns no errors.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new Schema().WithRule(
    /// new NotRule(new ValueComparisonRule("EQ", 1)));
    /// 
    /// schema.Validate(1);          // Result: error
    /// schema.Validate(5);          // Result: no error
    /// </code>
    /// </example>
    /// See <see cref="IValidationRule"/>
    public class NotRule
    {
        private readonly IValidationRule _rule;

        /// <summary>
        /// Creates a new validation rule and sets its values
        /// </summary>
        /// <param name="rule">a rule to be negated.</param>
        public NotRule(IValidationRule rule)
        {
            _rule = rule;
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
            if (_rule == null)
                return;

            var name = path ?? "value";
            var localResults = new List<ValidationResult>();
            _rule.Validate(path, schema, value, localResults);

            if (localResults.Count > 0)
                return;

            results.Add(
                new ValidationResult(
                    path,
                    ValidationResultType.Error,
                    "NOT_FAILED",
                    "Negative check for " + name + " failed",
                    null,
                    null
                )
            );
        }
    }
}
