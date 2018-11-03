using System.Collections;
using System.Threading.Tasks;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Helper class that closes previously opened components.
    /// </summary>
    /// See <see cref="IClosable"/>
    public class Closer
    {
        /// <summary>
        /// Closes specific component.
        /// To be closed components must implement ICloseable interface. If they don't
        /// the call to this method has no effect.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="component">a list of components to be closed</param>
        /// See <see cref="IClosable"/>
        public static async Task CloseOneAsync(string correlationId, object component)
        {
            var closable = component as IClosable;
            if (closable != null)
                await closable.CloseAsync(correlationId);
        }

        /// <summary>
        /// Closes multiple components.
        /// To be closed components must implement ICloseable interface. If they
        /// don't the call to this method has no effect.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="components">a list of components to be closed</param>
        /// See <see cref="CloseOneAsync(string, object)"/>, <see cref="IClosable"/>
        public static async Task CloseAsync(string correlationId, IEnumerable components)
        {
            if (components == null) return;

            foreach (var component in components)
                await CloseOneAsync(correlationId, component);
        }
    }
}
