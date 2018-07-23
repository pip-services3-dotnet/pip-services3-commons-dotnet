using System;
using System.Text;

namespace PipServices.Commons.Random
{
    public class RandomString
    {
        private static readonly string _digits = "01234956789";
        private static readonly string _symbols = "_,.:-/.[].{},#-!,$=%.+^.&*-() ";
        private static readonly string _alphaLower = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string _alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string _alpha = _alphaUpper + _alphaLower;
        private static readonly string _chars = _alpha + _digits + _symbols;

        public static char Pick(string values)
        {
            if (values == null || values.Length == 0)
                return '\0';

            int index = RandomInteger.NextInteger(values.Length);
            return values[index];
        }

        public static string Pick(String[] values)
        {
            if (values == null || values.Length == 0)
                return "";

            int index = RandomInteger.NextInteger(values.Length);
            return values[index];
        }

        public static string Distort(string value)
        {
            value = value.ToLower();

            if (RandomBoolean.Chance(1, 5))
                value = value.Substring(0, 1).ToUpper() + value.Substring(1);

            if (RandomBoolean.Chance(1, 3))
                value = value + Pick(_symbols);

            return value;
        }

        public static char NextAlphaChar()
        {
            int index = RandomInteger.NextInteger(_alpha.Length);
            return _alpha[index];
        }

        public static string NextString(int minLength, int maxLength)
        {
            StringBuilder result = new StringBuilder();

            int length = RandomInteger.NextInteger(minLength, maxLength);
            for (int i = 0; i < length; i++)
            {
                int index = RandomInteger.NextInteger(_chars.Length);
                result.Append(_chars[index]);
            }

            return result.ToString();
        }
    }
}
