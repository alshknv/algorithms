using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace _1_tree_traversals
{
    public class Node
    {
        public long Key;
        public Node Left;
        public Node Right;
    }

    public class TreeTraversals
    {
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
            return tree[0];
        }

        private static string TraverseInOrderIterative(Node node, int treeSize)
        {
            Stack<object> nodeStack = new Stack<object>();
            var traversal = new long[treeSize];
            var count = 0;
            nodeStack.Push(node);
            while (nodeStack.Count > 0)
            {
                var obj = nodeStack.Pop();
                if (obj is Node)
                {
                    if ((obj as Node).Right != null) nodeStack.Push((obj as Node).Right);
                    nodeStack.Push((obj as Node).Key);
                    if ((obj as Node).Left != null) nodeStack.Push((obj as Node).Left);
                }
                else
                {
                    traversal[count++] = (long)obj;
                }
            }
            return string.Join(" ", traversal);
        }

        private static string TraversePreOrderIterative(Node node, int treeSize)
        {
            Stack<object> nodeStack = new Stack<object>();
            var traversal = new long[treeSize];
            var count = 0;
            nodeStack.Push(node);
            while (nodeStack.Count > 0)
            {
                var obj = nodeStack.Pop();
                if (obj is Node)
                {
                    if ((obj as Node).Right != null) nodeStack.Push((obj as Node).Right);
                    if ((obj as Node).Left != null) nodeStack.Push((obj as Node).Left);
                    nodeStack.Push((obj as Node).Key);
                }
                else
                {
                    traversal[count++] = (long)obj;
                }
            }
            return string.Join(" ", traversal);
        }

        private static string TraversePostOrderIterative(Node node, int treeSize)
        {
            Stack<object> nodeStack = new Stack<object>();
            var traversal = new long[treeSize];
            var count = 0;
            nodeStack.Push(node);
            while (nodeStack.Count > 0)
            {
                var obj = nodeStack.Pop();
                if (obj is Node)
                {
                    nodeStack.Push((obj as Node).Key);
                    if ((obj as Node).Right != null) nodeStack.Push((obj as Node).Right);
                    if ((obj as Node).Left != null) nodeStack.Push((obj as Node).Left);
                }
                else
                {
                    traversal[count++] = (long)obj;
                }
            }
            return string.Join(" ", traversal);
        }

        public static string[] Solve(string[] vertices)
        {
            var tree = BuildTree(vertices);
            return new string[3] {
                TraverseInOrderIterative(tree, vertices.Length),
                TraversePreOrderIterative(tree, vertices.Length),
                TraversePostOrderIterative(tree, vertices.Length)
            };
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
