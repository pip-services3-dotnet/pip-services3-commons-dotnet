using System.Collections;
using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Helper class that opens a collection of components 
    /// </summary>
    public class Opener
    {
        /// <summary>
        /// Checks if component that implement IOpenable interface is opened
        /// </summary>
        /// <param name="component">a component to be checked</param>
        /// <returns></returns>
        public static bool IsOpenedOne(object component)
        {
            var openable = component as IOpenable;
            if (openable != null)
			    return openable.IsOpened();
		    else
			    return true;
        }

        /// <summary>
        /// if components that implement IOpenable interface are opened
        /// </summary>
        /// <param name="components">a list of components to be checked</param>
        /// <returns></returns>
        public static bool IsOpened(IEnumerable components)
        {
            if (components == null) return true;

            var result = true;
            foreach (var component in components)
                result = result && IsOpenedOne(component);

            return result;
        }

        /// <summary>
        /// Opens a component that implements IOpenable interface
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="component">a components to be opened</param>
        public static async Task OpenOneAsync(string correlationId, object component)
        {
            var openable = component as IOpenable;
            if (openable != null)
                await openable.OpenAsync(correlationId);
        }

        /// <summary>
        /// Opens component that implement IOpenable interface
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="components">a list of components to be opened</param>
        public static async Task OpenAsync(string correlationId, IEnumerable components)
        {
            if (components == null) return;

            foreach (var component in components)
                await OpenOneAsync(correlationId, component);
        }
    }
}
