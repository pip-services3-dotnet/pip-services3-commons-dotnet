using System;
using PipServices.Commons.Errors;

namespace PipServices.Commons.Reflect
{
    public class TypeReflector
    {
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

        public static Type GetType(string name)
        {
            return GetType(name, null);
        }

        public static Type GetTypeByDescriptor(TypeDescriptor type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Type descriptor cannot be null");

            return GetType(type.Name, type.Library);
        }

        public static object CreateInstanceByType(Type type, params object[] args)
        {
            if (args.Length == 0)
                return Activator.CreateInstance(type);

            throw new UnsupportedException(null, "NOT_SUPPORTED", "Constructors with paratemeters are not supported");
        }

        public static object CreateInstance(string name, string library, params object[] args)
        {
            var type = GetType(name, library);
            if (type == null)
                throw new NotFoundException(null, "TYPE_NOT_FOUND", "Type " + name + "," + library + " was not found")
                    .WithDetails("type", name).WithDetails("library", library);

            return CreateInstanceByType(type, args);
        }

        public static object CreateInstance(string name, params object[] args)
        {
            return CreateInstance(name, null, args);
        }

        public static object CreateInstanceByDescriptor(TypeDescriptor type, params object[] args)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Type descriptor cannot be null");

            return CreateInstance(type.Name, type.Library, args);
        }
    }
}
