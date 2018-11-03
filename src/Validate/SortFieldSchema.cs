using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Validate
{
    public class SortFieldSchema : ObjectSchema
    {
        public SortFieldSchema()
        {
            WithOptionalProperty("name", TypeCode.String);
            WithOptionalProperty("ascending", TypeCode.Boolean);
        }
    }
}