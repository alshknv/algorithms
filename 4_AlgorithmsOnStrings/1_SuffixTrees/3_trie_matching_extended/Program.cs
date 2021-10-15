﻿using System.Collections.Generic;
using System;
using System.Linq;

namespace _3_trie_matching_extended
{
    public static class TrieMatchingExtended
    {
        private static List<Dictionary<char, Tuple<int, bool>>> BuildTrie(string[] patterns)
        {
            var trie = new List<Dictionary<char, Tuple<int, bool>>>() { new Dictionary<char, Tuple<int, bool>>() };
            for (int i = 0; i < patterns.Length; i++)
            {
                var current = 0;
                for (int j = 0; j < patterns[i].Length; j++)
                {
                    if (trie[current].ContainsKey(patterns[i][j]))
                    {
                        if (j == patterns[i].Length - 1)
                        {
                            trie[current][patterns[i][j]] = new Tuple<int, bool>(trie[current][patterns[i][j]].Item1, true);
                        }
                        current = trie[current][patterns[i][j]].Item1;
                    }
                    else
                    {
                        var next = trie.Count;
                        trie[current][patterns[i][j]] = new Tuple<int, bool>(next, false);
                        current = next;
                        trie.Add(new Dictionary<char, Tuple<int, bool>>());
                    }
                }
            }
            return trie;
        }

        private static bool PrefixTrieMatching(char[] text, List<Dictionary<char, Tuple<int, bool>>> trie)
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
                    // if true it's a inner leaf
                    if (trie[v][symbol].Item2) return true;
                    // else continue going down
                    k++;
                    v = trie[v][symbol].Item1;
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
            var k = 0;
            while (k < text.Length)
            {
                if (PrefixTrieMatching(text.Skip(k).ToArray(), trie))
                    matches.Add(k);
                k++;
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
