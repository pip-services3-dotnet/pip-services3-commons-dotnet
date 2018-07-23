namespace PipServices.Commons.Random
{
    public class RandomFloat
    {
        private static readonly System.Random _random = new System.Random();

        public static float NextFloat(int maxValue)
        {
            return (float)_random.NextDouble() * maxValue;
        }

        public static float NextFloat(float minValue, float maxValue)
        {
            return (float)(minValue + _random.NextDouble() * (maxValue - minValue));
        }

        public static float UpdateFloat(float value)
        {
            return UpdateFloat(value, 0);
        }

        public static float UpdateFloat(float value, float range)
        {
            range = range == 0 ? (float)(0.1 * value) : range;
            float minValue = value - range;
            float maxValue = value + range;
            return NextFloat(minValue, maxValue);
        }
    }
}
