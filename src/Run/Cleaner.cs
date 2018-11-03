using System.Collections;
using System.Threading.Tasks;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Helper class that cleans stored object state.
    /// </summary>
    /// See <see cref="ICleanable"/>
    public class Cleaner
    {
        /// <summary>
        /// Clears state of specific component.
        /// To be cleaned state components must implement ICleanable interface. If they
        /// don't the call to this method has no effect.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="component">a component to be cleaned</param>
        /// See <see cref="ICleanable"/>
        public static async Task ClearOneAsync(string correlationId, object component)
        {
            var cleanable = component as ICleanable;
            if (cleanable != null)
                await cleanable.ClearAsync(correlationId);
        }

        /// <summary>
        /// Clears state of multiple components.
        /// To be cleaned state components must implement ICleanable interface. If they
        /// don't the call to this method has no effect.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="components">a list of components to be cleaned</param>
        /// See <see cref="ClearOneAsync(string, object)"/>, <see cref="ICleanable"/>
        public static async Task ClearAsync(string correlationId, IEnumerable components)
        {
            if (components == null) return;

            foreach (var component in components)
                await ClearOneAsync(correlationId, component);
        }
    }
}
