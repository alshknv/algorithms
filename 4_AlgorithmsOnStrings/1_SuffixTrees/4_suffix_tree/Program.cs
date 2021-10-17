using System.Collections.Generic;
using System;
using System.Linq;

namespace _4_suffix_tree
{
    public class TreeNode
    {
        public Dictionary<char, int> Edges = new Dictionary<char, int>();
        public int SuffixStart;
        public int SuffixLength;
        public int? SuffixIndex;
    }

    public static class SuffixTree
    {
        private static TreeNode[] BuildTrie(string text)
        {
            var trie = new List<TreeNode>() { new TreeNode() };
            for (int i = 0; i < text.Length; i++)
            {
                var current = 0;
                var suffix = text.Skip(i).ToArray();
                for (int j = 0; j < suffix.Length; j++)
                {
                    if (trie[current].Edges.ContainsKey(suffix[j]))
                    {
                        current = trie[current].Edges[suffix[j]];
                    }
                    else
                    {
                        var next = trie.Count;
                        trie[current].Edges[suffix[j]] = next;
                        current = next;
                        trie.Add(new TreeNode()
                        {
                            SuffixStart = i + j,
                            SuffixLength = 1,
                            SuffixIndex = i
                        });
                    }
                }
            }
            return trie.ToArray();
        }

        private static TreeNode[] BuildTree(string text)
        {
            var trie = BuildTrie(text);
            for (int i = trie.Length - 1; i >= 0; i--)
            {
                if (trie[i].Edges.Count == 1)
                {
                    trie[i].SuffixIndex = trie[trie[i].Edges.First().Value].SuffixIndex;
                    trie[i].SuffixLength = trie[trie[i].Edges.First().Value].SuffixLength + 1;
                    trie[trie[i].Edges.First().Value] = null;
                    trie[i].Edges.Clear();
                }
            }
            return trie.Where(t => t?.SuffixLength > 0).ToArray();
        }

        public static string[] Solve(string text)
        {
            var tree = BuildTree(text);
            var suffices = new List<string>();
            for (int i = tree.Length - 1; i >= 0; i--)
            {
                suffices.Add(text.Substring(tree[i].SuffixStart, tree[i].SuffixLength));
            }
            return suffices.ToArray();
        }

        static void Main(string[] args)
        {
            foreach (var line in Solve(Console.ReadLine()))
            {
                Console.WriteLine(line);
            }
        }
    }
}
