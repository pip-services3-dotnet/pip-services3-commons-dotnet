using System.Collections.Generic;

namespace PipServices.Commons.Random
{
    public static class RandomLong
    {
        private static readonly System.Random _random = new System.Random();

        public static long NextLong(long maxValue)
        {
            return _random.Next((int)maxValue);
        }

        public static long NextLong(long minValue, long maxValue)
        {
            if (maxValue - minValue <= 0)
                return minValue;

            return minValue + _random.Next((int)(maxValue - minValue));
        }

        public static long UpdateLong(long value)
        {
            return UpdateLong(value, 0);
        }

        public static long UpdateLong(long value, long range)
        {
            range = range == 0 ? (long)(0.1 * value) : range;
            long minValue = value - range;
            long maxValue = value + range;
            return NextLong(minValue, maxValue);
        }

        public static List<long> Sequence(long size)
        {
            return Sequence(size, size);
        }

        public static List<long> Sequence(long min, long max)
        {
            long count = NextLong(min, max);

            List<long> result = new List<long>();
            for (long i = 0; i < count; i++)
                result.Add(i);

            return result;
        }
    }
}
