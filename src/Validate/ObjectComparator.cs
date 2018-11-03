using PipServices3.Commons.Convert;
using System.Text.RegularExpressions;

namespace PipServices3.Commons.Validate
{
    /// <summary>
    /// Helper class to perform comparison operations over arbitrary values.
    /// </summary>
    /// <example>
    /// <code>
    /// ObjectComparator.Compare(2, "GT", 1);        // Result: true
    /// ObjectComparator.AreEqual("A", "B");         // Result: false
    /// </code>
    /// </example>
    public class ObjectComparator
    {
        /// <summary>
        /// Perform comparison operation over two arguments. The operation can be
        /// performed over values of any type.
        /// </summary>
        /// <param name="value1">the first argument to compare</param>
        /// <param name="operation">the comparison operation</param>
        /// <param name="value2">the second argument to compare</param>
        /// <returns>result of the comparison operation</returns>
        public static bool Compare(object value1, string operation, object value2)
        {
            operation = operation.ToUpperInvariant();

            if (operation == "=" || operation == "==" || operation == "EQ")
                return AreEqual(value1, value2);
            if (operation == "!=" || operation == "<>" || operation == "NE")
                return AreNotEqual(value1, value2);
            if (operation == "<" || operation == "LT")
                return IsLess(value1, value2);
            if (operation == "<=" || operation == "LE")
                return AreEqual(value1, value2) || IsLess(value1, value2);
            if (operation == ">" || operation == "GT")
                return IsGreater(value1, value2);
            if (operation == ">=" || operation == "GE")
                return AreEqual(value1, value2) || IsGreater(value1, value2);
            if (operation == "LIKE")
                return Match(value1, value2);

            return true;
        }

        /// <summary>
        /// Checks if two values are equal. The operation can be performed over values of any type.
        /// </summary>
        /// <param name="value1">the first value to compare</param>
        /// <param name="value2">the second value to compare</param>
        /// <returns>true if values are equal and false otherwise</returns>
        public static bool AreEqual(object value1, object value2)
        {
            if (value1 == null && value2 == null)
                return true;
            if (value1 == null || value2 == null)
                return false;
            return value1.Equals(value2);
        }

        /// <summary>
        /// Checks if two values are NOT equal The operation can be performed over values of any type.
        /// </summary>
        /// <param name="value1">the first value to compare</param>
        /// <param name="value2">the second value to compare</param>
        /// <returns>true if values are NOT equal and false otherwise</returns>
        public static bool AreNotEqual(object value1, object value2)
        {
            return !AreEqual(value1, value2);
        }

        /// <summary>
        /// Checks if first value is less than the second one. The operation can be
        /// performed over numbers or strings.
        /// </summary>
        /// <param name="value1">the first value to compare</param>
        /// <param name="value2">the second value to compare</param>
        /// <returns>true if the first value is less than second and false otherwise.</returns>
        public static bool IsLess(object value1, object value2)
        {
            var number1 = DoubleConverter.ToNullableDouble(value1);
            var number2 = DoubleConverter.ToNullableDouble(value2);

            if (number1 == null || number2 == null)
                return false;

            return number1 < number2;
        }

        /// <summary>
        /// Checks if first value is greater than the second one. The operation can be
        /// performed over numbers or strings.
        /// </summary>
        /// <param name="value1">the first value to compare</param>
        /// <param name="value2">the second value to compare</param>
        /// <returns>true if the first value is greater than second and false otherwise.</returns>
        public static bool IsGreater(object value1, object value2)
        {
            var number1 = DoubleConverter.ToNullableDouble(value1);
            var number2 = DoubleConverter.ToNullableDouble(value2);

            if (number1 == null || number2 == null)
                return false;

            return number1 > number2;
        }

        /// <summary>
        /// Checks if string matches a regular expression
        /// </summary>
        /// <param name="value1">a string value to match</param>
        /// <param name="value2">a regular expression string</param>
        /// <returns>true if the value matches regular expression and false otherwise.</returns>
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
