using System;

namespace PipServices3.Commons.Refer
{
    /// <summary>
    /// Contains a reference to a component and locator to find it.
    /// It is used by References to store registered component references.
    /// </summary>
    public class Reference
    {
        private object _locator;
        private object _component;

        /// <summary>
        /// Create a new instance of the reference object and assigns its values.
        /// </summary>
        /// <param name="locator">a locator to find the reference.</param>
        /// <param name="component">a reference to component.</param>
        public Reference(object locator, object component)
        {
            //if (locator == null)
            //    throw new ArgumentNullException(nameof(locator));
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            _locator = locator;
            _component = component;
        }

        /// <summary>
        /// Matches locator to this reference locator.
        /// 
        /// Descriptors are matched using equal method.All other locator types are
        /// matched using direct comparison.
        /// </summary>
        /// <param name="locator">the locator to match.</param>
        /// <returns>true if locators are matching and false it they don't.</returns>
        public bool Match(object locator)
        {
            if (_component.Equals(locator))
                return true;

            if (locator is Type)
                return ((Type)_locator).Equals(_component.GetType());

            // Locate by direct locator matching
            if (_locator != null)
                return _locator.Equals(locator);

            return false;
        }

        /// <summary>
        /// Gets the stored component reference.
        /// </summary>
        /// <returns>the component's references.</returns>
        public object GetComponent()
        {
            return _component;
        }

        /// <summary>
        /// Gets the stored component locator.
        /// </summary>
        /// <returns>the component's locator.</returns>
        public object GetLocator()
        {
            return _locator;
        }
    }
}