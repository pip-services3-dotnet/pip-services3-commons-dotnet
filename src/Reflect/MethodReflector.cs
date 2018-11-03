using System;
using System.Collections.Generic;
using System.Reflection;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Helper class to perform method introspection and dynamic invocation.
    /// This class has symmetric implementation across all languages supported
    /// by Pip.Services toolkit and used to support dynamic data processing.
    /// 
    /// Because all languages have different casing and case sensitivity rules,
    /// this MethodReflector treats all method names as case insensitive.
    /// </summary>
    /// <example>
    /// <code>
    /// var myObj = new MyObject();
    /// 
    /// var methods = MethodReflector.GetMethodNames();
    /// MethodReflector.HasMethod(myObj, "myMethod");
    /// MethodReflector.InvokeMethod(myObj, "myMethod", 123);
    /// </code>
    /// </example>
    public class MethodReflector
    {
        private static bool MatchMethod(MethodInfo method, string name)
        {
            return method.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                && method.IsPublic && !method.IsStatic
                && !method.IsAbstract;
        }

        /// <summary>
        /// Checks if object has a method with specified name.
        /// </summary>
        /// <param name="obj">an object to introspect.</param>
        /// <param name="name">a name of the method to check.</param>
        /// <returns>true if the object has the method and false if it doesn't.</returns>
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

        /// <summary>
        /// Invokes an object method by its name with specified parameters.
        /// </summary>
        /// <param name="obj">an object to invoke.</param>
        /// <param name="name">a name of the method to invoke.</param>
        /// <param name="args">a list of method arguments.</param>
        /// <returns>the result of the method invocation or null if method returns void.</returns>
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

        /// <summary>
        /// Gets names of all methods implemented in specified object.
        /// </summary>
        /// <param name="obj">an objec to introspect.</param>
        /// <returns>a list with method names.</returns>
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
