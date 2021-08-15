using System.Linq;
using System;

namespace _4_longest_common_subsequence_of_two_sequences
{
    public static class CommonSubsequence2
    {
        private static int[] NextPoint(int[,] matrix, int[] point)
        {
            int[][] nextPoints = new int[3][];
            nextPoints[0] = new int[2] { point[0] - 1, point[1] - 1 };
            nextPoints[1] = new int[2] { point[0] - 1, point[1] };
            nextPoints[2] = new int[2] { point[0], point[1] - 1 };

            int[] minPoint = nextPoints[0];
            int minDist = matrix[minPoint[0], minPoint[1]];

            for (int i = 1; i < 3; i++)
            {
                var dist = matrix[nextPoints[i][0], nextPoints[i][1]];
                if (dist < minDist)
                {
                    minDist = dist;
                    minPoint = nextPoints[i];
                }
            }
            return minPoint;
        }

        private static int Min(params int[] values)
        {
            Array.Sort(values, (v1, v2) => v1.CompareTo(v2));
            return values[0];
        }

        private static int[,] GetEditMatrix(long[] sequence1, long[] sequence2)
        {
            var matrix = new int[sequence1.Length + 1, sequence2.Length + 1];
            for (int i = 0; i <= sequence1.Length; i++)
            {
                for (int j = 0; j <= sequence2.Length; j++)
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
                            sequence1[i - 1] == sequence2[j - 1] ? matrix[i - 1, j - 1] : int.MaxValue
                        );
                    }
                }
            }
            return matrix;
        }
        public static string Solve(string line1, string line2)
        {
            var sequence1 = line1.Split(' ').Select(x => long.Parse(x)).ToArray();
            var sequence2 = line2.Split(' ').Select(x => long.Parse(x)).ToArray();
            var matrix = GetEditMatrix(sequence1, sequence2);
            var count = 0;
            var curPoint = new int[2] { sequence1.Length, sequence2.Length };
            while (curPoint[0] > 0 && curPoint[1] > 0)
            {
                var nextPoint = NextPoint(matrix, curPoint);
                if (matrix[nextPoint[0], nextPoint[1]] == matrix[curPoint[0], curPoint[1]])
                {
                    count++;
                }
                curPoint = nextPoint;
            }
            return count.ToString();
        }

        static void Main(string[] args)
        {
            Console.ReadLine();
            var line1 = Console.ReadLine();
            Console.ReadLine();
            var line2 = Console.ReadLine();
            Console.WriteLine(Solve(line1, line2));
        }
    }
}
