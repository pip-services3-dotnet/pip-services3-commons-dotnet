using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipServices.Commons.Run
{
    /// <summary>
    /// Helper class that triggers execution for components
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// Triggers execution for component that implement IExecutable interface.
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="component">a component to be executed</param>
        /// <param name="args">a set of parameters to pass to executed components</param>
        /// <returns>execution results</returns>
        public static async Task<object> ExecuteOneAsync(string correlationId, object component, Parameters args)
        {
            var executable = component as IExecutable;
            if (executable != null)
                return await executable.ExecuteAsync(correlationId, args);
            else return null;
        }

        /// <summary>
        /// Triggers execution for components that implement IExecutabl interfaces.
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <param name="components">a list of components to be executed</param>
        /// <param name="args">a set of parameters to pass to executed components</param>
        /// <returns>execution results</returns>
        public static async Task<List<object>> ExecuteAsync(string correlationId, IEnumerable components, Parameters args)
        {
            var results = new List<object>();
            if (components == null) return results;

            foreach (var component in components)
            {
                if (component is IExecutable)
                    results.Add(await ExecuteOneAsync(correlationId, component, args));
            }

            return results;
        }
    }
}