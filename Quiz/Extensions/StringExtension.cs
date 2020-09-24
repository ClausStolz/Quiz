using System;
using System.Linq;


namespace Quiz.Extensions
{
    public static class StringExtension
    {
        public static int GetInt32(this string value)
        {
            var result = Convert.ToInt32(new String(value.Where(Char.IsDigit).ToArray()));
            return result;
        }
    }
}