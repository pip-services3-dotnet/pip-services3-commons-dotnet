using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public class NotRule
    {
        private readonly IValidationRule _rule;

        public NotRule(IValidationRule rule)
        {
            _rule = rule;
        }

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
