using System.Collections;
using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Helper class that cleans components
    /// </summary>
    public class Cleaner
    {
        /// <summary>
        /// Cleans component that implement ICleanable interface
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="component">a component to be cleaned</param>
        /// <returns></returns>
        public static async Task ClearOneAsync(string correlationId, object component)
        {
            var cleanable = component as ICleanable;
            if (cleanable != null)
                await cleanable.ClearAsync(correlationId);
        }

        /// <summary>
        /// Cleans components that implement ICleanable interface
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="components">a list of components to be cleaned</param>
        /// <returns></returns>
        public static async Task ClearAsync(string correlationId, IEnumerable components)
        {
            if (components == null) return;

            foreach (var component in components)
                await ClearOneAsync(correlationId, component);
        }
    }
}
