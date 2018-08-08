using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PipServices.Commons.Data
{
    [DataContract]
    public class DataPage<T>
    {
        public DataPage() { }

        public DataPage(List<T> data, long? total = null)
        {
            Data = data;
            Total = total;
        }

        [DataMember(Name = "total")]
        public long? Total { get; set; }

        [DataMember(Name = "data")]
        public List<T> Data { get; set; }
    }
}