using System.Collections;
using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Helper class that closes components
    /// </summary>
    public class Closer
    {
        /// <summary>
        /// Closes component that implement ICloseable interface
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="component">a list of components to be closed</param>
        /// <returns></returns>
        public static async Task CloseOneAsync(string correlationId, object component)
        {
            var closable = component as IClosable;
            if (closable != null)
                await closable.CloseAsync(correlationId);
        }

        /// <summary>
        /// Closes components that implement ICloseable interface
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="components">a list of components to be closed</param>
        /// <returns></returns>
        public static async Task CloseAsync(string correlationId, IEnumerable components)
        {
            if (components == null) return;

            foreach (var component in components)
                await CloseOneAsync(correlationId, component);
        }
    }
}
