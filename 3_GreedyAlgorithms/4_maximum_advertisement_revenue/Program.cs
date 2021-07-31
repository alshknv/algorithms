using System.Linq;
using System.Collections.Generic;
using System;

namespace _4_maximum_advertisement_revenue
{
    public class MaximumAdvertisementRevenue
    {
        private static long CalcRevenue(int count, List<long> ads, List<long> slots)
        {
            ads.Sort((a1, a2) => -1 * a1.CompareTo(a2));
            slots.Sort((s1, s2) => -1 * s1.CompareTo(s2));
            long revenue = 0;
            for (int i = 0; i < count; i++)
            {
                revenue += (long)(ads[i] * slots[i]);
            }
            return revenue;
        }
        public static string Solve(List<string> input)
        {
            var count = int.Parse(input[0]);
            var ads = input[1].Split(' ').Select(a => long.Parse(a)).ToList();
            var slots = input[2].Split(' ').Select(s => long.Parse(s)).ToList();
            return CalcRevenue(count, ads, slots).ToString();
        }

        static void Main(string[] args)
        {
            var lines = new List<string>() {
                Console.ReadLine(),
                Console.ReadLine(),
                Console.ReadLine()
            };
            Console.WriteLine(Solve(lines));
        }
    }
}
