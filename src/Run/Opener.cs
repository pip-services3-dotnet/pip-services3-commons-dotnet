using System.Collections;
using System.Threading.Tasks;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Helper class that opens components. 
    /// </summary>
    /// See <see cref="IOpenable"/>
    public class Opener
    {
        /// <summary>
        /// Checks if specified component is opened.
        /// 
        /// To be checked components must implement IOpenable interface. If they don't
        /// the call to this method returns true.
        /// </summary>
        /// <param name="component">the component that is to be checked.</param>
        /// <returns>true if component is opened and false otherwise.</returns>
        /// See <see cref="IOpenable"/>
        public static bool IsOpenOne(object component)
        {
            var openable = component as IOpenable;
            if (openable != null)
			    return openable.IsOpen();
		    else
			    return true;
        }

        /// <summary>
        /// Checks if all components are opened.
        /// 
        /// To be checked components must implement IOpenable interface. If they don't
        /// the call to this method returns true.
        /// </summary>
        /// <param name="components">a list of components that are to be checked.</param>
        /// <returns>true if all components are opened and false if at least one component is closed.</returns>
        /// See <see cref="IsOpenOne(object)"/>, <see cref="IOpenable"/>
        public static bool IsOpen(IEnumerable components)
        {
            if (components == null) return true;

            var result = true;
            foreach (var component in components)
                result = result && IsOpenOne(component);

            return result;
        }

        /// <summary>
        /// Opens specific component.
        /// 
        /// To be opened components must implement IOpenable interface. If they don't the
        /// call to this method has no effect.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="component">the component that is to be opened.</param>
        public static async Task OpenOneAsync(string correlationId, object component)
        {
            var openable = component as IOpenable;
            if (openable != null)
                await openable.OpenAsync(correlationId);
        }

        /// <summary>
        /// Opens multiple component.
        /// 
        /// To be opened components must implement IOpenable interface. If they don't the
        /// call to this method has no effect.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="component">the list of components that is to be opened.</param>
        public static async Task OpenAsync(string correlationId, IEnumerable components)
        {
            if (components == null) return;

            foreach (var component in components)
                await OpenOneAsync(correlationId, component);
        }
    }
}
