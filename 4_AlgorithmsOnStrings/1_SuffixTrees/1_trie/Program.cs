using System.Collections.Generic;
using System;

namespace _1_trie
{
    public static class Trie
    {
        public static string[] Solve(string[] patterns)
        {
            var trie = new List<Dictionary<char, int>>();
            trie.Add(new Dictionary<char, int>());
            var count = 0;
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
            var result = new List<string>(count);
            for (int i = 0; i < trie.Count; i++)
            {
                foreach (var edge in trie[i])
                {
                    result.Add($"{i}->{edge.Value}:{edge.Key}");
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
