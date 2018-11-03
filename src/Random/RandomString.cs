using System;
using System.Text;

namespace PipServices3.Commons.Random
{
    /// <summary>
    /// Random generator for string values.
    /// </summary>
    /// <example>
    /// <code>
    /// var value1 = RandomString.Pick("ABC");     // Possible result: "C"
    /// var value2 = RandomString.Pick(new String {"A","B","C"}); // Possible result: "B"
    /// </code>
    /// </example>
    public class RandomString
    {
        private static readonly string _digits = "01234956789";
        private static readonly string _symbols = "_,.:-/.[].{},#-!,$=%.+^.&*-() ";
        private static readonly string _alphaLower = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string _alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string _alpha = _alphaUpper + _alphaLower;
        private static readonly string _chars = _alpha + _digits + _symbols;

        /// <summary>
        /// Picks a random character from a string.
        /// </summary>
        /// <param name="values">a string to pick a char from</param>
        /// <returns>a randomly picked char.</returns>
        public static char Pick(string values)
        {
            if (values == null || values.Length == 0)
                return '\0';

            int index = RandomInteger.NextInteger(values.Length);
            return values[index];
        }

        /// <summary>
        /// Picks a random character from an array of string..
        /// </summary>
        /// <param name="values">a string to pick from</param>
        /// <returns>a randomly picked string.</returns>
        public static string Pick(String[] values)
        {
            if (values == null || values.Length == 0)
                return "";

            int index = RandomInteger.NextInteger(values.Length);
            return values[index];
        }

        /// <summary>
        /// Distorts a string by randomly replacing characters in it.
        /// </summary>
        /// <param name="value">a string to distort.</param>
        /// <returns>a distored string.</returns>
        public static string Distort(string value)
        {
            value = value.ToLower();

            if (RandomBoolean.Chance(1, 5))
                value = value.Substring(0, 1).ToUpper() + value.Substring(1);

            if (RandomBoolean.Chance(1, 3))
                value = value + Pick(_symbols);

            return value;
        }

        /// <summary>
        /// Generates random alpha characted [A-Za-z]
        /// </summary>
        /// <returns>a random characted.</returns>
        public static char NextAlphaChar()
        {
            int index = RandomInteger.NextInteger(_alpha.Length);
            return _alpha[index];
        }

        /// <summary>
        /// Generates a random string, consisting of upper and lower case letters (of the English alphabet),
        /// digits(0-9), and symbols("_,.:-/.[].{},#-!,$=%.+^.&*-() ").
        /// </summary>
        /// <param name="minLength">(optional) minimum string length.</param>
        /// <param name="maxLength">maximum string length.</param>
        /// <returns>a random string.</returns>
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
