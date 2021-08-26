using System.Linq;
using System.Collections.Generic;
using System;

namespace _5_longest_common_substring
{
    public static class LongestCommonSubstring
    {
        public static string[] Solve(string[] stringPairs)
        {
            foreach (var pair in stringPairs)
            {
                var strings = pair.Split(' ').ToArray();
                var maxSubLength = Math.Min(strings[0].Length, strings[1].Length);
                for (var subLength = 1; subLength < maxSubLength; subLength++)
                {

                }
            }
        }

        static void Main(string[] args)
        {
            var stringPairs = new List<string>();
            string inLine;
            while ((inLine = Console.ReadLine()) != null)
            {
                stringPairs.Add(inLine);
            }
            foreach (var outLine in Solve(stringPairs.ToArray()))
            {
                Console.WriteLine(outLine);
            }
        }
    }
}
