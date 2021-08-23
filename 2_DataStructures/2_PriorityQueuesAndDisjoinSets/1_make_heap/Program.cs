using System;
using System.Collections.Generic;
using System.Linq;

namespace _1_make_heap
{
    public class MakeHeap
    {
        private static void Swap(long[] array, int a, int b)
        {
            var buf = array[a];
            array[a] = array[b];
            array[b] = buf;
        }

        private static int Parent(int i)
        {
            return (i - 1) / 2;
        }

        private static string[] SiftDown(long[] array, int i)
        {
            var swaps = new List<string>();
            while (i < array.Length)
            {
                var c1 = 2 * i + 1;
                var c2 = 2 * i + 2;
                var nextI = c1 < array.Length && array[c1] < array[i] ? c1 : i;
                nextI = c2 < array.Length && array[c2] < array[nextI] ? c2 : nextI;
                if (nextI != i)
                {
                    Swap(array, i, nextI);
                    swaps.Add($"{i} {nextI}");
                    i = nextI;
                }
                else
                {
                    break;
                }
            }
            return swaps.ToArray();
        }

        public static string[] Solve(string input)
        {
            var array = input.Split(' ').Select(x => long.Parse(x)).ToArray();
            var result = new List<string>() { "0" };
            for (int i = (array.Length - 1) / 2; i >= 0; i--)
            {
                var swaps = SiftDown(array, i);
                result.AddRange(swaps);
            }
            result[0] = (result.Count - 1).ToString();
            return result.ToArray();
        }

        static void Main(string[] args)
        {
            Console.ReadLine();
            var input = Console.ReadLine();
            var result = Solve(input);
            foreach (var line in result)
            {
                Console.WriteLine(line);
            }
        }
    }
}
