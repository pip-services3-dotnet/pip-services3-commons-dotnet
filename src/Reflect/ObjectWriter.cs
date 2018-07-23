using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PipServices.Commons.Convert;

namespace PipServices.Commons.Reflect
{
    public class ObjectWriter
    {
        public static void SetProperty(object obj, string name, object value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null");
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Method name cannot be null");

            var type = obj.GetType().GetTypeInfo();
            var isDict = type.GetInterfaces().Contains(typeof(IDictionary)) || type.GetInterfaces().Contains(typeof(IDictionary<,>));

            if (isDict)
            {
                var mapObj = (IDictionary<string, object>)obj;

                foreach (var key in mapObj.Keys)
                {
                    if (name.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        mapObj[key] = value;
                        return;
                    }
                }

                mapObj[name] = value;
            }
            else if (obj.GetType().IsArray)
            {
                var array = ((Array)obj);
                var length = array.Length;
                var index = IntegerConverter.ToIntegerWithDefault(name, -1);

                if (index >= 0 && index < length)
                {
                    array.SetValue(value, index);
                }
            }
            else if (type.GetInterfaces().Contains(typeof(IList)) || type.GetInterfaces().Contains(typeof(IList<>)))
            {
                var list = (IList<object>)obj;
                var index = IntegerConverter.ToIntegerWithDefault(name, -1);
                if (index < 0)
                    return;

                if (index < list.Count)
                    list[index] = value;
                else
                {
                    while (index - 1 >= list.Count)
                        list.Add(null);
                    list.Add(value);
                }
            }
            else
            {
                PropertyReflector.SetProperty(obj, name, value);
            }
        }

        public static void SetProperties(object obj, IDictionary<string, object> values)
        {
            if (values == null || values.Count == 0)
                return;

            foreach (var entry in values)
            {
                SetProperty(obj, entry.Key, entry.Value);
            }
        }
    }
}
