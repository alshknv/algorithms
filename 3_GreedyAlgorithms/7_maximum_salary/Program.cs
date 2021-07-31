using System.Linq;
using System.Collections.Generic;
using System;

namespace _7_maximum_salary
{
    public class MaximumSalary
    {
        private static int PartCompare(string part1, string part2)
        {
            return -1 * string.Concat(part1, part2).CompareTo(string.Concat(part2, part1)); // long.Parse(part1 + part2) <= long.Parse(part2 + part1) ? 1 : -1;
        }

        public static string Solve(string input)
        {
            var parts = input.Split(' ').ToList();
            parts.Sort((p1, p2) => PartCompare(p1, p2));
            return string.Join("", parts);
        }

        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
