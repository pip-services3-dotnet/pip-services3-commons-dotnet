using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Interface for components that should clean their state.
    /// 
    /// Cleaning state most often is used during testing.
    /// But there may be situations when it can be done in production.
    /// </summary>
    public interface ICleanable
    {
        Task ClearAsync(string correlationId);
    }
}
