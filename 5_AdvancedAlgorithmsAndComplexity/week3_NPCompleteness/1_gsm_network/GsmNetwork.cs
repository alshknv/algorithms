using System.Linq;
using System;

namespace _1_gsm_network
{
    public static class GsmNetwork
    {
        public static string[] Solve(string[] input)
        {
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var n = nm[0];
            var m = nm[1];
            var varCount = n * 3;
            var clauseCount = n + m * 3;
            var output = new int[clauseCount + 1][];
            output[0] = new int[2] { clauseCount, varCount };
            // first n clauses - each vertex must have color;
            for (int i = 1; i <= n; i++)
            {
                var b = 3 * (i - 1) + 1;
                output[i] = new int[4] { b, b + 1, b + 2, 0 };
            }
            // three clauses for each edge - color of vertices on its ends must be different
            for (int i = 1; i < input.Length; i++)
            {
                var uv = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                var baseU = 3 * (uv[0] - 1) + 1;
                var baseV = 3 * (uv[1] - 1) + 1;
                var baseO = n + 3 * (i - 1) + 1;
                for (int j = 0; j < 3; j++)
                {
                    output[baseO + j] = new int[3] { -(baseU + j), -(baseV + j), 0 };
                }
            }
            return output.Select(x => string.Join(" ", x)).ToArray();
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var m = int.Parse(nmline.Split(' ')[1]);
            var input = new string[m + 1];
            input[0] = nmline;
            for (int i = 1; i <= m; i++)
            {
                input[i] = Console.ReadLine();
            }
            foreach (var resline in Solve(input))
            {
                Console.WriteLine(resline);
            }
        }
    }
}
