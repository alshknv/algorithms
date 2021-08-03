using System.Collections.Generic;
using System.Linq;
using System;

namespace _2_majority_element
{
    public static class MajorityElement
    {
        private class ElementCountInfo
        {
            public long Value;
            public long Count;
            public ElementCountInfo(long value, long count)
            {
                Value = value; Count = count;
            }
        }

        private static List<ElementCountInfo> Merge(List<ElementCountInfo> left, List<ElementCountInfo> right)
        {
            var mergedList = new List<ElementCountInfo>();
            while (left.Count > 0 && right.Count > 0)
            {
                if (left[0].Value == right[0].Value)
                {
                    mergedList.Add(new ElementCountInfo(left[0].Value, left[0].Count + right[0].Count));
                    left.RemoveAt(0);
                    right.RemoveAt(0);
                }
                else if (left[0].Value > right[0].Value)
                {
                    mergedList.Add(left[0]);
                    left.RemoveAt(0);
                }
                else
                {
                    mergedList.Add(right[0]);
                    right.RemoveAt(0);
                }
            }
            if (left.Count > 0)
            {
                mergedList.AddRange(left);
            }
            else if (right.Count > 0)
            {
                mergedList.AddRange(right);
            }
            return mergedList;
        }

        private static List<ElementCountInfo> DivideAndCount(long[] array)
        {
            if (array.Length == 1)
            {
                return new List<ElementCountInfo>() {
                    new ElementCountInfo(array[0], 1)
                };
            }
            else
            {
                var mid = array.Length / 2;
                var left = DivideAndCount(array.Take(mid).ToArray());
                var right = DivideAndCount(array.Skip(mid).ToArray());
                return Merge(left, right);
            }
        }

        private static string HasMajority(string[] array)
        {
            var countResult = DivideAndCount(array.Select(x => long.Parse(x)).ToArray());
            for (int i = 0; i < countResult.Count; i++)
            {
                if (countResult[i].Count > array.Length / 2) return "1";
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
