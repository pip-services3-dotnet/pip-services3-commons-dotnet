using PipServices.Commons.Convert;
using System.Text.RegularExpressions;

namespace PipServices.Commons.Validate
{
    public class ObjectComparator
    {
        public static bool Compare(object value1, string operation, object value2)
        {
            operation = operation.ToUpperInvariant();

            if (operation == "=" || operation == "==" || operation == "EQ")
                return AreEqual(value1, value2);
            if (operation == "!=" || operation == "<>" || operation == "NE")
                return AreNotEqual(value1, value2);
            if (operation == "<" || operation == "LT")
                return Less(value1, value2);
            if (operation == "<=" || operation == "LE")
                return AreEqual(value1, value2) || Less(value1, value2);
            if (operation == ">" || operation == "GT")
                return More(value1, value2);
            if (operation == ">=" || operation == "GE")
                return AreEqual(value1, value2) || More(value1, value2);
            if (operation == "LIKE")
                return Match(value1, value2);

            return true;
        }

        public static bool AreEqual(object value1, object value2)
        {
            if (value1 == null && value2 == null)
                return true;
            if (value1 == null || value2 == null)
                return false;
            return value1.Equals(value2);
        }

        public static bool AreNotEqual(object value1, object value2)
        {
            return !AreEqual(value1, value2);
        }

        public static bool Less(object value1, object value2)
        {
            var number1 = DoubleConverter.ToNullableDouble(value1);
            var number2 = DoubleConverter.ToNullableDouble(value2);

            if (number1 == null || number2 == null)
                return false;

            return number1 < number2;
        }

        public static bool More(object value1, object value2)
        {
            var number1 = DoubleConverter.ToNullableDouble(value1);
            var number2 = DoubleConverter.ToNullableDouble(value2);

            if (number1 == null || number2 == null)
                return false;

            return number1 > number2;
        }

        public static bool Match(object value1, object value2)
        {
            if (value1 == null && value2 == null)
                return true;
            if (value1 == null || value2 == null)
                return false;

            var string1 = value1.ToString();
            var string2 = value2.ToString();
            return Regex.IsMatch(string1, string2);
        }
    }
}
