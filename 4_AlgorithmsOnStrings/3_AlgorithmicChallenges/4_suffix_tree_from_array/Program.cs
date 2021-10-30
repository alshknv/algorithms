using System.Linq;
using System.Collections.Generic;
using System;

namespace _4_suffix_tree_from_array
{
    public class Node
    {
        public Node Parent;
        public SortedDictionary<char, Node> Children = new SortedDictionary<char, Node>();
        public int Depth;
        public int Start;
        public int End;
        public bool Printed;
    }

    public static class SuffixTreeFromArray
    {
        private static Node CreateNewLeaf(Node node, string text, int suffix)
        {
            var leaf = new Node()
            {
                Parent = node,
                Depth = text.Length - suffix,
                Start = suffix + node.Depth,
                End = text.Length - 1
            };
            node.Children[text[leaf.Start]] = leaf;
            return leaf;
        }

        private static Node BreakEdge(Node node, string text, int start, int offset)
        {
            var startCh = text[start];
            var midCh = text[start + offset];
            var midNode = new Node()
            {
                Parent = node,
                Depth = node.Depth + offset,
                Start = start,
                End = start + offset - 1
            };
            midNode.Children[midCh] = node.Children[startCh];
            node.Children[startCh].Parent = midNode;
            node.Children[startCh].Start += offset;
            node.Children[startCh] = midNode;
            return midNode;
        }

        private static Node TreeFromArray(string text, int[] order, int[] lcpArray)
        {
            var root = new Node() { Start = -1, End = -1 };
            var lcpPrev = 0;
            var curNode = root;
            for (int i = 0; i < text.Length; i++)
            {
                var suffix = order[i];
                while (curNode.Depth > lcpPrev) curNode = curNode.Parent;
                if (curNode.Depth == lcpPrev)
                {
                    curNode = CreateNewLeaf(curNode, text, suffix);
                }
                else
                {
                    var edgeStart = order[i - 1] + curNode.Depth;
                    var offset = lcpPrev - curNode.Depth;
                    var midNode = BreakEdge(curNode, text, edgeStart, offset);
                    curNode = CreateNewLeaf(midNode, text, suffix);
                }
                if (i < text.Length - 1) lcpPrev = lcpArray[i];
            }
            return root;
        }

        public static string[] Solve(string text, string saString, string lcpAString)
        {
            var tree = TreeFromArray(
                text,
                saString.Split(' ').Select(x => int.Parse(x)).ToArray(),
                lcpAString.Split(' ').Select(x => int.Parse(x)).ToArray());

            var result = new List<string>() { text };
            var stack = new Stack<Node>();
            stack.Push(tree);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (node.Parent != null && !node.Printed)
                {
                    result.Add(node.Start.ToString() + " " + (node.End + 1).ToString());
                    node.Printed = true;
                }
                var children = node.Children.ToArray();
                for (int i = children.Length - 1; i >= 0; i--)
                {
                    stack.Push(children[i].Value);
                }
            }
            return result.ToArray();
        }

        static void Main(string[] args)
        {
            var text = Console.ReadLine();
            var suffixArray = Console.ReadLine();
            var lcpArray = Console.ReadLine();
            var result = Solve(text, suffixArray, lcpArray);
            foreach (var line in result)
            {
                Console.WriteLine(line);
            }
        }
    }
}
