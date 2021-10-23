using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_bwmatching
{
    public static class BwtMatching
    {
        private static int BetterBwtMatching(Dictionary<char, int> firstOccurence, string lastCol, string pattern, Dictionary<Tuple<char, int>, int> countI)
        {
            var top = 0;
            var bottom = lastCol.Length - 1;
            var plast = pattern.Length - 1;
            while (top <= bottom)
            {
                if (plast >= 0)
                {
                    var symbol = pattern[plast--];
                    if (!firstOccurence.ContainsKey(symbol)) return 0;
                    top = firstOccurence[symbol] + countI[new Tuple<char, int>(symbol, top)];
                    bottom = firstOccurence[symbol] + countI[new Tuple<char, int>(symbol, bottom + 1)] - 1;
                }
                else
                {
                    return bottom - top + 1;
                }
            }
            return 0;
        }

        public static string Solve(string bwt, string[] patterns)
        {
            var firstCol = bwt.ToCharArray();
            Array.Sort(firstCol);
            var firstOccurence = new Dictionary<char, int>();
            var count = new Dictionary<char, int>();
            var countI = new Dictionary<Tuple<char, int>, int>();
            for (int i = 0; i < bwt.Length; i++)
            {
                if (!firstOccurence.ContainsKey(firstCol[i])) firstOccurence[firstCol[i]] = i;
                if (count.ContainsKey(bwt[i]))
                {
                    count[bwt[i]]++;
                }
                else
                {
                    count[bwt[i]] = 1;
                }
                foreach (var ch in count.Keys)
                {
                    countI[new Tuple<char, int>(ch, i + 1)] = count[ch];
                }
            }

            for (int i = 0; i < bwt.Length; i++)
            {
                foreach (var ch in count.Keys)
                {
                    var tuple = new Tuple<char, int>(ch, i);
                    if (!countI.ContainsKey(tuple)) countI[tuple] = 0;
                }
            }

            var matches = new int[patterns.Length];
            for (int j = 0; j < patterns.Length; j++)
            {
                matches[j] = BetterBwtMatching(firstOccurence, bwt, patterns[j], countI);
            }
            return string.Join(" ", matches);
        }

        static void Main(string[] args)
        {
            var bwt = Console.ReadLine();
            Console.ReadLine();
            var patterns = Console.ReadLine();
            Console.WriteLine(Solve(bwt, patterns.Split(' ').ToArray()));
        }
    }
}
