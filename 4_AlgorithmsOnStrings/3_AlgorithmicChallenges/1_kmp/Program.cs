using System.Collections.Generic;
using System;

namespace _1_kmp
{
    public static class KnuthMorrisPratt
    {
        private static int[] GetPrefixFunction(string text)
        {
            var pf = new int[text.Length];
            var border = 0;
            for (int i = 1; i < text.Length; i++)
            {
                while (border > 0 && text[i] != text[border])
                    border = pf[border - 1];
                if (text[i] == text[border])
                    border++;
                else
                    border = 0;
                pf[i] = border;
            }
            return pf;
        }

        public static string Solve(string pattern, string text)
        {
            var s = pattern + "$" + text;
            var plen = pattern.Length;
            var pf = GetPrefixFunction(s);
            var matches = new List<int>();
            for (int i = plen + 1; i < s.Length; i++)
            {
                if (pf[i] == plen) matches.Add(i - 2 * plen);
            }
            return string.Join(" ", matches);
        }

        static void Main(string[] args)
        {
            var pattern = Console.ReadLine();
            var text = Console.ReadLine();
            Console.WriteLine(Solve(pattern, text));
        }
    }
}
