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
        public bool Leaf;
        public int SuffixStart;
        public int SuffixLength;
    }

    public static class NonSharedSubstring
    {
        private static TreeNode[] BuildTree(string text)
        {
            var origText = text;
            var tree = new List<TreeNode>() { new TreeNode() };
            for (int i = 0; i < origText.Length; i++)
            {
                var current = 0;
                var txti = 0;
                while (true)
                {
                    var p = 0;
                    if (current > 0)
                    {
                        // if it is not root node, skip common prefix
                        while (txti + p < text.Length && p < tree[current].SuffixLength &&
                            text[txti + p] == origText[tree[current].SuffixStart + p])
                        {
                            p++;
                        }
                        txti += p;
                    }
                    if (p == tree[current].SuffixLength)
                    {
                        if (tree[current].Edges.ContainsKey(text[txti]))
                        {
                            current = tree[current].Edges[text[txti]];
                        }
                        else
                        {
                            // add a node and edge from current node
                            tree.Add(new TreeNode()
                            {
                                SuffixStart = i + txti,
                                SuffixLength = origText.Length - i - txti,
                                FirstChar = origText[i + txti].ToString(),
                                Parent = current
                            });
                            tree[current].Edges[origText[i + txti]] = tree.Count - 1;
                            break;
                        }
                    }
                    else
                    {
                        // splitting the node
                        if (current > 0 && p < tree[current].SuffixLength)
                        {
                            if (tree[current].Edges.Count > 0)
                            {
                                var curchar1 = origText[tree[current].SuffixStart];
                                var curchar2 = origText[tree[current].SuffixStart + p];
                                var newnode = new TreeNode()
                                {
                                    SuffixStart = tree[current].SuffixStart,
                                    SuffixLength = p,
                                    FirstChar = curchar1.ToString(),
                                    Parent = tree[current].Parent,
                                    Edges = new Dictionary<char, int>() {
                                        {curchar2, current}
                                    }
                                };
                                tree.Add(newnode);
                                tree[tree[current].Parent].Edges[curchar1] = tree.Count - 1;
                                tree[current].Parent = tree.Count - 1;
                                tree[current].SuffixStart = newnode.SuffixStart + newnode.SuffixLength;
                                tree[current].SuffixLength -= newnode.SuffixLength;
                                tree[current].FirstChar = curchar2.ToString();
                                current = tree.Count - 1;
                            }
                            else
                            {
                                var newnode = new TreeNode()
                                {
                                    SuffixStart = tree[current].SuffixStart + p,
                                    SuffixLength = origText.Length - tree[current].SuffixStart - p,
                                    FirstChar = origText[tree[current].SuffixStart + p].ToString(),
                                    Parent = current
                                };
                                tree.Add(newnode);
                                tree[current].Edges[origText[newnode.SuffixStart]] = tree.Count - 1;
                                tree[current].SuffixLength -= newnode.SuffixLength;
                            }
                        }
                        // add a node and edge from current node
                        tree.Add(new TreeNode()
                        {
                            SuffixStart = i + txti,
                            SuffixLength = origText.Length - i - txti,
                            FirstChar = origText[i + txti].ToString(),
                            Parent = current
                        });
                        tree[current].Edges[origText[i + txti]] = tree.Count - 1;
                        break;
                    }
                }
                text = text.Substring(1);
            }

            var len = origText.Length / 2 - 1;
            for (int i = 0; i < tree.Count; i++)
            {
                if (tree[i] != null)
                {
                    while (tree[i].Edges.Count == 1)
                    {
                        var edge = tree[i].Edges.Single();
                        tree[i].SuffixLength += tree[edge.Value].SuffixLength;
                        tree[i].Edges = tree[edge.Value].Edges;
                        tree[edge.Value] = null;
                        foreach (var e in tree[i].Edges)
                        {
                            tree[e.Value].Parent = i;
                        }
                    }
                    if (tree[i].Edges.Count == 0)
                    {
                        tree[i].Leaf = true;
                    }
                }
            }

            for (int i = tree.Count - 1; i >= 0; i--)
            {
                var node = tree[i];
                if (node == null || !node.Leaf) continue;
                var path = "";
                //find path and remove nodes with all-left leaves
                while (node.Parent > 0)
                {
                    if (tree[node.Parent].Edges.All(e => tree[e.Value].Leaf && tree[e.Value].SuffixStart <= len))
                    {
                        tree[node.Parent].Edges.Clear();
                        tree[node.Parent].Leaf = true;
                        path = "";
                    }
                    path = origText.Substring(tree[node.Parent].SuffixStart, tree[node.Parent].SuffixLength) + path;
                    node = tree[node.Parent];
                }
                tree[i].Path = path;
            }
            return tree.ToArray();
        }

        public static string Solve(string line1, string line2)
        {
            var combinedString = line1 + "#" + line2 + "$";
            var tree = BuildTree(combinedString);
            var minsub = line1;

            for (int i = 1; i < tree.Length; i++)
            {
                if (tree[i] != null && tree[i].Leaf && tree[i].SuffixStart < line1.Length)
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
