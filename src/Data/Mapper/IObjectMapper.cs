using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipServices.Commons.Data.Mapper
{
    public interface IObjectMapper
    {
        TD Transfer<TS, TD>(TS source)
            where TS : class
            where TD : class;
    }
}
