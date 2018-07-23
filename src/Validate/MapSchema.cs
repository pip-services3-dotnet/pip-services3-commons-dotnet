using PipServices.Commons.Reflect;
using System.Collections;
using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public class MapSchema : Schema
    {
        public MapSchema() { }

        public MapSchema(object keyType, object valueType)
        {
            KeyType = keyType;
            ValueType = valueType;
        }

        public object KeyType { get; set; }
        public object ValueType { get; set; }

        protected internal override void PerformValidation(string path, object value, List<ValidationResult> results)
        {
            var name = path ?? "value";
            value = ObjectReader.GetValue(value);

            base.PerformValidation(path, value, results);

            if (value == null) return;

            var map = value as IDictionary;
            if (map != null)
            {
                foreach (var key in map.Keys)
                {
                    var elementPath = string.IsNullOrWhiteSpace(path)
                        ? key.ToString() : path + "." + key;

                    PerformTypeValidation(elementPath, KeyType, key, results);
                    PerformTypeValidation(elementPath, ValueType, map[key], results);
                }
            }
            else
            {
                results.Add(
                    new ValidationResult(
                        path,
                        ValidationResultType.Error,
                        "VALUE_ISNOT_MAP",
                        name + " type must to be Map (Dictionary)",
                        typeof(IDictionary),
                        value.GetType()
                    )
                );
            }
        }
    }
}
