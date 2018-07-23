using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public class IncludedRule : IValidationRule
    {
        private readonly object[] _values;

        public IncludedRule(params object[] values)
        {
            _values = values;
        }

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
