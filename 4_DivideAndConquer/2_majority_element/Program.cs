using System.Collections.Generic;
using System.Linq;
using System;

namespace _2_majority_element
{
    public static class MajorityElement
    {
        private static long[] Merge(long[] left, long[] right)
        {
            var mergedArray = new long[left.Length + right.Length];
            var idx1 = 0; var idx2 = 0;
            while (idx1 < left.Length && idx2 < right.Length)
            {
                if (left[idx1] < right[idx2])
                {
                    mergedArray[idx1 + idx2] = left[idx1++];
                }
                else
                {
                    mergedArray[idx1 + idx2] = right[idx2++];
                }
            }
            if (idx1 < left.Length)
            {
                for (int i = idx1; i < left.Length; i++)
                    mergedArray[i + idx2] = left[i];
            }
            else if (idx2 < right.Length)
            {
                for (int i = idx2; i < right.Length; i++)
                    mergedArray[idx1 + i] = right[i];
            }
            return mergedArray;
        }

        private static long[] MergeSort(long[] array)
        {
            if (array.Length == 1)
            {
                return new long[1] { array[0] };
            }
            else
            {
                var mid = array.Length / 2;
                var left = MergeSort(array.Take(mid).ToArray());
                var right = MergeSort(array.Skip(mid).ToArray());
                return Merge(left, right);
            }
        }

        private static string HasMajority(string[] array)
        {
            var sortedArray = MergeSort(array.Select(x => long.Parse(x)).ToArray());
            if (sortedArray.Length < 2) return "1";
            var count = 1;
            var currentValue = sortedArray[0];
            for (int i = 1; i < sortedArray.Length; i++)
            {
                if (sortedArray[i] == currentValue)
                {
                    count++;
                }
                else
                {
                    count = 1;
                    currentValue = sortedArray[i];
                }
                if (count > sortedArray.Length / 2) return "1";
            }
            return "0";
        }

        public static string Solve(string inputString)
        {
            return HasMajority(inputString.Split(' '));
        }

        static void Main(string[] args)
        {
            Console.ReadLine();
            var input = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
