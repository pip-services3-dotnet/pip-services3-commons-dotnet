namespace PipServices.Commons.Run
{
    /// <summary>
    /// Interface for components that require parameters
    /// </summary>
    public interface IParameterized
    {
        /// <summary>
        /// Sets component configuration parameters
        /// </summary>
        /// <param name="parameters">configuration parameters</param>
        void SetParameters(Parameters parameters);
    }
}
