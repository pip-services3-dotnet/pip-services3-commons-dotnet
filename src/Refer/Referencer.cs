using System.Collections;

namespace PipServices.Commons.Refer
{
    /// <summary>
    /// Helper class that assigns references to components
    /// </summary>
    public class Referencer
    {
        /// <summary>
        /// Assigns references to a single components that implements IReferenceable interface
        /// </summary>
        /// <param name="references">references to be assigned</param>
        /// <param name="component">a component to assign references</param>
        public static void SetReferencesForOne(IReferences references, object component)
        {
            var referenceable = component as IReferenceable;
            if (referenceable != null)
                referenceable.SetReferences(references);
        }

        /// <summary>
        /// Assigns references to components that implement IReferenceable interface
        /// </summary>
        /// <param name="references">references to be assigned</param>
        /// <param name="components">a list of components to assign references</param>
        public static void SetReferences(IReferences references, IEnumerable components = null)
        {
            components = components ?? references.GetAll();
            foreach (var component in components)
                SetReferencesForOne(references, component);
        }

        /// <summary>
        /// Clears references for component that implement IUnreferenceable interface 
        /// </summary>
        /// <param name="component">a components to clear references</param>
        public static void UnsetReferencesForOne(object component)
        {
            var unreferenceable = component as IUnreferenceable;
            if (unreferenceable != null)
                unreferenceable.UnsetReferences();
        }

        /// <summary>
        /// Clears references for components that implement IUnreferenceable interface 
        /// </summary>
        /// <param name="components">a list of components to clear references</param>
        public static void UnsetReferences(IEnumerable components)
        {
            foreach (var component in components)
                UnsetReferencesForOne(component);
        }

        /// <summary>
        /// Clears references for components that implement IUnreferenceable interface 
        /// </summary>
        /// <param name="references">a list of components to clear references</param>
        public static void UnsetReferences(IReferences references)
        {
            var components = references.GetAll();
            foreach (var component in components)
                UnsetReferencesForOne(component);
        }

    }
}