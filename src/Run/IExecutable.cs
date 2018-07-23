using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Interface for components that support parameterized execution that returns a result
    /// </summary>
    public interface IExecutable
    {
        /// <summary>
        /// Executes a unit of work with given parameters
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="args">a set of parameters for execution</param>
        Task<object> ExecuteAsync(string correlationId, Parameters args);
    }
}
