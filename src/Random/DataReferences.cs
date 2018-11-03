using System.Collections;
using System.Collections.Generic;

namespace PipServices3.Commons.Random
{
    public class DataReferences
    {
        private Dictionary<string, IList> _data = new Dictionary<string, IList>();

        public DataReferences() { }

        public IList<T> GetAs<T>()
        {
            foreach (var data in _data.Values)
            {
                if (data is IList<T>)
                    return data as IList<T>;
            }

            return null;
        }

        public IList Get(string name)
        {
            IList data = null;
            _data.TryGetValue(name, out data);
            return data;
        }

        public void Set(string name, IList data)
        {
            _data.Add(name, data);
        }
    }
}
