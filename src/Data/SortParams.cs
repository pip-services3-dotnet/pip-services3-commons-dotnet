using System.Collections.Generic;

namespace PipServices.Commons.Data
{
    public class SortParams : List<SortField>
    {
        public SortParams(IEnumerable<SortField> fields = null)
        {
            if (fields != null)
                AddRange(fields);
        }
    }
}