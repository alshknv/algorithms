using System.Collections.Generic;
using System;

namespace _6_maximum_number_of_prizes
{
    public class MaximumNumberOfPrizes
    {
        private static List<int> GetPrizeDistribution(int prizesTotal)
        {
            var prizes = new List<int>();
            var currentPrize = 1;
            while (prizesTotal > 0)
            {
                if (prizesTotal == currentPrize || prizesTotal - currentPrize > currentPrize)
                {
                    prizes.Add(currentPrize);
                    prizesTotal -= currentPrize;
                }
                currentPrize++;
            }
            return prizes;
        }

        public static List<string> Solve(string input)
        {
            var prizes = GetPrizeDistribution(int.Parse(input));
            return new List<string>() {
                prizes.Count.ToString(),
                string.Join(" ", prizes)
            };
        }

        static void Main(string[] args)
        {
            foreach (var line in Solve(Console.ReadLine()))
                Console.WriteLine(line);
        }
    }
}
