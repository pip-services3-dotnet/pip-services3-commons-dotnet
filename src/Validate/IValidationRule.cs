using System.Collections.Generic;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Interface for validation rules.
    /// 
    /// Validation rule can validate one or multiple values
    /// against complex rules like: value is in range,
    /// one property is less than another property,
    /// enforce enumerated values and more.
    /// This interface allows to implement custom rules.
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        /// Validates a given value against this rule.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="schema">a schema this rule is called from</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        void Validate(string path, Schema schema, object value, List<ValidationResult> results);
    }
}
