using System.Collections.Generic;
using System;
using System.Linq;

namespace _2_trie_matching
{
    public static class TrieMatching
    {
        private static List<Dictionary<char, int>> BuildTrie(string[] patterns)
        {
            var trie = new List<Dictionary<char, int>>() { new Dictionary<char, int>() };
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
                    }
                }
            }
            return trie;
        }

        private static bool PrefixTrieMatching(string text, List<Dictionary<char, int>> trie)
        {
            var k = 0;
            var v = 0;
            while (true)
            {
                var symbol = k < text.Length ? text[k] : '$';
                if (trie[v].Count == 0)
                {
                    // v is a leaf, it's a match
                    return true;
                }
                else if (trie[v].ContainsKey(symbol))
                {
                    // continue going down
                    k++;
                    v = trie[v][symbol];
                }
                else
                {
                    // no match
                    return false;
                }
            }
        }

        public static string Solve(string text, string[] patterns)
        {
            var matches = new List<int>();
            var trie = BuildTrie(patterns);
            var length = text.Length;
            for (int k = 0; k < length; k++)
            {
                if (PrefixTrieMatching(text, trie))
                    matches.Add(k);
                text = text.Substring(1);
            }
            return string.Join(" ", matches);
        }

        static void Main(string[] args)
        {
            var text = Console.ReadLine();
            var n = int.Parse(Console.ReadLine());
            var patterns = new string[n];
            for (int i = 0; i < n; i++)
            {
                patterns[i] = Console.ReadLine();
            }
            Console.WriteLine(Solve(text, patterns));
        }
    }
}
