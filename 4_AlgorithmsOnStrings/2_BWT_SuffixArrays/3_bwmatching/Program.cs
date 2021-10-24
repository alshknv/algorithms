using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_bwmatching
{
    public static class BwtMatching
    {
        private static int BetterBwtMatching(SortedDictionary<char, int> firstOccurence, string lastCol, string pattern, Dictionary<char, int> chars, int[,] countI)
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
                    top = firstOccurence[symbol] + countI[chars[symbol], top];
                    bottom = firstOccurence[symbol] + countI[chars[symbol], bottom + 1] - 1;
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
            // count occurencies of symbols
            var firstOccurence = new SortedDictionary<char, int>();
            for (int i = 0; i < bwt.Length; i++)
            {
                if (firstOccurence.ContainsKey(bwt[i]))
                    firstOccurence[bwt[i]]++;
                else
                    firstOccurence[bwt[i]] = 1;
            }

            var accCount = firstOccurence.Values.ToArray();
            var firstColChars = firstOccurence.Keys.ToArray();
            var countLast = new Dictionary<char, int>() {
                {firstColChars[0], 0}
            };
            var charIndex = new Dictionary<char, int>() {
                {firstColChars[0], 0}
            };

            // calculate first occurencies
            for (int i = 1; i < firstColChars.Length; i++)
            {
                accCount[i] = accCount[i - 1] + accCount[i];
                firstOccurence[firstColChars[i]] = accCount[i] - firstOccurence[firstColChars[i]];
                countLast.Add(firstColChars[i], 0);
                charIndex.Add(firstColChars[i], i);
            }
            firstOccurence[firstColChars[0]] = 0;

            var countI = new int[firstColChars.Length, bwt.Length + 1];
            for (int i = 0; i < bwt.Length; i++)
            {
                countLast[bwt[i]]++;

                for (int c = 0; c < firstColChars.Length; c++)
                {
                    countI[c, i + 1] = countLast[firstColChars[c]];
                }
            }

            var matches = new int[patterns.Length];
            for (int j = 0; j < patterns.Length; j++)
            {
                matches[j] = BetterBwtMatching(firstOccurence, bwt, patterns[j], charIndex, countI);
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
