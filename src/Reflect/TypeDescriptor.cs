using System.Text;
using PipServices3.Commons.Errors;

namespace PipServices3.Commons.Reflect
{
    /// <summary>
    /// Descriptor that points to specific object type by it's name
    /// and optional library(or module) where this type is defined.
    /// 
    /// This class has symmetric implementation across all languages supported
    /// by Pip.Services toolkit and used to support dynamic data processing.
    /// </summary>
    public class TypeDescriptor
    {
        /// <summary>
        /// Creates a new instance of the type descriptor.
        /// </summary>
        public TypeDescriptor() { }

        /// <summary>
        /// Creates a new instance of the type descriptor and sets its values.
        /// </summary>
        /// <param name="name">a name of the object type.</param>
        /// <param name="library">a library or module where this object type is implemented.</param>
        public TypeDescriptor(string name, string library)
        {
            Name = name;
            Library = library;
        }

        public string Name { get; }
        public string Library { get; }

        /// <summary>
        /// Compares this descriptor to a value. If the value is also a TypeDescriptor it
        /// compares their name and library fields.Otherwise this method returns false.
        /// </summary>
        /// <param name="obj">a value to compare.</param>
        /// <returns>true if value is identical TypeDescriptor and false otherwise.</returns>
        public override bool Equals(object obj)
        {
            var type = obj as TypeDescriptor;
            if (type != null) {
                var otherType = type;
                if (Name == null || otherType.Name == null)
                    return false;

                if (!Name.Equals(otherType.Name))
                    return false;

                if (Library == null || otherType.Library == null
                    || Library.Equals(otherType.Library))
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Gets a string representation of the object. The result has format name[, library]
        /// </summary>
        /// <returns>a string representation of the object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Name);

            if (Library != null)
                builder.Append(',').Append(Library);

            return builder.ToString();
        }

        /// <summary>
        /// Parses a string to get descriptor fields and returns them as a Descriptor.
        /// The string must have format name[, library]
        /// </summary>
        /// <param name="value">a string to parse.</param>
        /// <returns>a newly created Descriptor.</returns>
        public static TypeDescriptor FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var tokens = value.Split(',');

            if (tokens.Length == 1)
                return new TypeDescriptor(tokens[0].Trim(), null);
            else if (tokens.Length == 2)
                return new TypeDescriptor(tokens[0].Trim(), tokens[1].Trim());

            throw (ConfigException)new ConfigException(
                null, "BAD_DESCRIPTOR", "Type descriptor " + value + " is in wrong format"
            ).WithDetails("descriptor", value);
        }
    }
}
