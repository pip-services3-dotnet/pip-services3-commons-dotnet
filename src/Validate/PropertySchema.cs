using System.Collections.Generic;

namespace PipServices.Commons.Validate
{
    public class PropertySchema : Schema
    {
        public PropertySchema() { }

        public PropertySchema(string name, object type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public object Type { get; set; }

        protected internal override void PerformValidation(string path, object value, List<ValidationResult> results)
        {
            path = string.IsNullOrWhiteSpace(path) ? Name : path + "." + Name;

            base.PerformValidation(path, value, results);
            PerformTypeValidation(path, Type, value, results);
        }
    }
}