using System.Threading.Tasks;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Interface for components that can be called to execute work.
    /// </summary>
    /// <example>
    /// <code>
    /// class EchoComponent: IExecutable 
    /// {
    ///     ...
    ///     public void Execute(string correlationId, Parameters args)
    ///     {
    ///         var result = args.GetAsObject("message");
    ///     }
    /// }
    /// 
    /// var echo = new EchoComponent();
    /// string message = "Test";
    /// echo.Execute("123", Parameters.FromTuples("message", message));
    /// </code>
    /// </example>
    /// See <see cref="Executor"/>, <see cref="INotifiable"/>, <see cref="Parameters"/>
    public interface IExecutable
    {
        /// <summary>
        /// Executes component with arguments and receives execution result.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="args">execution arguments.</param>
        /// <returns>execution result</returns>
        Task<object> ExecuteAsync(string correlationId, Parameters args);
    }
}
