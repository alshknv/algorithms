using System.Collections.Generic;
using System;
using System.Linq;

namespace _5_non_shared_substring
{
    public class TreeNode
    {
        public Dictionary<char, int> Edges = new Dictionary<char, int>();
        public int Parent;
        public int SuffixStart;
        public int SuffixLength;
        public int? SuffixIndex;
    }

    public static class NonSharedSubstring
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
                        trie.Add(new TreeNode()
                        {
                            SuffixStart = i + j,
                            SuffixLength = 1,
                            SuffixIndex = i,
                            Parent = current
                        });
                        current = next;
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
                if (trie[i].Edges.Count == 1 && trie[trie[i].Edges.First().Value].Edges.Count == 0)
                {
                    trie[i].SuffixIndex = trie[trie[i].Edges.First().Value].SuffixIndex;
                    trie[i].SuffixLength = trie[trie[i].Edges.First().Value].SuffixLength + 1;
                    trie[trie[i].Edges.First().Value] = null;
                    trie[i].Edges.Clear();
                }
            }
            return trie.ToArray();
        }

        public static string Solve(string line1, string line2)
        {
            var combinedString = line1 + "#" + line2 + "$";
            var tree = BuildTree(combinedString);
            var minsub = line1;
            for (int i = tree.Length - 1; i > 0; i--)
            {
                if (tree[i]?.Edges.Count == 0 && tree[i].SuffixStart >= line1.Length)
                {
                    var node = tree[i];
                    while (node != null)
                    {
                        foreach (var key in tree[node.Parent].Edges.Keys.ToArray())
                        {
                            if (tree[tree[node.Parent].Edges[key]].SuffixStart >= line1.Length)
                            {
                                tree[tree[node.Parent].Edges[key]] = null;
                                tree[node.Parent].Edges.Remove(key);
                            }
                        }
                        if (node.SuffixIndex == null || tree[node.Parent].Edges.Count > 0) break;
                        node = tree[node.Parent];
                    }
                }
            }

            for (int i = 0; i < tree.Length; i++)
            {
                if (tree[i]?.Edges.Count == 0 && tree[i].SuffixStart <= line1.Length)
                {
                    var leafSymb = "";
                    if (tree[i].SuffixStart == line1.Length && tree[tree[i].Parent].Edges.Any(e =>
                        tree[e.Value].Edges.Count == 0 && tree[e.Value].SuffixStart > line1.Length))
                    {
                        continue;
                    }
                    else if (!tree[tree[i].Parent].Edges.All(e =>
                        tree[e.Value].Edges.Count == 0 && tree[e.Value].SuffixStart <= line1.Length))
                    {
                        leafSymb = combinedString[tree[i].SuffixStart].ToString();
                        if (leafSymb == "#") leafSymb = "";
                    }
                    var node = tree[i];
                    var path = "";
                    while (node.Parent > 0)
                    {
                        path = combinedString[tree[node.Parent].SuffixStart].ToString() + path;
                        node = tree[node.Parent];
                    }
                    var sub = path + leafSymb;
                    if (sub.Length < minsub.Length)
                    {
                        minsub = sub;
                    }
                }
            }
            return minsub;
        }

        static void Main(string[] args)
        {
            var gg = Solve("CCAAGCTGCTAGAGG", "CATGCTGGGCTGGCT");
            var line1 = Console.ReadLine();
            var line2 = Console.ReadLine();
            Console.WriteLine(Solve(line1, line2));
        }
    }
}
