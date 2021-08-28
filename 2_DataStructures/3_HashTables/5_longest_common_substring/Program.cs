using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace _5_longest_common_substring
{
    public static class LongestCommonSubstring
    {
        const int t9 = 200000; // 1000000000;
        const int p1 = t9 + 3;// 7;
        const int p2 = t9 + 9;// 9;
        private static HTItem[] ht = new HTItem[p1 + p2];

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
                long x = new Random().Next(t9) + 1;
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

                var guid = Guid.NewGuid();

                var s2Substrings = new List<Substring>();
                for (int i = string2.Length; i >= 1; i--)
                {
                    for (int j = 0; j <= string1.Length - i; j++)
                    {
                        var s1p1 = (h1[j + i] - xn1[i] * h1[j]) % p1;
                        if (s1p1 < 0) s1p1 += p1;
                        var s1p2 = (h2[j + i] - xn2[i] * h2[j]) % p2;
                        if (s1p2 < 0) s1p2 += p2;
                        ht[p1 + s1p2] = ht[s1p1] = new HTItem()
                        {
                            Id = guid,
                            Start = j,
                            Length = i
                        };
                        if (j <= string2.Length - i)
                        {
                            var s2p1 = (h1[string1.Length + j + i] - xn1[i] * h1[string1.Length + j]) % p1;
                            if (s2p1 < 0) s2p1 += p1;
                            var s2p2 = (h2[string1.Length + j + i] - xn2[i] * h2[string1.Length + j]) % p2;
                            if (s2p2 < 0) s2p2 += p2;
                            s2Substrings.Add(new Substring(j, i, s2p1, s2p2));
                        }
                    }
                }

                //searching for substrings
                var found = false;
                var s2SubstringArray = s2Substrings.ToArray();
                for (int i = 0; i < s2SubstringArray.Length; i++)
                {
                    var htitem1 = ht[s2SubstringArray[i].Hash1];
                    var htitem2 = ht[p1 + s2SubstringArray[i].Hash2];
                    if (htitem1?.Id == guid && htitem2?.Id == guid && htitem1.Start == htitem2.Start &&
                        htitem1.Length == htitem2.Length)
                    {
                        var index1 = strings[0].Length > strings[1].Length ? htitem1.Start : s2SubstringArray[i].Start;
                        var index2 = strings[0].Length > strings[1].Length ? s2SubstringArray[i].Start : htitem1.Start;
                        result[p] = $"{index1} {index2} {htitem1.Length}";
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    result[p] = "0 0 0";
                }
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
