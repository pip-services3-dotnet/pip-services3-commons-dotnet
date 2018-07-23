using System.Collections.Generic;

namespace PipServices.Commons.Random
{
    public static class RandomInteger
    {
        private static readonly System.Random _random = new System.Random();

        public static int NextInteger(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public static int NextInteger(int minValue, int maxValue)
        {
            if (maxValue - minValue <= 0)
                return minValue;

            return minValue + _random.Next(maxValue - minValue);
        }

        public static int UpdateInteger(int value)
        {
            return UpdateInteger(value, 0);
        }

        public static int UpdateInteger(int value, int range)
        {
            range = range == 0 ? (int)(0.1 * value) : range;
            int minValue = value - range;
            int maxValue = value + range;
            return NextInteger(minValue, maxValue);
        }

        public static List<int> Sequence(int size)
        {
            return Sequence(size, size);
        }

        public static List<int> Sequence(int min, int max)
        {
            int count = NextInteger(min, max);

            List<int> result = new List<int>();
            for (int i = 0; i < count; i++)
                result.Add(i);

            return result;
        }
    }
}
