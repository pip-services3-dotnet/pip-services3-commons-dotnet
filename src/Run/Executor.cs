using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Helper class that executes components.
    /// </summary>
    /// See <see cref="IExecutable"/>
    public class Executor
    {
        /// <summary>
        /// Executes specific component.
        /// To be executed components must implement IExecutable interface. If they don't
        /// the call to this method has no effect.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="component">a component to be executed</param>
        /// <param name="args">execution arguments.</param>
        /// <returns>execution results</returns>
        /// See <see cref="IExecutable"/>, <see cref="Parameters"/>
        public static async Task<object> ExecuteOneAsync(string correlationId, object component, Parameters args)
        {
            var executable = component as IExecutable;
            if (executable != null)
                return await executable.ExecuteAsync(correlationId, args);
            else return null;
        }

        /// <summary>
        /// Executes multiple components.
        /// To be executed components must implement IExecutable interface. If they don't
        /// the call to this method has no effect.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="component">a component to be executed</param>
        /// <param name="args">execution arguments.</param>
        /// <returns>execution results</returns>
        /// See <see cref="ExecuteOneAsync(string, object, Parameters)"/>
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