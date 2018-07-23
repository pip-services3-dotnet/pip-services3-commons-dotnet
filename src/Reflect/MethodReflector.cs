using System;
using System.Collections.Generic;
using System.Reflection;

namespace PipServices.Commons.Reflect
{
    public class MethodReflector
    {
        private static bool MatchMethod(MethodInfo method, string name)
        {
            return method.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                && method.IsPublic && !method.IsStatic
                && !method.IsAbstract;
        }

        public static bool HasMethod(object obj, string name)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null");
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Method name cannot be null");

            var type = obj.GetType();

            foreach (var method in type.GetRuntimeMethods())
            {
                if (MatchMethod(method, name))
                    return true;
            }

            return false;
        }

        public static object InvokeMethod(object obj, string name, params object[] args)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null");
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Method name cannot be null");

            var type = obj.GetType();

            foreach (var method in type.GetRuntimeMethods())
            {
                try
                {
                    if (MatchMethod(method, name))
                        return method.Invoke(obj, args);
                }
                catch (Exception)
                {
                    // Ignore exceptions
                }
            }

            return null;
        }

        public static IEnumerable<string> GetMethodNames(object obj)
        {
            var methods = new List<string>();

            var objClass = obj.GetType();

            foreach (var method in objClass.GetRuntimeMethods())
            {
                if (MatchMethod(method, method.Name))
                {
                    methods.Add(method.Name);
                }
            }

            return methods;
        }
    }
}
