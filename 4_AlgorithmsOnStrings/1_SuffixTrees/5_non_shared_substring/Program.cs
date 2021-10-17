using System.Collections.Generic;
using System;
using System.Linq;

namespace _5_non_shared_substring
{
    public class TreeNode
    {
        public Dictionary<char, int> Edges = new Dictionary<char, int>();
        public int Parent;
        public string FirstChar;
        public string Path;
        // public bool Leaf;
        public bool Left = true;
        public int SuffixStart;
        public int SuffixLength;
        public int? SuffixIndex;
    }

    public static class NonSharedSubstring
    {
        private static TreeNode[] BuildTrie(string text)
        {
            var trie = new List<TreeNode>() { new TreeNode() };
            var origText = text;
            for (int i = 0; i < origText.Length; i++)
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
                        trie.Add(new TreeNode()
                        {
                            SuffixStart = i + j,
                            SuffixLength = 1,
                            SuffixIndex = i,
                            Parent = current,
                            FirstChar = origText[i + j].ToString()
                        });
                        current = next;
                    }
                }
                text = text.Substring(1);
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
                    //trie[i].Leaf = true;
                    trie[i].Edges.Clear();
                }
                if (trie[trie[i].Parent].Edges.Count > 0)
                {
                    var node = trie[i];
                    var path = "";
                    while (node.Parent > 0)
                    {
                        path = text[trie[node.Parent].SuffixStart].ToString() + path;
                        node = trie[node.Parent];
                    }
                    trie[i].Path = path;
                }
            }
            return trie.ToArray();
        }

        public static string Solve(string line1, string line2)
        {
            var combinedString = line1 + "#" + line2 + "$";
            var tree = BuildTree(combinedString);
            var minsub = line1;

            // remove all nodes and edges leading to substrings of line2
            // if some node lose all its edges, than remove this path from its parent
            for (int i = tree.Length - 1; i > 0; i--)
            {
                if (tree[i] != null && tree[i].Edges.Count == 0 && tree[i].SuffixStart > line1.Length)
                {
                    var node = tree[i];
                    while (node != null)
                    {
                        if (tree[node.Parent] == null)
                        {
                            tree[i] = null;
                            break;
                        }
                        tree[node.Parent].Left = false;
                        foreach (var key in tree[node.Parent].Edges.Keys.ToArray())
                        {
                            if (tree[tree[node.Parent].Edges[key]].SuffixStart >= line1.Length)
                            {
                                tree[tree[node.Parent].Edges[key]] = null;
                                tree[node.Parent].Edges.Remove(key);
                            }
                        }
                        if (node.SuffixIndex == null || tree[node.Parent].Edges.Count > 0) break;
                        //tree[node.Parent].Leaf = true;
                        node = tree[node.Parent];
                        node.SuffixStart = line1.Length + 1;
                    }
                }
                else if (tree[i] != null && tree[i].Edges.Count > 0 && tree[i].Left && tree[i].Edges.All(e => tree[e.Value].Left))
                {
                    foreach (var key in tree[i].Edges.Keys.ToArray())
                    {
                        tree[tree[i].Edges[key]] = null;
                        tree[i].Edges.Remove(key);
                    }
                    //tree[i].Leaf = true;
                }
                /*else if (tree[i] != null && tree[i].Edges.Count > 0 &&
                    tree[i].Edges.Any(e => tree[e.Value].Leaf && tree[e.Value].SuffixStart == line1.Length))
                {
                    foreach (var key in tree[i].Edges.Keys.ToArray())
                    {
                        if (key == '#')
                        {
                            tree[tree[i].Edges[key]] = null;
                            tree[i].Edges.Remove(key);
                        }
                    }
                }*/
            }

            // only paths to substrings of line1 that are not present in line2 left
            // choose shortest one

            for (int i = 0; i < tree.Length; i++)
            {
                if (tree[i] != null && tree[i].Edges.Count == 0)
                {
                    var sub = tree[i].Path + tree[i].FirstChar;
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
            var line1 = Console.ReadLine();
            var line2 = Console.ReadLine();
            Console.WriteLine(Solve(line1, line2));
        }
    }
}
