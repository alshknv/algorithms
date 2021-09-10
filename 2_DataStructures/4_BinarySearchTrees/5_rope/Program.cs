using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace _5_rope
{
    public class Node
    {
        public Node Parent;
        public Node Left;
        public Node Right;
        public int Size;
        public char Letter;
        public Node(char letter)
        {
            Letter = letter;
        }
    }

    public static class SplayTree
    {
        public static Random Rnd = new Random();
        public static Node Splay(Node r, Node N)
        {
            if (N == null) return r;
            while (N != r)
            {
                if (N.Parent == r && r.Left == N)
                {
                    // left zig
                    r.Left = N.Right;
                    if (N.Right != null) N.Right.Parent = r;
                    N.Right = r;
                    r.Parent = N;
                    N.Parent = null;
                    r = N;
                    RecomputeSize(N.Right);
                    RecomputeSize(N);
                }
                else if (N.Parent == r && r.Right == N)
                {
                    // right zig
                    r.Right = N.Left;
                    if (N.Left != null) N.Left.Parent = r;
                    N.Left = r;
                    r.Parent = N;
                    N.Parent = null;
                    r = N;
                    RecomputeSize(N.Left);
                    RecomputeSize(N);
                }
                else
                {
                    var P = N.Parent;
                    var Q = P.Parent;
                    if (N.Parent.Left == N && N.Parent.Parent.Left == N.Parent)
                    {
                        //left-zig-zig
                        Q.Left = P.Right;
                        if (P.Right != null) P.Right.Parent = Q;
                        P.Right = Q;
                        P.Left = N.Right;
                        if (N.Right != null) N.Right.Parent = P;
                        N.Right = P;
                        N.Parent = Q.Parent;
                        P.Parent = N;
                        Q.Parent = P;
                    }
                    else if (N.Parent.Right == N && N.Parent.Parent.Right == N.Parent)
                    {
                        //right zig-zig
                        Q.Right = P.Left;
                        if (P.Left != null) P.Left.Parent = Q;
                        P.Left = Q;
                        P.Right = N.Left;
                        if (N.Left != null) N.Left.Parent = P;
                        N.Left = P;
                        N.Parent = Q.Parent;
                        P.Parent = N;
                        Q.Parent = P;
                    }
                    else if (N.Parent.Left == N && N.Parent.Parent.Right == N.Parent)
                    {
                        // left zig-zag
                        Q.Right = N.Left;
                        if (N.Left != null) N.Left.Parent = Q;
                        N.Left = Q;
                        P.Left = N.Right;
                        if (N.Right != null) N.Right.Parent = P;
                        N.Right = P;
                        N.Parent = Q.Parent;
                        P.Parent = N;
                        Q.Parent = N;
                    }
                    else if (N.Parent.Right == N && N.Parent.Parent.Left == N.Parent)
                    {
                        //right zig-zag
                        Q.Left = N.Right;
                        if (N.Right != null) N.Right.Parent = Q;
                        N.Right = Q;
                        P.Right = N.Left;
                        if (N.Left != null) N.Left.Parent = P;
                        N.Left = P;
                        N.Parent = Q.Parent;
                        P.Parent = N;
                        Q.Parent = N;
                    }
                    if (N.Parent != null)
                    {
                        if (N.Parent.Left == Q) N.Parent.Left = N; else N.Parent.Right = N;
                    }
                    else
                    {
                        r = N;
                    }
                    RecomputeSize(Q);
                    RecomputeSize(P);
                    RecomputeSize(N);
                }
            }
            return r;
        }

        private static void RecomputeSize(Node node)
        {
            if (node == null) return;
            node.Size = 1 + (node.Left?.Size ?? 0) + (node.Right?.Size ?? 0);
        }

        public static Node Add(Node r, int idx, char letter)
        {
            var newNode = new Node(letter);
            var node = FindNs(r, int.MaxValue);
            if (node == null)
            {
                r = newNode;
            }
            else
            {
                node.Right = newNode;
                newNode.Parent = node;
            }
            r = Splay(r, newNode);
            node = FindNs(r, SplayTree.Rnd.Next(1000) % (idx + 1));
            if (node != r) r = Splay(r, node);

            while (newNode != null)
            {
                RecomputeSize(newNode);
                newNode = newNode.Parent;
            }
            return r;
        }

        public static Node FindNs(Node r, int idx)
        {
            if (r == null) return null;
            while (r.Left != null || r.Right != null)
            {
                var ls = (r.Left?.Size ?? 0);
                if (idx == ls) return r;
                if (idx < ls)
                {
                    if (r.Left != null) r = r.Left;
                    else return r;
                }
                else
                {
                    if (r.Right != null)
                    {
                        r = r.Right;
                        idx -= (ls + 1);
                    }
                    else
                    {
                        return r;
                    }
                }
            }
            return r;
        }

        public static Node Find(Node r, int key)
        {
            var node = FindNs(r, key);
            Splay(r, node);
            return node;
        }

        public static Node[] SplitLeft(Node r, int x)
        {
            var N = Find(r, x);
            var result = new Node[2];
            result[0] = N.Left;
            result[1] = N;
            if (result[1] != null) result[1].Left = null;
            if (result[0] != null) result[0].Parent = null;
            RecomputeSize(result[0]);
            RecomputeSize(result[1]);
            return result;
        }

        public static Node[] SplitRight(Node r, int x)
        {
            var N = Find(r, x);
            var result = new Node[2];
            result[0] = N;
            result[1] = N.Right;
            if (result[0] != null) result[0].Right = null;
            if (result[1] != null) result[1].Parent = null;
            RecomputeSize(result[0]);
            RecomputeSize(result[1]);
            return result;
        }

        public static Node[] Cut(Node r, int i, int j)
        {
            var t1 = SplitLeft(r, i);
            var t2 = SplitRight(t1[1], j - i);
            return new Node[2] {
                t2[0], Merge(t1[0], t2[1])
            };
        }

        public static Node Insert(Node r, Node t, int k)
        {
            var t1 = SplitLeft(r, k);
            var merge1 = Merge(t1[0], t);
            return Merge(merge1, t1[1]);
        }

        public static Node Merge(Node t1, Node t2)
        {
            if (t1 == null) return t2;
            if (t2 == null) return t1;
            var maxT1 = Find(t1, int.MaxValue);
            maxT1.Right = t2;
            t2.Parent = maxT1;
            RecomputeSize(maxT1?.Right);
            RecomputeSize(maxT1);
            return maxT1;
        }

        public static string GetString(Node node, int size)
        {
            if (node == null) return "";
            Stack<object> nodeStack = new Stack<object>();
            var traversal = new char[size];
            var count = 0;
            nodeStack.Push(node);
            while (nodeStack.Count > 0)
            {
                var obj = nodeStack.Pop();
                if (obj is Node)
                {
                    if ((obj as Node).Right != null) nodeStack.Push((obj as Node).Right);
                    nodeStack.Push((obj as Node).Letter);
                    if ((obj as Node).Left != null) nodeStack.Push((obj as Node).Left);
                }
                else
                {
                    traversal[count++] = (char)obj;
                }
            }
            return new string(traversal);
        }
    }

    public static class Rope
    {
        public static string Solve(string input, string[] queries)
        {
            Node tree = null;
            for (int n = 0; n < input.Length; n++)
            {
                tree = SplayTree.Add(tree, n, input[n]);
            }
            foreach (var query in queries)
            {
                var queryData = query.Split(' ').Select(q => int.Parse(q)).ToArray();
                var cut = SplayTree.Cut(tree, queryData[0], queryData[1]);
                tree = SplayTree.Insert(cut[1], cut[0], queryData[2]);
            }
            return SplayTree.GetString(tree, input.Length);
        }

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var n = int.Parse(Console.ReadLine());
            var queries = new string[n];
            for (int i = 0; i < n; i++)
            {
                queries[i] = Console.ReadLine();
            }
            Console.WriteLine(Solve(input, queries));
        }
    }
}
