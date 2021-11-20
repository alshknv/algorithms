using System.Collections.Generic;
using System.Linq;
using System;

namespace _3_budget_allocation
{
    public static class BudgetAllocation
    {
        private static int[] ToBinary(int value, int digits)
        {
            var result = new int[digits];
            while (value > 0)
            {
                result[(int)Math.Log((value ^ (value - 1)) + 1, 2) - 1] = 1;
                value &= (value - 1);
            }
            return result;
        }

        public static string[] Solve(string[] input)
        {
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var n = nm[0];
            var m = nm[1];
            var A = new int[n][];
            for (int i = 1; i <= n; i++)
            {
                A[i - 1] = new int[m];
                var a = input[i].Split(' ');
                for (int j = 0; j < a.Length; j++)
                    A[i - 1][j] = int.Parse(a[j]);
            }
            var b = input[n + 1].Split(' ').Select(x => int.Parse(x)).ToArray();

            var output = new List<int[]>();
            output.Add(new int[2] { 0, m });

            for (int i = 0; i < n; i++)
            {
                var coeffs = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    if (A[i][j] != 0) coeffs.Add(j);
                    if (coeffs.Count == 3) break;
                }
                //check any possible assignment of 3 vars to falsify inequality
                for (int k = 0; k < Math.Pow(2, coeffs.Count); k++)
                {
                    var binary = ToBinary(k, coeffs.Count);
                    var sum = 0;
                    for (int l = 0; l < binary.Length; l++)
                    {
                        sum += A[i][coeffs[l]] * binary[l];
                    }
                    if (sum > b[i])
                    {
                        // inequality falsified, add clause
                        var clause = new int[binary.Length + 1];
                        for (var y = 0; y < binary.Length; y++)
                        {
                            clause[y] = (binary[y] > 0 ? -1 : 1) * (coeffs[y] + 1);
                        }
                        output.Add(clause);
                    }
                }
            }
            output[0][0] = output.Count - 1;
            return output.Select(x => string.Join(" ", x)).ToArray();
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var n = int.Parse(nmline.Split(' ')[0]);
            var input = new string[n + 2];
            input[0] = nmline;
            for (int i = 1; i <= n; i++)
            {
                input[i] = Console.ReadLine();
            }
            input[n + 1] = Console.ReadLine();
            foreach (var resline in Solve(input))
            {
                Console.WriteLine(resline);
            }
        }
    }
}
