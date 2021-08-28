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
            return hash;
        }

        private static long NextHash(long prevHash, byte[] strBytes, int i, int pl, long xp)
        {
            long xpn = strBytes[i + pl] * xp;
            long xh = x * prevHash;
            return (xh + strBytes[i] - xpn + prime) % prime;
        }

        public static string Solve(string pattern, string str)
        {
            var strBytes = Encoding.ASCII.GetBytes(str);
            var pBytes = Encoding.ASCII.GetBytes(pattern);
            var result = new List<int>();
            // preprocessing
            var phashes = new long[strBytes.Length - pattern.Length + 1];
            phashes[strBytes.Length - pattern.Length] = Hash(strBytes.Skip(str.Length - pattern.Length).Take(pattern.Length).ToArray());
            long xp = 1;
            for (int i = 0; i < pattern.Length; i++)
            {
                xp = (xp * x) % prime;
            }
            var patternHash = Hash(pBytes);
            for (int i = strBytes.Length - pattern.Length; i >= 0; i--)
            {
                if (i > 0)
                {
                    phashes[i - 1] = NextHash(phashes[i], strBytes, i - 1, pattern.Length, xp);
                }
                if (patternHash == phashes[i])
                {
                    result.Add(i);
                }
            }
            result.Reverse();
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
