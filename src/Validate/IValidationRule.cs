using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public interface IValidationRule
    {
        void Validate(string path, Schema schema, object value, List<ValidationResult> results);
    }
}
