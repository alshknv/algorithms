using System.Collections.Generic;
using System.Linq;
using System;

namespace _3_school_bus
{
    public static class SchoolBus
    {
        private static Dictionary<int, int[]> SetsNk(int n, int k)
        {
            var result = new Dictionary<int, int[]>();
            var len = (int)Math.Pow(2, n);
            for (int i = 0; i < len; i++)
            {
                var v = i;
                var set = new List<int>();
                var count = 0;
                while (v > 0)
                {
                    count++;
                    set.Add((int)Math.Log((v ^ (v - 1)) + 1, 2) - 1);
                    v &= v - 1;
                }
                if (count == k)
                    result.Add(i, set.ToArray());
            }

            return result;
        }

        private static int[] SetBits(int value)
        {
            var set = new List<int>();
            while (value > 0)
            {
                set.Add((int)Math.Log((value ^ (value - 1)) + 1, 2) - 1);
                value &= value - 1;
            }
            return set.ToArray();
        }

        private static string[] TSP(int n, int[,] distance)
        {
            var dp = new int[(int)Math.Pow(2, n)][];

            foreach (var setInit in SetsNk(n, 1))
            {
                dp[setInit.Key] = new int[n + 1];
            }

            for (int k = 2; k <= n; k++)
            {
                foreach (var setCur in SetsNk(n, k))
                {
                    dp[setCur.Key] = new int[n];
                    dp[setCur.Key][0] = int.MaxValue;
                    for (int i = 0; i < setCur.Value.Length; i++)
                    {
                        if (setCur.Value[i] == 0) continue;
                        dp[setCur.Key][setCur.Value[i]] = int.MaxValue;
                        for (int j = 0; j < setCur.Value.Length; j++)
                        {
                            if (setCur.Value[i] != setCur.Value[j] && distance[setCur.Value[i] + 1, setCur.Value[j] + 1] > 0)
                            {
                                var prevValue = setCur.Key ^ (1 << setCur.Value[i]);
                                var pathLen = dp[prevValue][setCur.Value[j]] + distance[setCur.Value[i] + 1, setCur.Value[j] + 1];
                                if (pathLen > 0 && pathLen < dp[setCur.Key][setCur.Value[i]])
                                {
                                    dp[setCur.Key][setCur.Value[i]] = pathLen;
                                }
                            }
                        }
                    }
                }
            }

            // calculate minimum path
            var minPath = int.MaxValue;
            for (int i = 1; i < n; i++)
            {
                if (distance[i + 1, 1] > 0)
                {
                    var path = dp[dp.Length - 1][i] + distance[i + 1, 1];
                    if (path > 0 && path < minPath)
                    {
                        minPath = path;
                    }
                }
            }

            // reconstruct path, going backwards from last dp element
            var result = new int[n];
            result[0] = 1;
            var rIdx = n - 1;
            var cIdx = dp.Length - 1;
            var curIdx = 1;
            var curValue = minPath;
            while (rIdx > 0)
            {
                for (int i = 1; i < n; i++)
                {
                    if (dp[cIdx][i] > 0 && dp[cIdx][i] < int.MaxValue && distance[i + 1, curIdx] + dp[cIdx][i] == curValue)
                    {
                        curValue = dp[cIdx][i];
                        curIdx = i + 1;
                        result[rIdx] = i + 1;
                        cIdx ^= (1 << i);
                        break;
                    }
                }
                rIdx--;
            }

            if (result.Length > 2 && minPath > 0 && minPath < int.MaxValue)
            {
                return new string[] {
                    minPath.ToString(),
                    string.Join(" ", result)
                };
            }
            else
            {
                return new string[] { "-1" };
            }
        }

        public static string[] Solve(string[] input)
        {
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var distance = new int[nm[0] + 1, nm[0] + 1];
            for (int i = 0; i <= nm[0]; i++)
            {
                for (int j = 0; j <= nm[0]; j++)
                {
                    distance[i, j] = -1;
                }
            }

            for (int i = 1; i < input.Length; i++)
            {
                var edge = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                distance[edge[0], edge[1]] = distance[edge[1], edge[0]] = edge[2];
            }
            return TSP(nm[0], distance);
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var m = int.Parse(nmline.Split(' ').Last());
            var input = new string[m + 1];
            input[0] = nmline;
            for (int i = 1; i <= m; i++)
            {
                input[i] = Console.ReadLine();
            }

            foreach (var line in Solve(input))
                Console.WriteLine(line);
        }
    }
}
