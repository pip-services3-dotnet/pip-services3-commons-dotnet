using System;

namespace PipServices.Commons.Data
{
    public class IdGenerator
    {
        private static readonly System.Random _random = new System.Random();

        public static string NextShort()
        {
            string result = "";
            
            for (int i = 10; result.Length < 9 && i >= 0; i--)
                result = ((long)(100000000 + _random.Next() * 899999999)).ToString();

            return result;
        }

        public static string NextLong()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}