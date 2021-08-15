using System.Collections.Generic;
using System.Linq;
using System;

namespace _4_number_of_inversions
{
    public static class NumberOfInversions
    {
        private class InversionCounter
        {
            public long[] Array;
            public int InversionCount = 0;
        }

        private static InversionCounter Merge(InversionCounter left, InversionCounter right)
        {
            var merged = new InversionCounter()
            {
                Array = new long[left.Array.Length + right.Array.Length],
                InversionCount = left.InversionCount + right.InversionCount
            };

            var idx1 = 0; var idx2 = 0;
            while (idx1 < left.Array.Length && idx2 < right.Array.Length)
            {
                if (left.Array[idx1] <= right.Array[idx2])
                {
                    merged.Array[idx1 + idx2] = left.Array[idx1++];
                }
                else
                {
                    merged.InversionCount += left.Array.Length - idx1;
                    merged.Array[idx1 + idx2] = right.Array[idx2++];
                }
            }
            if (idx1 < left.Array.Length)
            {
                for (int i = idx1; i < left.Array.Length; i++)
                    merged.Array[i + idx2] = left.Array[i];
            }
            else if (idx2 < right.Array.Length)
            {
                for (int i = idx2; i < right.Array.Length; i++)
                    merged.Array[idx1 + i] = right.Array[i];
            }
            return merged;
        }

        private static InversionCounter CountInversions(long[] array)
        {
            if (array.Length == 1)
            {
                return new InversionCounter
                {
                    Array = new long[1] { array[0] }
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

        public static string Naive(string input)
        {
            var array = input.Split(' ').Select(x => long.Parse(x)).ToArray();
            var count = 0;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[i]) count++;
                }
            }
            return count.ToString();
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
