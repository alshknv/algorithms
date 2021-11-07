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

        private static void SwapToTop(decimal[][] matrix, int i)
        {
            var buf = matrix[0];
            matrix[0] = matrix[i];
            matrix[i] = buf;
        }

        private static void Scale(decimal[][] matrix, int row, int pivCf)
        {
            var k = matrix[row][pivCf];
            for (int j = 0; j < matrix[row].Length; j++)
            {
                matrix[row][j] /= k;
            }
        }

        private static void Eliminate(decimal[][] matrix, int pivRow, int pivCf)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i != pivRow)
                {
                    decimal k = -matrix[i][pivCf] / matrix[pivRow][pivCf];
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        matrix[i][j] += k * matrix[pivRow][j];
                    }
                }
            }
        }

        private static void GaussianElimination(decimal[][] matrix)
        {
            var c = 0;
            while (c < matrix.Length)
            {
                for (int i = c; i < matrix.Length; i++)
                {
                    if ((c == 0 || (c > 0 && matrix[i][c - 1] == 0m)) && matrix[i][c] != 0)
                    {
                        SwapToTop(matrix, i);
                        Scale(matrix, 0, c);
                        Eliminate(matrix, 0, c);
                        break;
                    }
                }
                c++;
            }
        }

        private static bool ResultSatisfiesConstraints(decimal[][] matrix, decimal?[] result)
        {
            return ResultSatisfiesConstraints(matrix, result.Select(x => (decimal)x).ToArray());
        }

        private static bool ResultSatisfiesConstraints(decimal[][] matrix, decimal[] result)
        {
            var satisfies = true;
            for (int i = 0; i < matrix.Length; i++)
            {
                decimal value = 0;
                for (int j = 0; j < matrix[i].Length - 1; j++)
                {
                    value += (decimal)result[j] * matrix[i][j];
                }
                if (value > matrix[i][matrix[i].Length - 1])
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
            var matrix = new decimal[nm[0] + nm[1]][];
            for (int i = 0; i < nm[0]; i++)
            {
                var coeff = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                matrix[i] = new decimal[nm[1] + 1];
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
                matrix[nm[0] + k] = new decimal[nm[1] + 1];
                for (int l = 0; l <= nm[1]; l++) matrix[nm[0] + k][l] = l == k ? 1 : 0;
            }
            var plCoeff = input[nm[0] + 2].Split(' ').Select(x => int.Parse(x)).ToArray();

            //check if there're boundaries
            for (int r = 0; r < nm[1]; r++)
            {
                var noboundResult = new decimal[nm[1]];
                noboundResult[r] = 100000000;
                if (ResultSatisfiesConstraints(matrix.Take(nm[0]).ToArray(), noboundResult))
                {
                    return new string[] { "Infinity" };
                }
            }

            //enumerate groups of m inequalities
            var maxPleasure = decimal.MinValue;
            string[] bestResult = null;
            for (int w = 0; w < Math.Pow(2, nm[0] + nm[1]); w++)
            {
                var setBits = SetBits(w);
                if (setBits.Length != nm[1]) continue;
                var system = new decimal[nm[1]][];
                for (int t = 0; t < nm[1]; t++)
                {
                    system[t] = new List<decimal>(matrix[setBits[t]]).ToArray();
                }
                // solve system of m equalities
                GaussianElimination(system);
                var result = new decimal?[system.Length];

                //check if non-negative solution exists
                var gaussSolution = true;
                for (int i = 0; i < system.Length; i++)
                {
                    var hasValue = false;
                    for (int j = 0; j < system[i].Length - 1; j++)
                    {
                        if (system[i][j] == 1)
                        {
                            if (!hasValue && system[i][system.Length] >= 0)
                            {
                                result[j] = system[i][system.Length];
                                hasValue = true;
                            }
                            else
                            {
                                gaussSolution = false;
                                break;
                            }
                        }
                    }
                    if (!hasValue) gaussSolution = false;
                    if (!gaussSolution) break;
                }
                if (!gaussSolution) continue;

                // if result satifies constraints maximize function

                if (ResultSatisfiesConstraints(matrix.Take(nm[0]).ToArray(), result))
                {
                    decimal pleasure = 0;
                    for (int j = 0; j < plCoeff.Length; j++)
                    {
                        pleasure += (decimal)result[j] * plCoeff[j];
                    }
                    if (pleasure > maxPleasure)
                    {
                        maxPleasure = pleasure;
                        bestResult = result.Select(x => x?.ToString("0.000000000000000000").Replace(",", ".")).ToArray();
                    }
                }
            }

            if (maxPleasure == decimal.MinValue) return new string[] { "No solution" };
            return new string[] { "Bounded solution", string.Join(" ", bestResult) };
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
