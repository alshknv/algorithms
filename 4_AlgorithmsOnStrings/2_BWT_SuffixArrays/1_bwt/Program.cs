using System;
using System.Linq;

namespace _1_bwt
{
    public static class BurrowsWheelerTransform
    {
        public static string Solve(string text)
        {
            var bwt = new string[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                bwt[i] = text.Substring(i, text.Length - i) + (i > 0 ? text.Substring(0, i) : "");
            }
            Array.Sort(bwt);
            return new string(bwt.Select(x => x[x.Length - 1]).ToArray());
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
