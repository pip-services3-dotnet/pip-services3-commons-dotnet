using PipServices.Commons.Reflect;
using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public class PropertiesComparisonRule : IValidationRule
    {
        private readonly string _property1;
        private readonly string _property2;
        private readonly string _operation;

        public PropertiesComparisonRule(string property1, string operation, string property2)
        {
            _property1 = property1;
            _operation = operation;
            _property2 = property2;
        }

        public void Validate(string path, Schema schema, object value, List<ValidationResult> results)
        {
            var name = path ?? "value";
            var value1 = ObjectReader.GetProperty(value, _property1);
            var value2 = ObjectReader.GetProperty(value, _property2);

            if (!ObjectComparator.Compare(value1, _operation, value2))
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "PROPERTIES_NOT_MATCH",
                        name + " must have " + _property1 + " " + _operation + " " + _property2,
                        value2,
                        value1
                    )
                );
            }
        }
    }
}
