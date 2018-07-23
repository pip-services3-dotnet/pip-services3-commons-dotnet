using System;

namespace PipServices.Commons.Random
{
    public class RandomBoolean
    {
        private static readonly System.Random _random = new System.Random();

        public static bool Chance(float chances, float maxChances)
        {
            chances = chances >= 0 ? chances : 0;
            maxChances = maxChances >= 0 ? maxChances : 0;
            if (chances == 0 && maxChances == 0)
                return false;

            maxChances = Math.Max(maxChances, chances);
            double start = (maxChances - chances) / 2;
            double end = start + chances;
            double hit = _random.NextDouble() * maxChances;
            return hit >= start && hit <= end;
        }

        public static bool NextBoolean()
        {
            return _random.Next(100) < 50;
        }
    }
}
