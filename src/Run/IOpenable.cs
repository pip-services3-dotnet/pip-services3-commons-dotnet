using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Interface for components that require explicit opening
    /// </summary>
    public interface IOpenable: IClosable
    {
        /// <summary>
        /// Checks if component is opened
        /// </summary>
        /// <returns><code>true</code> if component is opened and <false> otherwise.</returns>
        bool IsOpened();

        /// <summary>
        /// Opens component, establishes connections to services
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        Task OpenAsync(string correlationId);
    }
}
