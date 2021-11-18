using System.Collections.Generic;
using System.Linq;
using System;

namespace _2_cleaning_apartment
{
    public static class CleaningApartment
    {
        private static List<int[]> OnlyOneClauses(int[] clause)
        {
            var result = new List<int[]>();
            for (int i = 0; i < clause.Length - 1; i++)
            {
                for (int j = i + 1; j < clause.Length - 1; j++)
                {
                    result.Add(new int[3] { -clause[i], -clause[j], 0 });
                }
            }
            return result;
        }

        public static string[] Solve(string[] input)
        {
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var n = nm[0];
            var m = nm[1];
            //x[ij] = 1 if ith node is at jth place in Hmiltonian cycle
            var output = new List<int[]>();
            output.Add(new int[2] { 0, n * n });

            var edgeMatrix = new bool[n + 1][];
            var varMatrix = new int[n + 1][];
            for (int i = 1; i <= n; i++)
            {
                varMatrix[i] = new int[n + 1];
                edgeMatrix[i] = new bool[n + 1];
                for (int j = 1; j <= n; j++)
                {
                    varMatrix[i][j] = (i - 1) * n + j;
                    edgeMatrix[i][j] = i != j + 1;
                }
            }

            for (int i = 1; i <= n; i++)
            {
                var vicClause = new int[n + 1];
                var phvClause = new int[n + 1];
                for (int j = 1; j <= n; j++)
                {
                    vicClause[j - 1] = varMatrix[i][j];
                    phvClause[j - 1] = varMatrix[j][i];
                }
                output.Add(vicClause);
                output.Add(phvClause);
                output.AddRange(OnlyOneClauses(vicClause));
                output.AddRange(OnlyOneClauses(phvClause));
            }

            for (int i = 1; i < input.Length; i++)
            {
                var uv = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                edgeMatrix[uv[0]][uv[1]] = edgeMatrix[uv[1]][uv[0]] = false;
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = i + 1; j <= n; j++)
                {
                    if (edgeMatrix[i][j])
                    {
                        //no edge between i and j, add clause
                        for (int k = 1; k <= n; k++)
                        {
                            if (k + 1 <= n)
                                output.Add(new int[3] { -varMatrix[i][k], -varMatrix[j][k + 1], 0 });
                        }
                    }
                }
            }

            output[0][0] = output.Count - 1;

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
