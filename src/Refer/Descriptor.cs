using PipServices3.Commons.Errors;
using System.Text;

namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// Locator type that most often used in PipServices toolkit.
    /// It locates components using several fields:
    /// - Group: a package or just named group of components like "pip-services3"
    /// - Type: logical component type that defines it's contract like "persistence"
    /// - Kind: physical implementation type like "mongodb"
    /// - Name: unique component name like "default"
    /// - Version: version of the component contract like "1.0"
    /// 
    /// The locator matching can be done by all or only few selected fields.
    /// The fields that shall be excluded from the matching must be set to <c>"*"</c> or <c>null</c>.
    /// That approach allows to implement many interesting scenarios. For instance:
    /// - Locate all loggers (match by type and version)
    /// - Locate persistence components for a microservice(match by group and type)
    /// - Locate specific component by its name(match by name)
    /// </summary>
    /// <example>
    /// <code>
    /// var locator1 = new Descriptor("mygroup", "connector", "aws", "default", "1.0");
    /// var locator2 = Descriptor.fromString("mygroup:connector:*:*:1.0");
    /// 
    /// locator1.match(locator2);		// Result: true
    /// locator1.equal(locator2);		// Result: true
    /// locator1.exactMatch(locator2);	// Result: false
    /// </code>
    /// </example>
    public class Descriptor
    {
        /// <summary>
        /// Creates a new instance of the descriptor.
        /// </summary>
        /// <param name="group">a logical component group</param>
        /// <param name="type">a logical component type or contract</param>
        /// <param name="kind">a component implementation type</param>
        /// <param name="name">a unique component name</param>
        /// <param name="version">a component implementation version</param>
        public Descriptor(string group, string type, string kind, string name, string version)
        {
            Group = group != "*" ? group : null;
            Type = type != "*" ? type : null;
            Kind = kind != "*" ? kind : null;
            Name = name != "*" ? name : null;
            Version = version != "*" ? version : null;
        }

        public string Group { get; private set; }
        public string Type { get; private set; }
        public string Kind { get; private set; }
        public string Name { get; private set; }
        public string Version { get; private set; }

        private bool MatchField(string field1, string field2)
        {
            return field1 == null
                   || field2 == null
                   || field1.Equals(field2);
        }

        /// <summary>
        /// Partially matches this descriptor to another descriptor. Fields that contain 
        /// "*" or null are excluded from the match.
        /// </summary>
        /// <param name="descriptor">the descriptor to match this one against.</param>
        /// <returns>true if descriptors match and false otherwise</returns>
        public bool Match(Descriptor descriptor)
        {
            return MatchField(Group, descriptor.Group)
                   && MatchField(Type, descriptor.Type)
                   && MatchField(Kind, descriptor.Kind)
                   && MatchField(Name, descriptor.Name)
                   && MatchField(Version, descriptor.Version);
        }

        private bool ExactMatchField(string field1, string field2)
        {
            if (field1 == null && field2 == null)
                return true;
            if (field1 == null || field2 == null)
                return false;
            return field1.Equals(field2);
        }

        /// <summary>
        /// Matches this descriptor to another descriptor by all fields. No exceptions are made.
        /// </summary>
        /// <param name="descriptor">the descriptor to match this one against.</param>
        /// <returns>true if descriptors match and false otherwise.</returns>
        public bool ExactMatch(Descriptor descriptor)
        {
            return ExactMatchField(Group, descriptor.Group)
                && ExactMatchField(Type, descriptor.Type)
                && ExactMatchField(Kind, descriptor.Kind)
                && ExactMatchField(Name, descriptor.Name)
                && ExactMatchField(Version, descriptor.Version);
        }

        /// <summary>
        /// Checks whether all descriptor fields are set. If descriptor has at least one
        /// "*" or null field it is considered "incomplete",
        /// </summary>
        /// <returns>true if all descriptor fields are defined and false otherwise.</returns>
        public bool IsComplete()
        {
            return Group != null && Type != null && Kind != null
                && Name != null && Version != null;
        }

        /// <summary>
        /// Compares this descriptor to a value. If value is a Descriptor it tries to
        /// match them, otherwise the method returns false.
        /// </summary>
        /// <param name="obj">the value to match against this descriptor.</param>
        /// <returns>true if the value is matching descriptor and false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Descriptor)
            {
                return Match((Descriptor)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Gets a string representation of the object. The result is a colon-separated
        /// list of descriptor fields as "mygroup:connector:aws:default:1.0"
        /// </summary>
        /// <returns>a string representation of the object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Group ?? "*")
                .Append(":").Append(Type ?? "*")
                .Append(":").Append(Kind ?? "*")
                .Append(":").Append(Name ?? "*")
                .Append(":").Append(Version ?? "*");
            return builder.ToString();
        }

        /// <summary>
        /// Parses colon-separated list of descriptor fields and returns them as a Descriptor.
        /// </summary>
        /// <param name="value">colon-separated descriptor fields to initialize Descriptor.</param>
        /// <returns>a newly created Descriptor.</returns>
        public static Descriptor FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var tokens = value.Split(':');

            if (tokens.Length != 5)
            {
                throw new ConfigException(
                    null, "BAD_DESCRIPTOR", "Descriptor " + value + " is in wrong format"
                ).WithDetails("descriptor", value);
            }

            return new Descriptor(tokens[0].Trim(), tokens[1].Trim(), tokens[2].Trim(), tokens[3].Trim(), tokens[4].Trim());
        }
    }
}