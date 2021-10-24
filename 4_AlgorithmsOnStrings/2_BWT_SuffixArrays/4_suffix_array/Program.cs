using System.Collections.Generic;
using System;
using System.Linq;

namespace _4_suffix_array
{
    public class TreeNode
    {
        public SortedDictionary<char, int> Edges = new SortedDictionary<char, int>();
        public int Parent;
        public int SuffixStart;
        public int SuffixLength;
        public int SuffixIndex;
    }

    public static class SuffixArray
    {
        private static TreeNode[] BuildTree(string text)
        {
            var origText = text;
            var tree = new List<TreeNode>() { new TreeNode() { SuffixIndex = -1 } };
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
                                    Parent = tree[current].Parent,
                                    Edges = new SortedDictionary<char, int>() {
                                        {curchar2, current}
                                    },
                                    SuffixIndex = -1
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
                                    Parent = current,
                                    SuffixIndex = tree[current].SuffixIndex
                                };
                                tree.Add(newnode);
                                tree[current].Edges[origText[newnode.SuffixStart]] = tree.Count - 1;
                                tree[current].SuffixLength -= newnode.SuffixLength;
                                tree[current].SuffixIndex = -1;
                            }
                        }
                    }

                    // add a node and edge from current node
                    tree.Add(new TreeNode()
                    {
                        SuffixStart = i + txti,
                        SuffixLength = origText.Length - i - txti,
                        Parent = current,
                        SuffixIndex = i
                    });
                    tree[current].Edges[origText[i + txti]] = tree.Count - 1;
                    tree[current].SuffixIndex = -1;
                    break;
                }
                text = text.Substring(1);
            }

            return tree.ToArray();
        }

        public static string Solve(string text)
        {
            var tree = BuildTree(text);
            //tree traverse
            var stack = new Stack<int>();
            stack.Push(0);
            var result = new Stack<int>();
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (tree[node].SuffixIndex >= 0) result.Push(tree[node].SuffixIndex);
                foreach (var e in tree[node].Edges)
                    stack.Push(e.Value);
            }
            return string.Join(" ", result);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
