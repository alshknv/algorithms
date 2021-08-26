using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

namespace _2_hash_chains
{
    public class StringHT
    {
        private readonly LinkedList<string>[] table;
        private readonly int buckets;
        private readonly int x = 263;
        private readonly int prime = 1000000007;

        private int Hash(string value)
        {
            var stringBytes = Encoding.ASCII.GetBytes(value);
            long hash = 0;
            for (int i = stringBytes.Length - 1; i >= 0; i--)
            {
                hash = (hash * x + stringBytes[i]) % prime;
            }
            return (int)hash % buckets;
        }

        public StringHT(int bucketCount)
        {
            buckets = bucketCount;
            table = new LinkedList<string>[buckets];
        }

        public void Add(string value)
        {
            var hash = Hash(value);
            if (table[hash] == null)
            {
                table[hash] = new LinkedList<string>();
                table[hash].AddFirst(value);
            }
            else
            {
                var node = table[hash].Find(value);
                if (node != null)
                {
                    node.Value = value;
                }
                else
                {
                    table[hash].AddFirst(value);
                }
            }
        }

        public string Find(string value)
        {
            var hash = Hash(value);
            return table[hash]?.Find(value) != null ? "yes" : "no";
        }

        public void Delete(string value)
        {
            var hash = Hash(value);
            if (table[hash] != null)
            {
                var node = table[hash].Find(value);
                if (node != null)
                {
                    table[hash].Remove(node);
                    if (table[hash].Count == 0)
                    {
                        table[hash] = null;
                    }
                }
            }
        }

        public string Check(int i)
        {
            if (table[i] == null) return "";
            return string.Join(" ", table[i]);
        }
    }

    public static class HashChains
    {
        public static string[] Solve(int buckets, string[] queries)
        {
            var output = new List<string>();
            var ht = new StringHT(buckets);
            foreach (var query in queries)
            {
                var queryParts = query.Split(' ');
                switch (queryParts[0])
                {
                    case "add":
                        ht.Add(queryParts[1]);
                        break;
                    case "del":
                        ht.Delete(queryParts[1]);
                        break;
                    case "find":
                        output.Add(ht.Find(queryParts[1]));
                        break;
                    case "check":
                        output.Add(ht.Check(int.Parse(queryParts[1])));
                        break;
                }
            }
            return output.ToArray();
        }

        static void Main(string[] args)
        {
            var buckets = int.Parse(Console.ReadLine());
            var N = int.Parse(Console.ReadLine());
            var queries = new string[N];
            for (int i = 0; i < N; i++)
            {
                queries[i] = Console.ReadLine();
            }
            foreach (var line in Solve(buckets, queries))
            {
                Console.WriteLine(line);
            }
        }
    }
}
