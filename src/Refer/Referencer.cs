using System.Collections;

namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// Helper class that sets and unsets references to components.
    /// </summary>
    /// See <see cref="IReferenceable"/>, <see cref="IUnreferenceable"/>
    public class Referencer
    {
        /// <summary>
        /// Sets references to specific component.
        /// 
        /// To set references components must implement IReferenceable interface. If they
        /// don't the call to this method has no effect.
        /// </summary>
        /// <param name="references">references to be set</param>
        /// <param name="component">a component to set references to</param>
        /// See <see cref="IReferenceable"/>
        public static void SetReferencesForOne(IReferences references, object component)
        {
            var referenceable = component as IReferenceable;
            if (referenceable != null)
                referenceable.SetReferences(references);
        }

        /// <summary>
        /// Sets references to multiple components.
        /// 
        /// To set references components must implement IReferenceable interface. If they
        /// don't the call to this method has no effect.
        /// </summary>
        /// <param name="references">references to be set</param>
        /// <param name="components">a list of components to set references to</param>
        /// See <see cref="IReferenceable"/>
        public static void SetReferences(IReferences references, IEnumerable components = null)
        {
            components = components ?? references.GetAll();
            foreach (var component in components)
                SetReferencesForOne(references, component);
        }

        /// <summary>
        /// Unsets references in specific component.
        /// 
        /// To unset references components must implement IUnreferenceable interface. If
        /// they don't the call to this method has no effect.
        /// </summary>
        /// <param name="component">a components to unset references</param>
        /// See <see cref="IUnreferenceable"/>
        public static void UnsetReferencesForOne(object component)
        {
            var unreferenceable = component as IUnreferenceable;
            if (unreferenceable != null)
                unreferenceable.UnsetReferences();
        }

        /// <summary>
        /// Unsets references in multiple components.
        /// To unset references components must implement IUnreferenceable interface. If
        /// they don't the call to this method has no effect.
        /// </summary>
        /// <param name="components">a list of components to unset references</param>
        public static void UnsetReferences(IEnumerable components)
        {
            foreach (var component in components)
                UnsetReferencesForOne(component);
        }

        /// <summary>
        /// Unsets references in multiple components.
        /// To unset references components must implement IUnreferenceable interface. If
        /// they don't the call to this method has no effect.
        /// </summary>
        /// <param name="components">a list of components to unset references</param>
        public static void UnsetReferences(IReferences references)
        {
            var components = references.GetAll();
            foreach (var component in components)
                UnsetReferencesForOne(component);
        }

    }
}