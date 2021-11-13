using System.Linq;
using System;

namespace _1_energy_values
{
    public static class EnergyValues
    {
        private static void Swap(double[][] matrix, int s, int t)
        {
            var buf = matrix[t];
            matrix[t] = matrix[s];
            matrix[s] = buf;
        }

        private static void Scale(double[][] matrix, int row, int pivCf)
        {
            double k = matrix[row][pivCf];
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

        public static string Solve(string[] input)
        {
            // initialize matrix
            var matrix = new double[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                var coeff = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                matrix[i] = new double[coeff.Length];
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
