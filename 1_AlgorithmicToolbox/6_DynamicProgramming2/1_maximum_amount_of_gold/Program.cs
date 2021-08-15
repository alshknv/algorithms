using System.Linq;
using System;

namespace _1_maximum_amount_of_gold
{
    public static class MaximumAmountOfGold
    {
        private static int?[,] V;

        public static int BruteForce(int[] bars, int W, int v)
        {
            if (bars.Length == 1)
            {
                return bars[0] <= W ? v + bars[0] : v;
            }
            var maxWeight = 0;
            for (int i = 0; i < bars.Length; i++)
            {
                var newbars = bars.Where((value, index) => index != i).ToArray();
                var weight = Math.Max(
                    W - bars[i] >= 0 ? BruteForce(newbars, W - bars[i], v + bars[i]) : int.MinValue,
                    BruteForce(newbars, W, v)
                );
                if (weight > maxWeight)
                {
                    maxWeight = weight;
                }
            }
            return maxWeight;
        }

        public static int Recursive(int[] bars, int w, int b)
        {
            if (V == null) V = new int?[w + 1, bars.Length + 1];
            if (V[w, b] == null)
            {
                if (w == 0 || b == 0)
                {
                    V[w, b] = 0;
                }
                else
                {
                    V[w, b] = Recursive(bars, w, b - 1);
                    if (w >= bars[b - 1])
                    {
                        V[w, b] = (int)Math.Max(
                            (int)V[w, b],
                            Recursive(bars, w - bars[b - 1], b - 1) + bars[b - 1]
                        );
                    }
                }
                return (int)V[w, b];
            }
            else
            {
                return (int)V[w, b];
            }
        }

        public static int Iterative(int[] bars, int W)
        {
            var values = new int[W + 1, bars.Length + 1];
            for (int w = 0; w <= W; w++)
            {
                for (int b = 0; b <= bars.Length; b++)
                {
                    if (w == 0 || b == 0)
                    {
                        values[w, b] = 0;
                    }
                    else
                    {
                        values[w, b] = values[w, b - 1];
                        if (w >= bars[b - 1])
                        {
                            values[w, b] = (int)Math.Max(
                                values[w, b],
                                values[w - bars[b - 1], b - 1] + bars[b - 1]);
                        }
                    }
                }
            }
            return values[W, bars.Length];
        }

        public static string Solve(string line1, string line2)
        {
            var W = int.Parse(line1.Split(' ')[0]);
            var bars = line2.Split(' ').Select(x => int.Parse(x)).ToArray();
            return Iterative(bars, W).ToString();
        }
        static void Main(string[] args)
        {
            var line1 = Console.ReadLine();
            var line2 = Console.ReadLine();
            Console.WriteLine(Solve(line1, line2));
        }
    }
}
