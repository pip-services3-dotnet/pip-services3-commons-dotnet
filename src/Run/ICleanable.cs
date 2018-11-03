using System.Threading.Tasks;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Interface for components that should clean their state.
    /// 
    /// Cleaning state most often is used during testing.
    /// But there may be situations when it can be done in production.
    /// </summary>
    /// <example>
    /// <code>
    /// class MyObjectWithState: ICleanable 
    /// {
    ///     var _state = new Object[]{};
    ///     ...
    ///     public void Clear(string correlationId)
    ///     {
    ///         this._state = new Object[] { };
    ///     }
    /// }
    /// </code>
    /// </example>
    public interface ICleanable
    {
        /// <summary>
        /// Clears component state.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        Task ClearAsync(string correlationId);
    }
}
