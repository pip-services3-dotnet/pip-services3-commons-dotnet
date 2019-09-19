namespace PipServices3.Commons.Config
{
    /// <summary>
    /// A helper class to parameters from <c>"options"</c> configuration section.
    /// </summary>
    /// <example>
    /// <code>
    /// var config = ConfigParams.FromTuples( ...
    /// "options.param1", "ABC",
    /// "options.param2", 123 );
    /// 
    /// var options = OptionsResolver.Resolve(config, false); // Result: param1=ABC;param2=123
    /// </code>
    /// </example>
    public static class OptionsResolver
    {
        /// <summary>
        /// Resolves an "options" configuration section from component configuration parameters.
        /// </summary>
        /// <param name="config">configuration parameters</param>
        /// <param name="configAsDefault">(optional) When set true the method returns the entire
        /// parameter set when "options" section is not found.
        /// Default: false</param>
        /// <returns>configuration parameters from "options" section</returns>
        public static ConfigParams Resolve(ConfigParams config, bool configAsDefault = true)
        {
            var options = config.GetSection("options");
            if (options.Count == 0)
                options = configAsDefault ? config : null;
            return options;
        }
    }
}
