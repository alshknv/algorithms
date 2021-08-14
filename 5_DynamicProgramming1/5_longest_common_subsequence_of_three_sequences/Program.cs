using System.Collections.Generic;
using System.Linq;
using System;

namespace _5_longest_common_subsequence_of_three_sequences
{
    public static class CommonSubsequence3
    {

        private static int Max(params int[] values)
        {
            Array.Sort(values, (v1, v2) => -v1.CompareTo(v2));
            return values[0];
        }

        public static string Solve(string line1, string line2, string line3)
        {
            var sequence1 = line1.Split(' ').Select(x => long.Parse(x)).ToArray();
            var sequence2 = line2.Split(' ').Select(x => long.Parse(x)).ToArray();
            var sequence3 = line3.Split(' ').Select(x => long.Parse(x)).ToArray();
            var matrix = new int[sequence1.Length + 1, sequence2.Length + 1, sequence3.Length + 1];
            for (int i = 0; i <= sequence1.Length; i++)
            {
                for (int j = 0; j <= sequence2.Length; j++)
                {
                    for (int k = 0; k <= sequence3.Length; k++)
                    {
                        if (i == 0 || j == 0 || k == 0)
                        {
                            matrix[i, j, k] = 0;
                        }
                        else if (sequence1[i - 1] == sequence2[j - 1] && sequence1[i - 1] == sequence3[k - 1])
                        {
                            matrix[i, j, k] = matrix[i - 1, j - 1, k - 1] + 1;
                        }
                        else
                        {
                            matrix[i, j, k] = Max(
                                matrix[i - 1, j, k],
                                matrix[i, j - 1, k],
                                matrix[i, j, k - 1]);
                        }
                    }
                }
            }
            return matrix[sequence1.Length, sequence2.Length, sequence3.Length].ToString();
        }

        static void Main(string[] args)
        {
            Console.ReadLine();
            var line1 = Console.ReadLine();
            Console.ReadLine();
            var line2 = Console.ReadLine();
            Console.ReadLine();
            var line3 = Console.ReadLine();
            Console.WriteLine(Solve(line1, line2, line3));
        }
    }
}
