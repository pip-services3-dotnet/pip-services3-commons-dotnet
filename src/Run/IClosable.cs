using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Interface for components that require explicit closure
    /// </summary>
    public interface IClosable
    {
        /// <summary>
        /// Closes component, disconnects it from services, disposes resources
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <returns></returns>
        Task CloseAsync(string correlationId);
    }
}
