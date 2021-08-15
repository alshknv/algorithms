using System.Linq;
using System;

namespace _3_improving_quicksort
{
    public static class ImprovingQuicksort
    {
        private readonly static Random random = new Random();

        private static void Swap(long[] array, int idx1, int idx2)
        {
            var buf = array[idx1];
            array[idx1] = array[idx2];
            array[idx2] = buf;
        }

        private static int[] Partition3(long[] a, int l, int r)
        {
            int[] m = new int[2] { l, l };
            long x = a[l];
            for (int i = l + 1; i <= r; i++)
            {
                if (a[i] <= x)
                {
                    m[1]++;
                    Swap(a, i, m[1]);
                    if (a[m[1]] < x)
                    {
                        Swap(a, m[0], m[1]);
                        m[0]++;
                    }
                }
            }
            return m;
        }

        private static int Partition2(long[] a, int l, int r)
        {
            long x = a[l];
            int j = l;
            for (int i = l + 1; i <= r; i++)
            {
                if (a[i] <= x)
                {
                    j++;
                    Swap(a, i, j);
                }
            }
            Swap(a, l, j);
            return j;
        }

        private static void RandomizedQuickSort2(long[] a, int l, int r)
        {
            if (l >= r)
            {
                return;
            }
            int k = random.Next(r - l) + l;
            Swap(a, l, k);
            int m = Partition2(a, l, r);
            RandomizedQuickSort2(a, l, m - 1);
            RandomizedQuickSort2(a, m + 1, r);
        }

        private static void RandomizedQuickSort3(long[] a, int l, int r)
        {
            if (l >= r)
            {
                return;
            }
            int k = random.Next(r - l) + l;
            Swap(a, l, k);
            int[] m = Partition3(a, l, r);
            RandomizedQuickSort3(a, l, m[0] - 1);
            RandomizedQuickSort3(a, m[1] + 1, r);
        }

        public static string Solve(string input)
        {
            var array = input.Split(' ').Select(x => long.Parse(x)).ToArray();
            RandomizedQuickSort3(array, 0, array.Length - 1);
            return string.Join(" ", array);
        }
        static void Main(string[] args)
        {
            Console.ReadLine();
            var input = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
