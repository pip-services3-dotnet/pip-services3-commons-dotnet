namespace PipServices.Commons.Random
{
    public class RandomDouble
    {
        private static readonly System.Random _random = new System.Random();

        public static double NextDouble(double maxValue)
        {
            return _random.NextDouble() * maxValue;
        }

        public static double NextDouble(double minValue, double maxValue)
        {
            return minValue + _random.NextDouble() * (maxValue - minValue);
        }

        public static double UpdateDouble(double value)
        {
            return UpdateDouble(value, 0);
        }

        public static double UpdateDouble(double value, double range)
        {
            range = range == 0 ? 0.1 * value : range;
            double minValue = value - range;
            double maxValue = value + range;
            return NextDouble(minValue, maxValue);
        }
    }
}
