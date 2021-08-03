using System.Collections.Generic;
using System.Linq;
using System;

namespace _4_number_of_inversions
{
    public static class NumberOfInversions
    {
        private class InversionCounter : List<long>
        {
            public int InversionCount = 0;
        }

        private static InversionCounter Merge(InversionCounter left, InversionCounter right)
        {
            var mergedList = new InversionCounter()
            {
                InversionCount = left.InversionCount + right.InversionCount
            };
            var fromRight = 0;
            while (left.Count > 0 && right.Count > 0)
            {
                if (left[0] <= right[0])
                {
                    mergedList.Add(left[0]);
                    left.RemoveAt(0);
                    mergedList.InversionCount += fromRight;
                }
                else
                {
                    fromRight++;
                    mergedList.Add(right[0]);
                    right.RemoveAt(0);
                }
            }
            if (left.Count > 0)
            {
                mergedList.AddRange(left);
                mergedList.InversionCount += left.Count;
            }
            else if (right.Count > 0)
            {
                mergedList.AddRange(right);
            }
            return mergedList;
        }

        private static InversionCounter CountInversions(long[] array)
        {
            if (array.Length == 1)
            {
                return new InversionCounter() {
                     array[0]
                 };
            }
            else
            {
                var mid = array.Length / 2;
                var left = CountInversions(array.Take(mid).ToArray());
                var right = CountInversions(array.Skip(mid).ToArray());
                return Merge(left, right);
            }
        }

        public static string Solve(string input)
        {
            var array = input.Split(' ').Select(x => long.Parse(x)).ToArray();
            return CountInversions(array).InversionCount.ToString();
        }
        static void Main(string[] args)
        {
            Console.ReadLine();
            var input = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
