using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace _5_longest_common_substring
{
    public static class LongestCommonSubstring
    {
        private const int t5 = 100000;
        private const int t9 = 1000000000;
        private const int p1 = t9 + 7;
        private const int p2 = t9 + 9;
        private const long x = 1337;

        public class HTItem
        {
            public Guid Id;
            public int Start;
            public int Length;
        }

        public class Substring
        {
            public long Hash1;
            public long Hash2;
            public int Start;
            public int Length;
            public Substring(int start, int length, long hash1 = 0, long hash2 = 0)
            {
                Hash1 = hash1;
                Hash2 = hash2;
                Start = start;
                Length = length;
            }
        }

        public static string[] Solve(string[] stringPairs)
        {
            var result = new string[stringPairs.Length];
            for (int p = 0; p < stringPairs.Length; p++)
            {
                var strings = stringPairs[p].Split(' ').ToArray();
                if (strings.Length < 2)
                {
                    result[p] = "0 0 0"; continue;
                }
                string string1, string2;
                if (strings[0].Length > strings[1].Length)
                {
                    string1 = strings[0];
                    string2 = strings[1];
                }
                else
                {
                    string1 = strings[1];
                    string2 = strings[0];
                }
                // preprocessing substrings

                var h1 = new long[string1.Length + string2.Length + 1];
                var h2 = new long[string1.Length + string2.Length + 1];
                var xn1 = new long[string2.Length + 1];
                var xn2 = new long[string2.Length + 1];
                h1[0] = h2[0] = 0;
                xn1[0] = xn2[0] = 1;
                var sBytes = Encoding.ASCII.GetBytes($"{string1}{string2}");
                for (int i = 1; i <= sBytes.Length; i++)
                {
                    h1[i] = (h1[i - 1] * x + sBytes[i - 1]) % p1;
                    h2[i] = (h2[i - 1] * x + sBytes[i - 1]) % p2;
                    if (i <= string2.Length)
                    {
                        xn1[i] = xn1[i - 1] * x % p1;
                        xn2[i] = xn2[i - 1] * x % p2;
                    }
                }

                // substring of length len binary search
                var low = 1;
                var high = string2.Length;
                var res = "";
                while (high >= low)
                {
                    var ht = new Dictionary<long, HTItem>();
                    var len = low + (int)Math.Round((double)(high - low) / 2);
                    var guid = Guid.NewGuid();
                    var s2Substrings = new Substring[string2.Length - len + 1];
                    for (int j = 0; j <= string1.Length - len; j++)
                    {
                        var s1p1 = (h1[j + len] - xn1[len] * h1[j]) % p1;
                        if (s1p1 < 0) s1p1 += p1;
                        var s1p2 = (h2[j + len] - xn2[len] * h2[j]) % p2;
                        if (s1p2 < 0) s1p2 += p2;
                        var htItem = new HTItem()
                        {
                            Id = guid,
                            Start = j,
                            Length = len
                        };
                        if (!ht.ContainsKey(s1p1)) ht.Add(s1p1, htItem);
                        if (!ht.ContainsKey(p1 + s1p2)) ht.Add(p1 + s1p2, htItem);
                        if (j <= string2.Length - len)
                        {
                            var s2p1 = (h1[string1.Length + j + len] - xn1[len] * h1[string1.Length + j]) % p1;
                            if (s2p1 < 0) s2p1 += p1;
                            var s2p2 = (h2[string1.Length + j + len] - xn2[len] * h2[string1.Length + j]) % p2;
                            if (s2p2 < 0) s2p2 += p2;
                            s2Substrings[j] = new Substring(j, len, s2p1, s2p2);
                        }
                    }

                    var found = false;
                    var s2SubstringArray = s2Substrings.ToArray();
                    for (int i = 0; i < s2SubstringArray.Length; i++)
                    {
                        if (ht.ContainsKey(s2SubstringArray[i].Hash1) && ht.ContainsKey(p1 + s2SubstringArray[i].Hash2) &&
                            ht[s2SubstringArray[i].Hash1] == ht[p1 + s2SubstringArray[i].Hash2])
                        {
                            var htitem = ht[s2SubstringArray[i].Hash1];
                            var index1 = strings[0].Length > strings[1].Length ? htitem.Start : s2SubstringArray[i].Start;
                            var index2 = strings[0].Length > strings[1].Length ? s2SubstringArray[i].Start : htitem.Start;
                            res = $"{index1} {index2} {htitem.Length}";
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        low = len + 1;
                    else
                        high = len - 1;
                }
                result[p] = res.Length > 0 ? res : "0 0 0";
            }
            return result;
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
