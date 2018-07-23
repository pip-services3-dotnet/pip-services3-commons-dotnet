using System.Text;
using PipServices.Commons.Errors;

namespace PipServices.Commons.Reflect
{
    public class TypeDescriptor
    {
        public TypeDescriptor() { }

        public TypeDescriptor(string name, string library)
        {
            Name = name;
            Library = library;
        }

        public string Name { get; }
        public string Library { get; }
        
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

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Name);

            if (Library != null)
                builder.Append(',').Append(Library);

            return builder.ToString();
        }

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
