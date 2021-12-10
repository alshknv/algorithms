using System.Collections.Generic;
using System;
using System.Linq;

namespace _1_heavy_hitters
{
    public class PriorityQueue
    {
        private int[] data = new int[100];
        private int count = 0;

        private void Swap(int x, int y)
        {
            var buf = data[x];
            data[x] = data[y];
            data[y] = buf;
        }

        private void SiftUp(int i)
        {
            var p = (i - 1) / 2;
            while (i > 0 && data[i] > data[p])
            {
                Swap(i, p);
                i = p;
                p = i / 2;
            }
        }

        private void SiftDown(int i)
        {
            var l = 2 * i + 1;
            var r = 2 * i + 2;
            while (l < count)
            {
                if (r < count && data[r] > data[l]) l = r;
                if (data[l] > data[i])
                {
                    Swap(i, l);
                    i = l;
                }
                else
                {
                    break;
                }
            }
        }

        public void Clear()
        {
            count = 0;
        }

        public void Add(int value)
        {
            data[count] = value;
            SiftUp(count);
            count++;
        }

        public int Extract()
        {
            var top = data[0];
            data[0] = data[count - 1];
            count--;
            SiftDown(0);
            return top;
        }
    }

    public static class HeavyHitters
    {
        private const int p = 1000000007;
        private const int nb = 50000;
        private const int nh = 3;
        public static int HashM(this int[] f, long x, int m)
        {
            return (int)(((f[0] * x) + f[1]) % p % m);
        }

        public static int HashS(this int[] f, int x)
        {
            double v1 = ((f[0] * x) + f[1]) % p / p;
            return 0.5 - v1 > 0 ? 1 : -1;
        }

        public static string Solve(int n, int t, int[][] lists, int[] queries)
        {
            // init hash functions
            var hash = new int[nh][];
            var a = 10;
            var b = 33;
            for (int i = 0; i < nh; i++)
            {
                hash[i] = new int[2] { a, b };
                a *= 2;
                b *= 3;
            }

            // int stream data buf
            var data = new int[nb, nh];

            // processing stream
            for (int i = 0; i < lists.Length; i++)
            {
                for (int j = 0; j < nh; j++)
                {
                    var bi = hash[j].HashM(lists[i][0], nb);
                    data[bi, j] += (i < n ? 1 : -1) * lists[i][1] * hash[j].HashS(lists[i][0]);
                }
            }

            // processing queries
            var result = new int[queries.Length];
            var pq = new PriorityQueue();
            var half2 = 0;
            const int nh2 = nh / 2;
            int med;
            for (int i = 0; i < queries.Length; i++)
            {
                pq.Clear();
                for (int j = 0; j < nh; j++)
                {
                    pq.Add(data[hash[j].HashM(queries[i], nb), j] * hash[j].HashS(queries[i]));
                }
                // median
                for (int j = 0; j < nh2; j++)
                    half2 = pq.Extract();
                med = nh2 > 0 && nh2 % 2 > 0 ? pq.Extract() : (pq.Extract() + half2) / 2;
                result[i] = med < t ? 0 : 1;
            }

            return string.Join(" ", result);
        }

        static void Main(string[] args)
        {
            var nline = Console.ReadLine();
            var tline = Console.ReadLine();
            var n = int.Parse(nline);
            var t = int.Parse(tline);
            var lists = new int[2 * n][];
            for (int i = 0; i < 2 * n; i++)
            {
                lists[i] = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
            }
            Console.ReadLine();
            var queries = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
            Console.WriteLine(Solve(n, t, lists, queries));
        }
    }
}
