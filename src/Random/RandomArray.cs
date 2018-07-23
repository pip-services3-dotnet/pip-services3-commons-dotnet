using System.Collections.Generic;

namespace PipServices.Commons.Random
{
    public class RandomArray
    {
        public static T Pick<T>(T[] values)
        {
            if (values == null || values.Length == 0)
                return default(T);

            return values[RandomInteger.NextInteger(values.Length)];
        }

        public static T Pick<T>(List<T> values)
        {
            if (values == null || values.Count == 0)
                return default(T);

            int index = RandomInteger.NextInteger(values.Count);
            return values[index];
        }
    }
}
