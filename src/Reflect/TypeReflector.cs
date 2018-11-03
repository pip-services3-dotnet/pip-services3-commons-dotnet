using System;
using PipServices3.Commons.Errors;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Helper class to perform object type introspection and object instantiation.
    /// 
    /// This class has symmetric implementation across all languages supported
    /// by Pip.Services toolkit and used to support dynamic data processing.
    /// 
    /// Because all languages have different casing and case sensitivity rules,
    /// this TypeReflector treats all type names as case insensitive.
    /// </summary>
    /// <example>
    /// <code>
    /// var descriptor = new TypeDescriptor("MyObject", "mylibrary");
    /// TypeReflector.GetTypeByDescriptor(descriptor);
    /// var myObj = TypeReflector.createInstanceByDescriptor(descriptor);
    /// 
    /// </code>
    /// </example>
    /// See <see cref="TypeDescriptor"/>
    public class TypeReflector
    {
        /// <summary>
        /// Gets object type by its name and library where it is defined.
        /// </summary>
        /// <param name="name">an object type name.</param>
        /// <param name="library">a library where the type is defined</param>
        /// <returns>the object type or null is the type wasn't found.</returns>
        public static Type GetType(string name, string library)
        {
            try
            {
                // Load module
                if (!string.IsNullOrWhiteSpace(library) && !string.IsNullOrWhiteSpace(library))
                    return Type.GetType(name + "," + library);
                else
                    return Type.GetType(name);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets object type by its name.
        /// </summary>
        /// <param name="name">an object type name.</param>
        /// <returns>the object type or null is the type wasn't found.</returns>
        public static Type GetType(string name)
        {
            return GetType(name, null);
        }

        /// <summary>
        /// Gets object type by type descriptor.
        /// </summary>
        /// <param name="type">a type descriptor that points to an object type</param>
        /// <returns>the object type or null is the type wasn't found.</returns>
        /// See <see cref="GetType(string, string)"/>, <see cref="TypeDescriptor"/>
        public static Type GetTypeByDescriptor(TypeDescriptor type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Type descriptor cannot be null");

            return GetType(type.Name, type.Library);
        }

        /// <summary>
        /// Creates an instance of an object type.
        /// </summary>
        /// <param name="type">an object type (factory function) to create.</param>
        /// <param name="args">arguments for the object constructor.</param>
        /// <returns>the created object instance.</returns>
        public static object CreateInstanceByType(Type type, params object[] args)
        {
            if (args.Length == 0)
                return Activator.CreateInstance(type);

            throw new UnsupportedException(null, "NOT_SUPPORTED", "Constructors with paratemeters are not supported");
        }

        /// <summary>
        /// Creates an instance of an object type specified by its name and library where it is defined.
        /// </summary>
        /// <param name="name">an object type name.</param>
        /// <param name="library">a library (module) where object type is defined.</param>
        /// <param name="args">arguments for the object constructor.</param>
        /// <returns>the created object instance.</returns>
        public static object CreateInstance(string name, string library, params object[] args)
        {
            var type = GetType(name, library);
            if (type == null)
                throw new NotFoundException(null, "TYPE_NOT_FOUND", "Type " + name + "," + library + " was not found")
                    .WithDetails("type", name).WithDetails("library", library);

            return CreateInstanceByType(type, args);
        }

        /// <summary>
        /// Creates an instance of an object type specified by its name.
        /// </summary>
        /// <param name="name">an object type name.</param>
        /// <param name="args">arguments for the object constructor.</param>
        /// <returns>the created object instance.</returns>
        public static object CreateInstance(string name, params object[] args)
        {
            return CreateInstance(name, null, args);
        }

        /// <summary>
        /// Creates an instance of an object type specified by type descriptor.
        /// </summary>
        /// <param name="type">a type descriptor that points to an object type</param>
        /// <param name="args">arguments for the object constructor.</param>
        /// <returns>the created object instance.</returns>
        public static object CreateInstanceByDescriptor(TypeDescriptor type, params object[] args)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Type descriptor cannot be null");

            return CreateInstance(type.Name, type.Library, args);
        }
    }
}
