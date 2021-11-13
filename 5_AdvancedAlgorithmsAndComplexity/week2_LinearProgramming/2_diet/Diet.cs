using System.Linq;
using System.Collections.Generic;
using System;

namespace _2_diet
{
    public static class Diet
    {
        private static int[] SetBits(int value)
        {
            var result = new List<int>();
            while (value > 0)
            {
                result.Add((int)Math.Log((value ^ (value - 1)) + 1, 2) - 1);
                value &= (value - 1);
            }
            return result.ToArray();
        }

        private static void Swap(double[][] matrix, int s, int t)
        {
            var buf = matrix[t];
            matrix[t] = matrix[s];
            matrix[s] = buf;
        }

        private static void Scale(double[][] matrix, int row, int pivCf)
        {
            var k = matrix[row][pivCf];
            for (int j = 0; j < matrix[row].Length; j++)
            {
                matrix[row][j] /= k;
            }
        }

        private static void Eliminate(double[][] matrix, int pivRow, int pivCf)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i != pivRow)
                {
                    double k = -matrix[i][pivCf] / matrix[pivRow][pivCf];
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        matrix[i][j] += k * matrix[pivRow][j];
                    }
                }
            }
        }

        private static bool GaussianElimination(double[][] matrix, out double?[] result)
        {
            result = new double?[matrix.Length];
            var c = 0;
            while (c < matrix.Length)
            {
                for (int i = c; i < matrix.Length; i++)
                {
                    if (Math.Round(matrix[i][c], 5) != 0)
                    {
                        Swap(matrix, i, c);
                        Scale(matrix, c, c);
                        Eliminate(matrix, c, c);
                        break;
                    }
                }
                c++;
            }

            //check if non-negative solution exists
            for (int i = 0; i < matrix.Length; i++)
            {
                var hasValue = false;
                for (int j = 0; j < matrix[i].Length - 1; j++)
                {
                    if (Math.Round(matrix[i][j], 5) == 1)
                    {
                        if (!hasValue && Math.Round(matrix[i][matrix.Length], 5) >= 0)
                        {
                            result[j] = matrix[i][matrix.Length];
                            hasValue = true;
                        }
                        else
                        {
                            // multiple variables in one final equation
                            return false;
                        }
                    }
                }
                if (!hasValue)
                {
                    // 0 == 1
                    return false;
                }
            }
            return true;
        }

        private static bool ResultSatisfiesConstraints(double[][] matrix, double?[] result)
        {
            var satisfies = true;
            for (int i = 0; i < matrix.Length; i++)
            {
                double value = 0;
                for (int j = 0; j < matrix[i].Length - 1; j++)
                {
                    value += (double)result[j] * matrix[i][j];
                }
                if (Math.Round(value, 5) > Math.Round(matrix[i][matrix[i].Length - 1], 5))
                {
                    satisfies = false;
                    break;
                }
            }
            return satisfies;
        }

        public static string[] Solve(string[] input)
        {
            // init matrix
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var matrix = new double[nm[0] + nm[1] + 1][];
            for (int i = 0; i < nm[0]; i++)
            {
                var coeff = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                matrix[i] = new double[nm[1] + 1];
                for (int j = 0; j < coeff.Length; j++)
                {
                    matrix[i][j] = coeff[j];
                }
            }
            var b = input[nm[0] + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
            for (int j = 0; j < b.Length; j++)
            {
                matrix[j][nm[1]] = b[j];
            }
            for (int k = 0; k < nm[1]; k++)
            {
                matrix[nm[0] + k] = new double[nm[1] + 1];
                for (int l = 0; l <= nm[1]; l++) matrix[nm[0] + k][l] = l == k ? 1 : 0;
            }

            //last inequality for infinity, sum of x's must me less than 10^9
            matrix[matrix.Length - 1] = new double[nm[1] + 1];
            for (int l = 0; l < nm[1]; l++) matrix[matrix.Length - 1][l] = 1;
            matrix[matrix.Length - 1][nm[1]] = 1000000000;

            var plCoeff = input[nm[0] + 2].Split(' ').Select(x => int.Parse(x)).ToArray();

            var maxPleasure = double.MinValue;
            double?[] bestResult = null;

            // a kind of brute-force algorithm - check every vertex to find optimum
            for (int w = 0; w < Math.Pow(2, matrix.Length); w++)
            {
                var setBits = SetBits(w);
                if (setBits.Length != nm[1]) continue;
                var system = new double[nm[1]][];
                for (int t = 0; t < nm[1]; t++)
                {
                    system[t] = new List<double>(matrix[setBits[t]]).ToArray();
                }
                // solve system of m equalities to find value of function at this vertex
                double?[] result;
                // if system has no solution, proceed to next system
                if (!GaussianElimination(system, out result)) continue;

                // if result satifies constraints check if it is maximal
                if (ResultSatisfiesConstraints(matrix.Take(nm[0]).ToArray(), result))
                {
                    double pleasure = 0;
                    for (int j = 0; j < plCoeff.Length; j++)
                    {
                        pleasure += (double)result[j] * plCoeff[j];
                    }
                    if (pleasure > maxPleasure)
                    {
                        maxPleasure = pleasure;
                        bestResult = result;
                    }
                }
            }

            if (maxPleasure == double.MinValue) return new string[] { "No solution" };
            if (bestResult.Any(x => x > 1000000)) return new string[] { "Infinity" };
            return new string[] { "Bounded solution", string.Join(" ", bestResult.Select(x => x?.ToString("0.000000000000000000").Replace(",", ".")).ToArray()) };
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var n = int.Parse(nmline.Split(' ')[0]);
            var input = new string[n + 3];
            input[0] = nmline;
            for (int i = 1; i <= n; i++)
                input[i] = Console.ReadLine();
            input[n + 1] = Console.ReadLine();
            input[n + 2] = Console.ReadLine();

            foreach (var line in Solve(input))
                Console.WriteLine(line);
        }
    }
}
