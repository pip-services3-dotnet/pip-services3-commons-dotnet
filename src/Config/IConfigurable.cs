namespace PipServices.Commons.Config
{
    /// <summary>
    /// Interface for components that require configuration.
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        /// Sets the components configuration.
        /// </summary>
        /// <param name="config">Configuration parameters.</param>
        void Configure(ConfigParams config);
    }
}