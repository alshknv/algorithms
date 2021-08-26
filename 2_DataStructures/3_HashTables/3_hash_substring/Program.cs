using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace _3_hash_substring
{
    public static class HashSubstring
    {
        private const int x = 17;
        private const int prime = 1000000007;

        private static long Hash(byte[] bytes)
        {
            long hash = 0;
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                hash = (hash * x + bytes[i]) % prime;
            }
            return (int)hash;
        }

        private static long NextHash(long prevHash, byte[] strBytes, int a, int b, int xp)
        {
            long xpn = (long)strBytes[a + b] * xp;
            long xh = x * prevHash;
            return (xh + strBytes[a] - xpn) % prime;
        }

        public static string Solve(string pattern, string str)
        {
            var strBytes = Encoding.ASCII.GetBytes(str);
            var pBytes = Encoding.ASCII.GetBytes(pattern);
            var result = new List<int>();
            // preprocessing
            var phashes = new long[strBytes.Length - pattern.Length + 1];
            phashes[strBytes.Length - pattern.Length] = Hash(strBytes.Skip(str.Length - pattern.Length).Take(pattern.Length).ToArray());
            var xp = 1;
            for (int i = 0; i < pattern.Length; i++)
            {
                xp = (int)((long)xp * x) % prime;
            }
            for (int i = strBytes.Length - pattern.Length - 1; i >= 0; i--)
            {
                phashes[i] = NextHash(phashes[i + 1], strBytes, i, pattern.Length, xp);
            }
            // search
            var patternHash = Hash(pBytes);
            for (int i = 0; i <= strBytes.Length - pBytes.Length; i++)
            {
                if (patternHash == phashes[i])
                {
                    if (str.Substring(i, pattern.Length).Equals(pattern))
                    {
                        result.Add(i);
                    }
                }
            }
            return string.Join(" ", result);
        }

        static void Main(string[] args)
        {
            var p = Console.ReadLine();
            var l = Console.ReadLine();
            Console.WriteLine(Solve(p, l));
        }
    }
}
