using System.Collections.Generic;
using System;
using System.Linq;

namespace _3_merging_tables
{
    public class Table
    {
        public int Index;
        public int Size;
        public Table(int index, int size)
        {
            Index = index;
            Size = size;
        }
    }

    public class DisjoinSet
    {
        private int maxSize = int.MinValue;
        private readonly Table[] tables;
        public Table Find(int i)
        {
            if (tables[i].Size >= 0) return tables[i];
            var realTable = Find(tables[i].Index);
            tables[i].Index = realTable.Index;
            return realTable;
        }

        public DisjoinSet(int[] data)
        {
            tables = data.Select((x, i) =>
            {
                if (x > maxSize) maxSize = x;
                return new Table(i, x);
            }).ToArray();
        }

        public int Merge(int dst, int src)
        {
            if (src != dst)
            {
                var source = Find(src - 1);
                var destination = Find(dst - 1);
                if (source.Index != destination.Index)
                {
                    destination.Size += source.Size;
                    if (destination.Size > maxSize) maxSize = destination.Size;
                    source.Index = destination.Index;
                    source.Size = -1;
                }
            }
            return maxSize;
        }
    }

    public static class MergingTables
    {
        public static string[] Solve(string line1, string line2, string[] queries)
        {
            var data = line2.Split(' ').Select(x => int.Parse(x)).ToArray();
            var set = new DisjoinSet(data);
            var result = new string[queries.Length];
            for (int i = 0; i < queries.Length; i++)
            {
                var query = queries[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                result[i] = set.Merge(query[0], query[1]).ToString();
            }
            return result;
        }

        static void Main(string[] args)
        {
            var line1 = Console.ReadLine();
            var line2 = Console.ReadLine();

            var queryCount = int.Parse(line1.Split(' ')[1]);
            var queries = new string[queryCount];
            for (int i = 0; i < queryCount; i++)
            {
                queries[i] = Console.ReadLine();
            }
            foreach (var line in Solve(line1, line2, queries))
            {
                Console.WriteLine(line);
            }
        }
    }
}
