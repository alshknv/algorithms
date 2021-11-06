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

        private static void SwapToTop(double[][] matrix, int i)
        {
            var buf = matrix[0];
            matrix[0] = matrix[i];
            matrix[i] = buf;
        }

        private static void Scale(double[][] matrix, int row, int pivCf)
        {
            double k = 1 / matrix[row][pivCf];
            for (int j = 0; j < matrix[row].Length; j++)
            {
                matrix[row][j] *= k;
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

        private static void GaussianElimination(double[][] matrix)
        {
            var c = 0;
            while (c < matrix.Length)
            {
                for (int i = c; i < matrix.Length; i++)
                {
                    if ((c == 0 || (c > 0 && matrix[i][c - 1] == 0)) && matrix[i][c] != 0)
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

        public static string Solve(string[] input)
        {
            // init matrix
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var matrix = new double[nm[0] + nm[1]][];
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

            //solve each group of m equalities
            for (int w = 0; w < Math.Pow(2, nm[0] + nm[1]); w++)
            {
                var setBits = SetBits(w);
                if (setBits.Length != nm[1]) continue;
                var system = new double[nm[1]][];
                for (int t = 0; t < nm[1]; t++)
                {
                    system[t] = matrix[setBits[t]];
                }
                GaussianElimination(system);

                // check if solution satisfies other inequalities
            }

            return "";
        }

        static void Main(string[] args)
        {
            // var gg = Solve(new string[] { "3 2", "-1 -1", "1 0", "0 1", "-1 2 2", "-1 2" });
            // var gg = Solve(new string[] { "1 3", "0 0 1", "3", "1 1 1" });
            var gg = Solve(new string[] { "2 2", "1 1", "-1 -1", "1 -2", "1 1" });
            return;
            var nmline = Console.ReadLine();
            var n = int.Parse(nmline.Split(' ')[0]);
            var input = new string[n + 3];
            input[0] = nmline;
            for (int i = 1; i <= n; i++)
                input[i] = Console.ReadLine();
            input[n + 1] = Console.ReadLine();
            input[n + 2] = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
