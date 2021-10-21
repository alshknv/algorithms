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
        public int PathLength;
        public bool Leaf;
        public bool Left;
        public int SuffixStart;
        public int SuffixLength;
    }

    public static class NonSharedSubstring
    {
        private static void UpdateLeft(List<TreeNode> tree, int node)
        {
            while (tree[node].Parent > 0)
            {
                var parent = tree[node].Parent;
                tree[parent].Left &= tree[node].Left;
                node = parent;
            }
        }

        private static void UpdatePath(List<TreeNode> tree, int node)
        {
            var queue = new Queue<int>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var n = queue.Dequeue();
                tree[n].PathLength = tree[tree[n].Parent].PathLength + (tree[n].Leaf ? 1 : tree[n].SuffixLength);
                foreach (var e in tree[n].Edges)
                {
                    queue.Enqueue(e.Value);
                }
            }
        }

        private static TreeNode[] BuildTree(string text)
        {
            var origText = text;
            var lefti = text.Length / 2 - 1;
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
                            continue;
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
                                    },
                                    Leaf = false,
                                    Left = tree[current].Left
                                };
                                tree.Add(newnode);
                                tree[tree[current].Parent].Edges[curchar1] = tree.Count - 1;
                                tree[current].Parent = tree.Count - 1;
                                tree[current].SuffixStart = newnode.SuffixStart + newnode.SuffixLength;
                                tree[current].SuffixLength -= newnode.SuffixLength;
                                tree[current].FirstChar = curchar2.ToString();
                                current = tree.Count - 1;
                                UpdatePath(tree, tree.Count - 1);
                            }
                            else
                            {
                                var newnode = new TreeNode()
                                {
                                    SuffixStart = tree[current].SuffixStart + p,
                                    SuffixLength = origText.Length - tree[current].SuffixStart - p,
                                    FirstChar = origText[tree[current].SuffixStart + p].ToString(),
                                    Parent = current,
                                    Leaf = true,
                                    Left = tree[current].SuffixStart + p <= lefti
                                };
                                tree[current].SuffixLength -= newnode.SuffixLength;
                                newnode.PathLength = tree[current].PathLength + 1;
                                tree.Add(newnode);
                                tree[current].Leaf = false;
                                tree[current].Edges[origText[newnode.SuffixStart]] = tree.Count - 1;
                                UpdateLeft(tree, tree.Count - 1);
                                UpdatePath(tree, current);
                            }
                        }
                    }

                    // add a node and edge from current node
                    tree.Add(new TreeNode()
                    {
                        SuffixStart = i + txti,
                        SuffixLength = origText.Length - i - txti,
                        FirstChar = origText[i + txti].ToString(),
                        Parent = current,
                        Leaf = true,
                        Left = i + txti <= lefti,
                        PathLength = tree[current].PathLength + 1
                    });
                    tree[current].Edges[origText[i + txti]] = tree.Count - 1;
                    tree[current].Leaf = false;
                    UpdateLeft(tree, tree.Count - 1);
                    break;
                }
                text = text.Substring(1);
            }
            return tree.ToArray();
        }

        public static string Solve(string line1, string line2)
        {
            var combinedString = line1 + "#" + line2 + "$";
            var tree = BuildTree(combinedString);
            var minlen = int.MaxValue;
            var mini = 0;

            for (int i = 0; i < tree.Length; i++)
            {
                if (tree[i].Left && tree[i].SuffixStart < line1.Length && tree[i].PathLength < minlen)
                {
                    minlen = tree[i].PathLength;
                    mini = i;
                }
            }
            var node = mini;
            var result = "";
            while (node > 0)
            {
                result = (tree[node].Leaf ? tree[node].FirstChar : line1.Substring(tree[node].SuffixStart, tree[node].SuffixLength)) + result;
                node = tree[node].Parent;
            }
            return result;
        }

        static void Main(string[] args)
        {
            var line1 = Console.ReadLine();
            var line2 = Console.ReadLine();
            Console.WriteLine(Solve(line1, line2));
        }
    }
}
