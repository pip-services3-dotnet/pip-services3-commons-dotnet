using System.Threading.Tasks;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Interface for components that require explicit closure.
    /// 
    /// For components that require opening as well as closing
    /// use IOpenable interface instead.
    /// </summary>
    /// <example>
    /// <code>
    /// class MyConnector: ICloseable 
    /// {
    ///     private object _client = null;
    ///     
    ///     ... // The _client can be lazy created
    ///     
    ///     public void Close(string correlationId)
    ///     {
    ///         if (this._client != null)
    ///         {   
    ///             this._client.Close();
    ///             this._client = null;
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    /// See <see cref="IOpenable"/>, <see cref="Closer"/>
    public interface IClosable
    {
        /// <summary>
        /// Closes component and frees used resources.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        Task CloseAsync(string correlationId);
    }
}
