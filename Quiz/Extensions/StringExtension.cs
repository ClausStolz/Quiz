using System;
using System.Linq;


namespace Quiz.Extensions
{
    public static class StringExtension
    {
        public static int GetInt32(this string value)
        {
            var cleanedValue = value.Replace("\0", string.Empty); 
            var result = Convert.ToInt32(new String(cleanedValue.Where(Char.IsDigit).ToArray()));
            return result;
        }
    }
}