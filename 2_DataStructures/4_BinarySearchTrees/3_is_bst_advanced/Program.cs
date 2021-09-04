using System.Collections.Generic;
using System;

namespace _3_is_bst_advanced
{
    public static class IsBstAdvanced
    {
        public class Node
        {
            public long Key;
            public Node Left;
            public Node Right;
            public bool Calc;
            public bool? Correct;
            public long? MinChild;
            public long? MaxChild;
            public long? MaxLeftChild;
            public long? MinRightChild;
        }

        private static Node BuildTree(string[] vertices)
        {
            var tree = new Node[vertices.Length];
            var index = 0;
            foreach (string vertex in vertices)
            {
                var vertexData = vertex.Split(' ');
                var leftIdx = int.Parse(vertexData[1]);
                var rightIdx = int.Parse(vertexData[2]);
                if (tree[index] == null) tree[index] = new Node();
                tree[index].Key = long.Parse(vertexData[0]);
                if (leftIdx >= 0)
                {
                    if (tree[leftIdx] == null) tree[leftIdx] = new Node();
                    tree[index].Left = tree[leftIdx];
                }
                if (rightIdx >= 0)
                {
                    if (tree[rightIdx] == null) tree[rightIdx] = new Node();
                    tree[index].Right = tree[rightIdx];
                }
                index++;
            }
            return tree.Length > 0 ? tree[0] : null;
        }

        private static void Traverse(Node node)
        {
            if (node == null) return;
            Stack<Node> nodeStack = new Stack<Node>();
            nodeStack.Push(node);
            while (nodeStack.Count > 0)
            {
                var n = nodeStack.Pop();
                if (!n.Calc)
                {
                    n.Calc = true;
                    nodeStack.Push(n);
                    if (n.Right != null) nodeStack.Push(n.Right);
                    if (n.Left != null) nodeStack.Push(n.Left);
                }
                else
                {
                    n.Correct = (n.Right?.Correct ?? true) && (n.Left?.Correct ?? true);
                    if (n?.Correct ?? true)
                    {
                        n.MaxLeftChild = Math.Max(n.Left?.Key ?? long.MinValue, n.Left?.MaxChild ?? long.MinValue);
                        n.MinRightChild = Math.Min(n.Right?.Key ?? long.MaxValue, n.Right?.MinChild ?? long.MaxValue);
                        n.MinChild = Math.Min(
                            (long)n.MinRightChild,
                            Math.Min(n.Left?.Key ?? long.MaxValue, n.Left?.MinChild ?? long.MaxValue));
                        n.MaxChild = Math.Max(
                            Math.Max(n.Right?.Key ?? long.MinValue, n.Right?.MaxChild ?? long.MinValue),
                            (long)n.MaxLeftChild);
                        n.Correct = n.MinRightChild >= n.Key && n.MaxLeftChild < n.Key;
                    }
                }
            }
        }

        public static string Solve(string[] vertices)
        {
            var tree = BuildTree(vertices);
            Traverse(tree);
            return (tree?.Correct ?? true) ? "CORRECT" : "INCORRECT";
        }

        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var input = new string[n];
            for (int i = 0; i < n; i++)
                input[i] = Console.ReadLine();
            foreach (var line in Solve(input))
            {
                Console.WriteLine(line);
            }
        }
    }
}
