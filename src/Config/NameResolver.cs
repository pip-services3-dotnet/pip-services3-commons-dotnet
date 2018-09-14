using PipServices.Commons.Refer;

namespace PipServices.Commons.Config
{
    /// <summary>
    /// A helper class that allows to extract component name from configuration parameters.
    /// The name can be defined in "id", "name" parameters or inside a component descriptor.
    /// </summary>
    /// <example>
    /// <code>
    /// var config = ConfigParams.fromTuples( "descriptor", "myservice:connector:aws:connector1:1.0",
    /// "param1", "ABC",
    /// "param2", 123 );
    /// 
    /// var name = NameResolver.resolve(config); // Result: connector1
    /// </code>
    /// </example>
    public static class NameResolver
    {
        /// <summary>
        /// Resolves a component name from configuration parameters. 
        /// The name can be stored in "id", "name" fields or inside a component descriptor.If name
        /// cannot be determined it returns a defaultName.
        /// </summary>
        /// <param name="config">configuration parameters that may contain a component name.</param>
        /// <param name="defaultName">(optional) a default component name.</param>
        /// <returns>resolved name or default name if the name cannot be determined.</returns>
        public static string Resolve(ConfigParams config, string defaultName = null)
        {
            // If name is not defined get is from name property
            var name = config.GetAsNullableString("name") ?? config.GetAsNullableString("id");

            // Or get name from descriptor
            if (name == null)
            {
                var descriptorStr = config.GetAsNullableString("descriptor");
                var descriptor = Descriptor.FromString(descriptorStr);
                name = descriptor != null ? descriptor.Name : null;
            }

            return name ?? defaultName;
        }
    }
}
