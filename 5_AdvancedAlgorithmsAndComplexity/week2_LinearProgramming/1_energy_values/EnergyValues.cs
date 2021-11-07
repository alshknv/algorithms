using System.Linq;
using System;

namespace _1_energy_values
{
    public static class EnergyValues
    {
        private static void SwapToTop(decimal[][] matrix, int i)
        {
            var buf = matrix[0];
            matrix[0] = matrix[i];
            matrix[i] = buf;
        }

        private static void Scale(decimal[][] matrix, int row, int pivCf)
        {
            decimal k = matrix[row][pivCf];
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

        public static string Solve(string[] input)
        {
            // initialize matrix
            var matrix = new decimal[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                var coeff = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                matrix[i] = new decimal[coeff.Length];
                for (int j = 0; j < coeff.Length; j++)
                {
                    matrix[i][j] = coeff[j];
                }
            }

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

            var result = new string[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == 1)
                    {
                        result[j] = matrix[i][matrix.Length].ToString("0.00000").Replace(",", ".");
                        break;
                    }
                }
            }
            return string.Join(" ", result);
        }

        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var input = new string[n];
            for (int i = 0; i < n; i++)
            {
                input[i] = Console.ReadLine();
            }
            Console.WriteLine(Solve(input));
        }
    }
}
