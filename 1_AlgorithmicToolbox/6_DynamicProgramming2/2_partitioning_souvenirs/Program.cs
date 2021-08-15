using System.Collections.Generic;
using System.Linq;
using System;

namespace _2_partitioning_souvenirs
{

    public static class PartitioningSouvenirs
    {
        private class MatrixElement
        {
            public int Sum;
            public List<int> Content = new List<int>();
            public MatrixElement(int sum)
            {
                Sum = sum;
            }

            public MatrixElement(MatrixElement e, int? extra = null)
            {
                Sum = e.Sum + (extra ?? 0);
                Content = new List<int>(e.Content);
                if (extra != null)
                {
                    Content.Add((int)extra);
                }
            }
        }

        private static int[] RemoveItems(int[] items, List<int> selected)
        {
            selected.Sort();
            var s = 0;
            var result = new List<int>();

            for (int i = 0; i < items.Length; i++)
            {
                if (s >= selected.Count || selected[s] != items[i])
                {
                    result.Add(items[i]);
                }
                else
                {
                    s++;
                }
            }
            return result.ToArray();
        }

        private static MatrixElement[,] GetMatrix(int[] items, int value)
        {
            var values = new MatrixElement[value + 1, items.Length + 1];
            for (int w = 0; w <= value; w++)
            {
                for (int b = 0; b <= items.Length; b++)
                {
                    if (w == 0 || b == 0)
                    {
                        values[w, b] = new MatrixElement(0);
                    }
                    else
                    {
                        values[w, b] = new MatrixElement(values[w, b - 1]);
                        if (w >= items[b - 1])
                        {
                            var v2 = new MatrixElement(values[w - items[b - 1], b - 1], items[b - 1]);
                            if (v2.Sum >= values[w, b].Sum)
                            {
                                values[w, b] = v2;
                            }
                        }
                    }
                }
            }
            return values;
        }

        public static string Solve(string input)
        {
            var items = input.Split(' ').Select(x => int.Parse(x)).ToArray();
            var sum = items.Sum();
            if (sum % 3 > 0)
            {
                return "0";
            }
            var share = sum / 3;
            Array.Sort(items);
            for (int i = 0; i < 3; i++)
            {
                var matrix = GetMatrix(items, share);
                if (matrix[share, items.Length].Sum != share)
                {
                    return "0";
                }
                items = RemoveItems(items, matrix[share, items.Length].Content);
            }
            return "1";
        }

        static void Main(string[] args)
        {
            Console.ReadLine();
            var input = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
