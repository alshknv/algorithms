using System.Collections.Generic;
using System.Linq;
using System;

namespace _3_school_bus
{
    public static class SchoolBus
    {
        private static Dictionary<int, int[]> SetsNk(int n, int k)
        {
            var result = new Dictionary<int, int[]>();
            var len = (int)Math.Pow(2, n);
            for (int i = 0; i < len; i++)
            {
                var v = i;
                var set = new List<int>();
                var count = 0;
                while (v > 0)
                {
                    count++;
                    set.Add((int)Math.Log((v ^ (v - 1)) + 1, 2) - 1);
                    v &= v - 1;
                }
                if (count == k)
                    result.Add(i, set.ToArray());
            }

            return result;
        }

        private static int[] SetBits(int value)
        {
            var set = new List<int>();
            while (value > 0)
            {
                set.Add((int)Math.Log((value ^ (value - 1)) + 1, 2) - 1);
                value &= value - 1;
            }
            return set.ToArray();
        }

        private static void TSP(int n, int[,] matrix)
        {
            var c = new int[(int)Math.Pow(2, n)];
            var prev = new int[n];
            for (int k = 2; k <= n; k++)
            {
                foreach (var setCur in SetsNk(n, k))
                {
                    c[setCur.Key] = int.MaxValue;
                    for (int i = 0; i < setCur.Value.Length; i++)
                    {
                        var si = setCur.Key ^ (1 << setCur.Value[i]);
                        foreach (var setPrev in SetBits(si))
                        {
                            var len = c[si] + matrix[setCur.Value[i] + 1, setPrev + 1];
                            if (len < c[setCur.Key])
                            {
                                c[setCur.Key] = len;
                                prev[setCur.Value[i]] = setPrev + 1;
                            }
                        }
                    }
                }
            }
            foreach (var setk in SetsNk(n, n))
            {

            }
        }

        public static string[] Solve(string[] input)
        {
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var matrix = new int[nm[0] + 1, nm[0] + 1];
            for (int i = 1; i < input.Length; i++)
            {
                var edge = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                matrix[edge[0], edge[1]] = matrix[edge[1], edge[0]] = edge[2];
            }
            TSP(nm[0], matrix);
            return new string[0];
        }

        static void Main(string[] args)
        {
            var gg = Solve(new string[] {"4 6",
"1 2 20",
"1 3 42",
"1 4 35",
"2 3 30",
"2 4 34",
"3 4 12"});
            return;

            var nmline = Console.ReadLine();
            var m = int.Parse(nmline.Split(' ').Last());
            var input = new string[m + 1];
            input[0] = nmline;
            for (int i = 1; i <= m; i++)
            {
                input[i] = Console.ReadLine();
            }

            foreach (var line in Solve(input))
                Console.WriteLine(line);
        }
    }
}
