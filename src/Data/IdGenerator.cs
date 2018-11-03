using System;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Helper class to generate unique object IDs.
    /// It supports two types of IDs: long and short.
    /// 
    /// Long IDs are string GUIDs.They are globally unique and 32-character long.
    /// 
    /// ShortIDs are just 9-digit random numbers.They are not guaranteed be unique.
    /// </summary>
    /// <example>
    /// <code>
    /// IdGenerator.NextLong();      // Possible result: "234ab342c56a2b49c2ab42bf23ff991ac"
    /// IdGenerator.NextShort();     // Possible result: "23495247"
    /// </code>
    /// </example>
    public class IdGenerator
    {
        private static readonly System.Random _random = new System.Random();

        /// <summary>
        /// Generates a random 9-digit random ID (code).
        /// 
        /// Remember: The returned value is not guaranteed to be unique.
        /// </summary>
        /// <returns>a generated random 9-digit code</returns>
        public static string NextShort()
        {
            string result = "";
            
            for (int i = 10; result.Length < 9 && i >= 0; i--)
                result = ((long)(100000000 + _random.Next() * 899999999)).ToString();

            return result;
        }

        /// <summary>
        /// Generates a globally unique 32-digit object ID. The value is a string representation of a GUID value.
        /// </summary>
        /// <returns>a generated 32-digit object ID</returns>
        public static string NextLong()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}