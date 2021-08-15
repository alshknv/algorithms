using System;

namespace _3_edit_distance
{
    public static class EditDistance
    {
        private static int Min(params int[] values)
        {
            Array.Sort(values, (v1, v2) => v1.CompareTo(v2));
            return values[0];
        }

        public static string Solve(string line1, string line2)
        {
            var matrix = new int[line1.Length + 1, line2.Length + 1];
            for (int i = 0; i <= line1.Length; i++)
            {
                for (int j = 0; j <= line2.Length; j++)
                {
                    if (i == 0)
                    {
                        matrix[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        matrix[i, j] = i;
                    }
                    else
                    {
                        matrix[i, j] = Min(
                            matrix[i, j - 1] + 1,
                            matrix[i - 1, j] + 1,
                            line1[i - 1] == line2[j - 1] ? matrix[i - 1, j - 1] : matrix[i - 1, j - 1] + 1
                        );
                    }
                }
            }
            return matrix[line1.Length, line2.Length].ToString();
        }

        static void Main(string[] args)
        {
            var line1 = Console.ReadLine();
            var line2 = Console.ReadLine();
            Console.WriteLine(Solve(line1, line2));
        }
    }
}
