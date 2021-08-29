using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace _3_hash_substring
{
    public static class HashSubstring
    {
        private const int x = 263;
        private const int primes = 3;
        private const int t9 = 1000000000;
        private static readonly int[] p = new int[primes];
        private static int GetPrime(int start)
        {
            var num = start + 1;
            while (true)
            {
                var factors = Math.Sqrt(num);
                var factorFound = false;
                for (var factor = 2; factor <= factors; factor++)
                {
                    if (num % factor == 0)
                    {
                        factorFound = true;
                        break;
                    }
                }
                if (!factorFound) return num;
                num++;
            }
        }

        private static long Hash(byte[] bytes, int prime)
        {
            long hash = 0;
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                hash = (hash * x + bytes[i]) % prime;
            }
            return hash;
        }

        private static long NextHash(long prevHash, byte[] strBytes, int i, int pl, long xp, int prime)
        {
            long xpn = strBytes[i + pl] * xp;
            long xh = x * prevHash;
            var hash = (xh + strBytes[i] - xpn) % prime;
            if (hash < 0) hash += prime;
            return hash;
        }

        private static long Pow(long x, int y, int prime)
        {
            long result = 1;
            x %= prime;
            if (x == 0) return 0;
            while (y > 0)
            {
                if ((y & 1) != 0) result = (result * x) % prime;
                y >>= 1;
                x = (x * x) % prime;
            }
            return result;
        }

        public static string Solve(string pattern, string str)
        {
            var strBytes = Encoding.ASCII.GetBytes(str);
            var pBytes = Encoding.ASCII.GetBytes(pattern);
            var patternHashes = new long[primes];
            var xp = new long[primes];
            var phashes = new long[primes, strBytes.Length - pBytes.Length + 1];
            var lastSubstrBytes = strBytes.Skip(str.Length - pattern.Length).Take(pattern.Length).ToArray();

            var start = t9;
            for (int i = 0; i < primes; i++)
            {
                p[i] = GetPrime(start);
                patternHashes[i] = Hash(pBytes, p[i]);
                xp[i] = Pow(x, pattern.Length, p[i]);
                phashes[i, strBytes.Length - pattern.Length] = Hash(lastSubstrBytes, p[i]);
                start = p[i];
            }

            var result = new int[str.Length - pattern.Length + 1];
            var c = 0;
            for (int i = str.Length - pattern.Length; i >= 0; i--)
            {
                var found = true;
                for (int j = 0; j < primes; j++)
                {
                    if (found && patternHashes[j] != phashes[j, i])
                    {
                        found = false;
                    }
                    if (i > 0)
                    {
                        phashes[j, i - 1] = NextHash(phashes[j, i], strBytes, i - 1, pattern.Length, xp[j], p[j]);
                    }
                }
                if (found)
                {
                    if (str.Substring(i, pattern.Length) == pattern)
                    {
                        result[c++] = i;
                    }
                }
            }
            return string.Join(" ", result.Take(c).Reverse());
        }

        static void Main(string[] args)
        {
            var p = Console.ReadLine();
            var l = Console.ReadLine();
            Console.WriteLine(Solve(p, l));
        }
    }
}
