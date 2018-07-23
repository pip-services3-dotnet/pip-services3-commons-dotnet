using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public class OrRule : IValidationRule
    {
        private readonly IValidationRule[] _rules;

        public OrRule(params IValidationRule[] rules)
        {
            _rules = rules;
        }

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
