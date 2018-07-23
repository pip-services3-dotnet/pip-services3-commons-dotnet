using PipServices.Commons.Refer;

namespace PipServices.Commons.Config
{
    public static class NameResolver
    {
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
