using System.Collections.Generic;
using System.Linq;
using System;

namespace _3_school_bus
{
    public static class SchoolBus
    {
        private readonly static long[] fact = new long[20];
        private readonly static long[] pow2 = new long[20];

        private static long BitValue(int[] bits)
        {
            long value = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                value += bits[i] * pow2[i];
            }
            return value;
        }

        private static bool MoveBit(int[] bits, int p)
        {
            if (p + 1 < bits.Length && bits[p + 1] == 0 && bits[p] == 1)
            {
                bits[p + 1] = 1;
                bits[p] = 0;
                return true;
            }
            return false;
        }

        private static void ShiftBitsToRight(int[] bits, int p, int m)
        {
            for (int i = p; i >= 0; i--)
            {
                bits[i] = i < m ? 1 : 0;
            }
        }

        private static int[] GetSetBits(int[] bits, int k)
        {
            var setBits = new int[k];
            var c = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == 1) setBits[c++] = i;
                if (c == k) break;
            }
            return setBits;
        }

        private static void MoveAllBits(int k0, int k, int[] bits, int p, ref int c, long[] values, int[][] setBits)
        {
            while (p >= 0 && p + 1 < bits.Length && bits[p] == 1 && bits[p + 1] == 0)
            {
                MoveBit(bits, p);
                ShiftBitsToRight(bits, p - 1, k - 1);
                values[c] = BitValue(bits);
                setBits[c] = GetSetBits(bits, k0);
                c++;
                MoveAllBits(k0, k - 1, bits, k - 2, ref c, values, setBits);
                p++;
            }
        }

        private static void SetsNk(int n, int k, out long[] values, out int[][] setBits)
        {
            var ncomb = k == n ? 1 : fact[n] / fact[n - k] / fact[k];
            var bits = new int[n];
            for (int i = 0; i < k; i++)
            {
                bits[i] = 1;
            }
            values = new long[ncomb];
            setBits = new int[ncomb][];
            values[0] = BitValue(bits);
            setBits[0] = GetSetBits(bits, k);
            if (ncomb == 1) return;
            var c = 1;
            var f0 = k - 1;
            while (f0 < bits.Length - 1)
            {
                MoveBit(bits, f0);
                ShiftBitsToRight(bits, f0 - 1, k - 1);
                values[c] = BitValue(bits);
                setBits[c] = GetSetBits(bits, k);
                c++;
                MoveAllBits(k, k - 1, bits, k - 2, ref c, values, setBits);
                f0++;
            }
        }

        private static string[] TSP(int n, int[,] distance)
        {
            var dp = new int[(int)Math.Pow(2, n)][];
            long f = 1;
            pow2[0] = 1;
            for (long i = 1; i < 20; i++)
            {
                f *= i;
                fact[i] = f;
                pow2[i] = (long)Math.Pow(2, i);
            }

            long[] setValues;
            int[][] setBits;

            SetsNk(n, 1, out setValues, out setBits);
            foreach (var val in setValues)
            {
                dp[val] = new int[n + 1];
            }

            for (int k = 2; k <= n; k++)
            {
                SetsNk(n, k, out setValues, out setBits);
                for (var s = 0; s < setValues.Length; s++)
                {
                    dp[setValues[s]] = new int[n];
                    dp[setValues[s]][0] = int.MaxValue;
                    for (int i = 0; i < setBits[s].Length; i++)
                    {
                        if (setBits[s][i] == 0) continue;
                        dp[setValues[s]][setBits[s][i]] = int.MaxValue;
                        for (int j = 0; j < setBits[s].Length; j++)
                        {
                            if (setBits[s][i] != setBits[s][j] && distance[setBits[s][i] + 1, setBits[s][j] + 1] > 0)
                            {
                                var prevValue = setValues[s] ^ (1 << setBits[s][i]);
                                var pathLen = dp[prevValue][setBits[s][j]] + distance[setBits[s][i] + 1, setBits[s][j] + 1];
                                if (pathLen > 0 && pathLen < dp[setValues[s]][setBits[s][i]])
                                {
                                    dp[setValues[s]][setBits[s][i]] = pathLen;
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

            if (minPath > 0 && minPath < int.MaxValue)
            {
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
