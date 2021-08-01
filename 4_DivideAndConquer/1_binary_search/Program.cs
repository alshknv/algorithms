using System.Linq;
using System.Collections.Generic;
using System;

namespace _1_binary_search
{
    public static class BinarySearch
    {
        public static long Search(long[] array, long value)
        {
            long min = 0, max = array.Length - 1;
            while (max >= min)
            {
                var mid = (long)Math.Floor((decimal)(max + min) / 2);
                if (array[mid] == value) return mid;
                if (array[mid] > value)
                    max = mid - 1;
                else
                    min = mid + 1;
            }
            if (max < min) return -1;
            return array[max];
        }
        public static string Solve(string lineA, string lineB)
        {
            var array = lineA.Split(' ').Skip(1).Select(x => long.Parse(x)).ToArray();
            var results = lineB.Split(' ').Skip(1).Select(x => Search(array, long.Parse(x)).ToString());
            return string.Join(" ", results);
        }
        static void Main(string[] args)
        {
            var lineA = Console.ReadLine();
            var lineB = Console.ReadLine();
            Console.WriteLine(Solve(lineA, lineB));
        }
    }
}
