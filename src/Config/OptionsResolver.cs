namespace PipServices.Commons.Config
{
    public static class OptionsResolver
    {
        public static ConfigParams Resolve(ConfigParams config, bool configAsDefault = true)
        {
            var options = config.GetSection("options");
            if (options.Count == 0)
                options = config;
            return options;
        }
    }
}
