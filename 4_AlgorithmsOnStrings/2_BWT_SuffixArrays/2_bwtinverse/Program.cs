using System.Linq;
using System.Collections.Generic;
using System;

namespace _2_bwtinverse
{
    public static class BwtInverse
    {
        public static string Solve(string bwt)
        {
            // count occurencies of symbols
            var idx = new SortedDictionary<char, int>();
            for (int i = 0; i < bwt.Length; i++)
            {
                if (idx.ContainsKey(bwt[i]))
                    idx[bwt[i]]++;
                else
                    idx[bwt[i]] = 1;
            }
            // original symbol count for first column reconstruction
            var count = idx.Select(x => x.Value).ToArray();

            var accCount = count.ToArray();
            var chars = idx.Select(x => x.Key).ToArray();
            var countLast = new Dictionary<char, int>() {
                {chars[0], 0}
            };

            // compute indexes where symbol runs begin in first column
            for (int i = 1; i < chars.Length; i++)
            {
                accCount[i] = accCount[i - 1] + accCount[i];
                idx[chars[i]] = accCount[i] - idx[chars[i]];
                countLast.Add(chars[i], 0);
            }
            idx[chars[0]] = 0;

            // build last-first array and first column
            var lastFirst = new int[bwt.Length];
            var firstCol = new char[bwt.Length];
            var charIdx = 0;
            var charCount = 0;
            for (int i = 0; i < bwt.Length; i++)
            {
                countLast[bwt[i]]++;
                charCount++;
                firstCol[i] = chars[charIdx];
                if (charCount == count[charIdx])
                {
                    charCount = 0;
                    charIdx++;
                }
                lastFirst[i] = idx[bwt[i]] + countLast[bwt[i]] - 1;
            }

            // restore original string
            var symb = 0;
            var result = new char[bwt.Length];
            var resI = result.Length - 1;
            while (resI >= 0)
            {
                result[resI--] = firstCol[symb];
                symb = lastFirst[symb];
            }
            return new string(result);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
