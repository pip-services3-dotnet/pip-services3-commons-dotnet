using System.Collections.Generic;

using PipServices.Commons.Convert;

namespace PipServices.Commons.Reflect
{
    public sealed class RecursiveObjectWriter
    {
        private static object CreateProperty(object obj, string[] names, int nameIndex)
        {
            // If next field is index then create an array
            string subField = names.Length > nameIndex + 1 ? names[nameIndex + 1] : null;
            int? subFieldIndex = IntegerConverter.ToNullableInteger(subField);
            if (subFieldIndex != null)
                return new List<object>();

            // Else create a dictionary
            return new Dictionary<string, object>();
        }

        private static void PerformSetProperty(object obj, string[] names, int nameIndex, object value)
        {
            if (nameIndex < names.Length - 1)
            {
                var subObj = ObjectReader.GetProperty(obj, names[nameIndex]);
                if (subObj != null)
                    PerformSetProperty(subObj, names, nameIndex + 1, value);
                else
                {
                    subObj = CreateProperty(obj, names, nameIndex);
                    if (subObj != null)
                    {
                        PerformSetProperty(subObj, names, nameIndex + 1, value);
                        ObjectWriter.SetProperty(obj, names[nameIndex], subObj);
                    }
                }
            }
            else
                ObjectWriter.SetProperty(obj, names[nameIndex], value);
        }

        public static void SetProperty(object obj, string name, object value)
        {
            if (obj == null || name == null) return;

            var names = name.Split('.');
            if (names == null || names.Length == 0)
                return;

            PerformSetProperty(obj, names, 0, value);
        }

        public static void SetProperties(object obj, IDictionary<string, object> values)
        {
            if (values == null || values.Count == 0) return;

            foreach (var entry in values)
                SetProperty(obj, entry.Key, entry.Value);
        }

        public static void CopyProperties(object dest, object src)
        {
            if (dest == null || src == null) return;

            var values = RecursiveObjectReader.GetProperties(src);
            SetProperties(dest, values);
        }
    }
}
