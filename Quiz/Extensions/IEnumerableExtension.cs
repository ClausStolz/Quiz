using System;
using System.Linq;
using System.Collections.Generic;


namespace Quiz.Extensions
{
    public static class IEnumerableExtension
    {
        public static decimal Median(this IEnumerable<int> values)
        {
            if (values == null || values.Count() == 0)
            {
                throw new Exception("Median of empty array not defined");
            }

            var sortedValues = values.ToArray();
            Array.Sort(sortedValues);

            int size = sortedValues.Length;
            int mid = size / 2;
            decimal median = (size % 2 != 0) ? (decimal)sortedValues[mid] : ((decimal)sortedValues[mid] + (decimal)sortedValues[mid - 1]) / 2;
            return median;
        }
    }
}