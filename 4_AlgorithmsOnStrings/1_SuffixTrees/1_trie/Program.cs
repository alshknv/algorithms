using System.Collections.Generic;
using System;

namespace _1_trie
{
    public static class Trie
    {
        private static List<Dictionary<char, int>> BuildTrie(string[] patterns, out int count)
        {
            var trie = new List<Dictionary<char, int>>() { new Dictionary<char, int>() };
            count = 0;
            for (int i = 0; i < patterns.Length; i++)
            {
                var current = 0;
                for (int j = 0; j < patterns[i].Length; j++)
                {
                    if (trie[current].ContainsKey(patterns[i][j]))
                    {
                        current = trie[current][patterns[i][j]];
                    }
                    else
                    {
                        var next = trie.Count;
                        trie[current][patterns[i][j]] = next;
                        current = next;
                        trie.Add(new Dictionary<char, int>());
                        count++;
                    }
                }
            }
            return trie;
        }

        public static string[] Solve(string[] patterns)
        {
            int count;
            var trie = BuildTrie(patterns, out count);
            var result = new List<string>(count);
            for (int i = 0; i < trie.Count; i++)
            {
                foreach (var edge in trie[i])
                {
                    result.Add(i.ToString() + "->" + edge.Value.ToString() + ":" + edge.Key);
                }
            }
            return result.ToArray();
        }

        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var patterns = new string[n];
            for (int i = 0; i < n; i++)
            {
                patterns[i] = Console.ReadLine();
            }
            foreach (var line in Solve(patterns))
            {
                Console.WriteLine(line);
            }
        }
    }
}
