using System.Text;
using System.Linq;
using System;

namespace _4_substring_equality
{
    public static class SubstringEquality
    {
        public static string[] Solve(string str, string[] queries)
        {
            const int t9 = 1000000000;
            const int p1 = t9 + 7;
            var h1 = new long[str.Length + 1];
            var xn1 = new long[str.Length + 1];
            const int p2 = t9 + 9;
            var h2 = new long[str.Length + 1];
            var xn2 = new long[str.Length + 1];
            // preprocessing
            long x = new Random().Next(t9);
            h1[0] = h2[0] = 0;
            xn1[0] = xn2[0] = 1;
            var bytes = Encoding.ASCII.GetBytes(str);
            for (int i = 1; i <= bytes.Length; i++)
            {
                h1[i] = (h1[i - 1] * x + bytes[i - 1]) % p1;
                h2[i] = (h2[i - 1] * x + bytes[i - 1]) % p2;
                xn1[i] = xn1[i - 1] * x % p1;
                xn2[i] = xn2[i - 1] * x % p2;
            }

            // queries
            var result = new string[queries.Length];
            for (int i = 0; i < queries.Length; i++)
            {
                var qParams = queries[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                result[i] =
                    (h1[qParams[0] + qParams[2]] % p1 - xn1[qParams[2]] * h1[qParams[0]]) % p1 ==
                    (h1[qParams[1] + qParams[2]] % p1 - xn1[qParams[2]] * h1[qParams[1]]) % p1 &&
                    (h2[qParams[0] + qParams[2]] % p2 - xn2[qParams[2]] * h2[qParams[0]]) % p2 ==
                    (h2[qParams[1] + qParams[2]] % p2 - xn2[qParams[2]] * h2[qParams[1]]) % p2
                ? "Yes" : "No";
            }
            return result;
        }

        static void Main(string[] args)
        {
            var str = Console.ReadLine();
            var n = int.Parse(Console.ReadLine());
            var queries = new string[n];
            for (int i = 0; i < n; i++)
            {
                queries[i] = Console.ReadLine();
            }
            foreach (var line in Solve(str, queries))
            {
                Console.WriteLine(line);
            }
        }
    }
}
