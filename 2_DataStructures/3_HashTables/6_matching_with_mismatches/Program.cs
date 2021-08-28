using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace _6_matching_with_mismatches
{
    public class Triple
    {
        public int MaxDistance;
        public string Text;
        public string Pattern;
    }

    public class Substring
    {
        public int Start;
        public int DistanceToPattern;
    }

    public static class MatchingWithMismatches
    {
        private const long x = 17;
        const int t9 = 1000000000;
        const int p = t9 + 7;

        private static int CountMismatches(long[] hash, long[] xn, Triple triple, int i, int l, int r)
        {
            var thash = (hash[r + 1] - xn[r - l + 1] * hash[l]) % p;
            if (thash < 0) thash += p;
            var phash = (hash[triple.Text.Length - i + r + 1] - xn[r - l + 1] * hash[triple.Text.Length - i + l]) % p;
            if (phash < 0) phash += p;

            if (thash.Equals(phash)) return 0;
            if (r == l) return 1;
            var mid = l + (r - l) / 2;
            return CountMismatches(hash, xn, triple, i, l, mid) + CountMismatches(hash, xn, triple, i, mid + 1, r);
        }

        public static string[] Solve(string[] input)
        {
            var triples = input.Select(x =>
            {
                var triple = x.Split(' ').ToArray();
                return new Triple()
                {
                    MaxDistance = int.Parse(triple[0]),
                    Text = triple[1],
                    Pattern = triple[2]
                };
            }).ToArray();
            var result = new string[triples.Length];
            for (int t = 0; t < triples.Length; t++)
            {
                var triple = triples[t];
                // preprocessing

                var h = new long[triple.Text.Length + triple.Pattern.Length + 1];
                var xn = new long[triple.Pattern.Length + 1];

                h[0] = 0;
                xn[0] = 1;
                var bytes = Encoding.ASCII.GetBytes(triple.Text + triple.Pattern);
                var pattBytes = Encoding.ASCII.GetBytes(triple.Pattern);
                for (int i = 1; i <= bytes.Length; i++)
                {
                    h[i] = (h[i - 1] * x + bytes[i - 1]) % p;
                    if (i <= triple.Pattern.Length) xn[i] = xn[i - 1] * x % p;
                }

                // count
                var positions = new List<int>();
                for (int i = 0; i <= triple.Text.Length - triple.Pattern.Length; i++)
                {
                    var mismatches = CountMismatches(h, xn, triple, i, i, i + triple.Pattern.Length - 1);
                    if (mismatches <= triple.MaxDistance)
                    {
                        positions.Add(i);
                    }
                }
                positions.Insert(0, positions.Count);
                result[t] = string.Join(" ", positions);
            }
            return result;
        }

        static void Main(string[] args)
        {
            var triples = new List<string>();
            string inLine;
            while ((inLine = Console.ReadLine()) != null)
            {
                triples.Add(inLine);
            }
            foreach (var outLine in Solve(triples.ToArray()))
            {
                Console.WriteLine(outLine);
            }
        }
    }
}
