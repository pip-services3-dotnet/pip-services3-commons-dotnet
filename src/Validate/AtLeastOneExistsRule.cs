using PipServices.Commons.Reflect;
using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public class AtLeastOneExistsRule : IValidationRule
    {
        private readonly string[] _properties;

        public AtLeastOneExistsRule(params string[] properties)
        {
            _properties = properties;
        }

        public void Validate(string path, Schema schema, object value, List<ValidationResult> results)
        {
            var name = path ?? "value";
            var found = new List<string>();

            foreach (var property in _properties)
            {
                var propertyValue = ObjectReader.GetProperty(value, property);
                if (propertyValue != null)
                    found.Add(property);
            }

            if (found.Count == 0)
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "VALUE_NULL",
                        name + " must have at least one property from " + _properties,
                        _properties,
                        null
                    )
                );
            }
        }
    }
}
