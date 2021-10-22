using System.Collections.Generic;
using System;

namespace _2_bwtinverse
{
    public static class BwtInverse
    {
        public static string Solve(string bwt)
        {
            var sortedBwt = bwt.ToCharArray();
            Array.Sort(sortedBwt);
            var countFirst = new Dictionary<char, int>();
            var countLast = new Dictionary<char, int>();
            var firstLast = new Dictionary<string, string>();
            for (int i = 0; i < bwt.Length; i++)
            {
                var firstI = countFirst.ContainsKey(sortedBwt[i]) ? countFirst[sortedBwt[i]] + 1 : 1;
                var lastI = countLast.ContainsKey(bwt[i]) ? countLast[bwt[i]] + 1 : 1;
                firstLast.Add(sortedBwt[i] + firstI.ToString(), bwt[i] + lastI.ToString());
                countFirst[sortedBwt[i]] = firstI;
                countLast[bwt[i]] = lastI;
            }
            var result = "";
            var symb = "$1";
            while (result.Length < bwt.Length)
            {
                result = symb[0] + result;
                symb = firstLast[symb];
            }
            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
