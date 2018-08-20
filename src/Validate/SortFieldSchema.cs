using PipServices.Commons.Convert;

namespace PipServices.Commons.Validate
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