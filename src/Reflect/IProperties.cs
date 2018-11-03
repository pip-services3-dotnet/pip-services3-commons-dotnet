using System.Collections.Generic;

namespace PipServices3.Commons.Reflect
{
    public interface IProperties
    {
        List<string> GetPropertyNames();
        object GetProperty(string name);
        void SetProperty(string name, object value);
    }
}
