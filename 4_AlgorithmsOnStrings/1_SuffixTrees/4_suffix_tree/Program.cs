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
    }

    public static class SuffixTree
    {
        private static TreeNode[] BuildTrie(string text)
        {
            var trie = new List<TreeNode>() { new TreeNode() };
            var length = text.Length;
            for (int i = 0; i < length; i++)
            {
                var current = 0;
                for (int j = 0; j < text.Length; j++)
                {
                    if (trie[current].Edges.ContainsKey(text[j]))
                    {
                        current = trie[current].Edges[text[j]];
                    }
                    else
                    {
                        var next = trie.Count;
                        trie[current].Edges[text[j]] = next;
                        current = next;
                        trie.Add(new TreeNode()
                        {
                            SuffixStart = i + j,
                            SuffixLength = 1
                        });
                    }
                }
                text = text.Substring(1);
            }
            return trie.ToArray();
        }

        private static TreeNode[] BuildTree2(string text)
        {
            var trie = BuildTrie(text);
            for (int i = trie.Length - 1; i >= 0; i--)
            {
                if (trie[i].Edges.Count == 1)
                {
                    var edge = trie[i].Edges.First().Value;
                    trie[i].SuffixLength = trie[edge].SuffixLength + 1;
                    trie[edge] = null;
                    trie[i].Edges.Clear();
                }
            }
            return trie.Where(t => t != null && t.SuffixLength > 0).ToArray();
        }

        private static TreeNode[] BuildTree(string text)
        {
            var tree = new List<TreeNode>() { new TreeNode() };
            var origText = text;
            for (int i = 0; i < origText.Length; i++)
            {
                var current = 0;
                var depth = 0;
                while (true)
                {
                    if (tree[current].Edges.ContainsKey(text[depth]))
                    {
                        current = tree[current].Edges[text[depth]];
                        if (tree[current].Edges.Count == 0)
                        {
                            var next = tree.Count;
                            tree.Add(new TreeNode()
                            {
                                SuffixStart = tree[current].SuffixStart + 1,
                                SuffixLength = tree[current].SuffixLength - 1
                            });
                            tree[current].Edges.Add(origText[tree[next].SuffixStart], next);
                            tree[current].SuffixLength = 1;
                        }
                        depth++;
                    }
                    else
                    {
                        var next = tree.Count;
                        tree[current].Edges[text[depth]] = next;
                        tree.Add(new TreeNode()
                        {
                            SuffixStart = i + depth,
                            SuffixLength = text.Length - depth
                        });
                        current = next;
                        break;
                    }
                }
                text = text.Substring(1);
            }
            return tree.Where(t => t != null && t.SuffixLength > 0).ToArray();
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
