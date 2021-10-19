using System.Collections.Generic;
using System;
using System.Linq;

namespace _4_suffix_tree
{
    public class TreeNode
    {
        public Dictionary<char, int> Edges = new Dictionary<char, int>();
        public int Parent;
        public int SuffixStart;
        public int SuffixLength;
    }

    public static class SuffixTree
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
                                current = tree.Count - 1;
                            }
                            else
                            {
                                var newnode = new TreeNode()
                                {
                                    SuffixStart = tree[current].SuffixStart + p,
                                    SuffixLength = origText.Length - tree[current].SuffixStart - p,
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
                            Parent = current
                        });
                        tree[current].Edges[origText[i + txti]] = tree.Count - 1;
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
