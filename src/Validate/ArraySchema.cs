using System.Collections;
using System.Collections.Generic;
using PipServices3.Commons.Reflect;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Schema to validate arrays.
    /// </summary>
    /// <example>
    /// <code>
    /// var schema = new ArraySchema(TypeCode.String);
    /// 
    /// schema.Validate(new String[]{"A", "B", "C"});    // Result: no errors
    /// schema.Validate(new int[] {1, 2, 3});          // Result: element type mismatch
    /// schema.Validate("A");                // Result: type mismatch
    /// </code>
    /// </example>
    public class ArraySchema : Schema
    {
        /// <summary>
        /// Creates a new instance of validation schema.
        /// </summary>
        public ArraySchema() { }

        /// <summary>
        /// Creates a new instance of validation schema and sets its values.
        /// </summary>
        /// <param name="valueType">a type of array elements. Null means that elements may have any type.</param>
        public ArraySchema(object valueType)
        {
            ValueType = valueType;
        }

        public object ValueType { get; set; }

        /// <summary>
        /// Validates a given value against the schema and configured validation rules.
        /// </summary>
        /// <param name="path">a dot notation path to the value.</param>
        /// <param name="value">a value to be validated.</param>
        /// <param name="results">a list with validation results to add new results.</param>
        protected internal override void PerformValidation(string path, object value, List<ValidationResult> results)
        {
            var name = path ?? "value";
            value = ObjectReader.GetValue(value);

            base.PerformValidation(path, value, results);

            if (value == null) return;

            var list = value as IEnumerable;
            if (list != null)
            {
                var index = 0;
                foreach (var element in list)
                {
                    var elementPath = string.IsNullOrWhiteSpace(path)
                        ? index.ToString() : path + "." + index;
                    PerformTypeValidation(elementPath, ValueType, element, results);
                    index++;
                }
            }
            else
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "VALUE_ISNOT_ARRAY",
                        name + " type must be List or Array",
                        typeof(IEnumerable),
                        value.GetType()
                    )
                );
            }
        }
    }
}
