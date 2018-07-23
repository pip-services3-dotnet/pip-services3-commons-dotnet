using System;

namespace PipServices.Commons.Refer
{
    public class Reference
    {
        private object _locator;
        private object _component;

        public Reference(object locator, object component)
        {
            //if (locator == null)
            //    throw new ArgumentNullException(nameof(locator));
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            _locator = locator;
            _component = component;
        }

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

        public object GetComponent()
        {
            return _component;
        }

        public object GetLocator()
        {
            return _locator;
        }
    }
}