using System;

namespace _4_set_range_sum
{
    public class Node
    {
        public Node Parent;
        public Node Left;
        public Node Right;
        public long Key;
        public Node(long key)
        {
            Key = key;
        }
    }

    public class SplayTree
    {
        private Node root;

        private void Splay(Node N)
        {
            if (N == null) return;
            while (N != root)
            {
                if (N.Parent == root && root.Left == N)
                {
                    // left zig
                    root.Left = N.Right;
                    N.Right = root;
                    root.Parent = N;
                    N.Parent = null;
                    root = N;
                }
                if (N.Parent == root && root.Right == N)
                {
                    // right zig
                    root.Right = N.Left;
                    N.Left = root;
                    root.Parent = N;
                    N.Parent = null;
                    root = N;
                }
                else
                {
                    var P = N.Parent;
                    var Q = P.Parent;
                    if (N.Parent.Left == N && N.Parent.Parent.Left == N.Parent)
                    {
                        //left-zig-zig
                        Q.Left = P.Right;
                        P.Right = Q;
                        P.Left = N.Right;
                        N.Right = P;
                        N.Parent = Q.Parent;
                        P.Parent = N;
                        Q.Parent = P;
                    }
                    else if (N.Parent.Right == N && N.Parent.Parent.Right == N.Parent)
                    {
                        //right zig-zig
                        Q.Right = P.Left;
                        P.Left = Q;
                        P.Right = N.Left;
                        N.Left = P;
                        N.Parent = Q.Parent;
                        P.Parent = N;
                        Q.Parent = P;
                    }
                    else if (N.Parent.Left == N && N.Parent.Parent.Right == N.Parent)
                    {
                        // left zig-zag
                        Q.Left = N.Right;
                        N.Right = Q;
                        P.Right = N.Left;
                        N.Left = P;
                        N.Parent = Q.Parent;
                        P.Parent = N;
                        Q.Parent = N;
                    }
                    else if (N.Parent.Right == N && N.Parent.Parent.Left == N.Parent)
                    {
                        //right zig-zag
                        Q.Right = N.Left;
                        N.Left = Q;
                        P.Left = N.Right;
                        N.Right = P;
                        N.Parent = Q.Parent;
                        P.Parent = N;
                        Q.Parent = N;
                    }
                    if (N.Parent != null)
                    {
                        if (N.Parent.Left == Q) N.Parent.Left = N; else N.Parent.Right = N;
                    }
                }
            }
        }

        private Node AddNs(long key)
        {
            var newNode = new Node(key);
            var node = FindNs(key);
            if (node?.Key == key) return null;
            if (node == null)
            {
                root = newNode;
            }
            else if (node.Key > key)
            {
                node.Left = newNode;
            }
            else if (node.Key < key)
            {
                node.Right = newNode;
            }
            return newNode;
        }

        private Node FindNs(long key)
        {
            var node = root;
            while (node != null && node.Key != key)
            {
                if (node.Key > key && node.Left != null)
                {
                    node = node.Left;
                }
                else if (node.Key < key && node.Right != null)
                {
                    node = node.Right;
                }
                else
                {
                    break;
                }
            }
            return node;
        }

        private Node Next(long key)
        {
            var node = FindNs(key);
            if (node.Right != null)
            {
                node = node.Right;
                while (node.Left != null) node = node.Left;
            }
            else
            {
                while (node != null && node.Key < key) node = node.Parent;
            }
            return node;
        }

        public void Add(long key)
        {
            var node = AddNs(key);
            Splay(node);
        }

        public Node Find(long key)
        {
            var node = FindNs(key);
            Splay(node);
            return node;
        }

        public void Remove(long key)
        {
            var node = Find(key);
            if (node.Key == key)
            {
                var next = Next(node.Key);
                if (next == null)
                {
                    node.Parent.Right = null;
                    node.Parent = null;
                }
                else
                {
                    Splay(next);
                    Splay(node);
                    next.Left = node.Left;
                    next.Left.Parent = next;
                    root = next;
                    next.Parent = null;
                }
            }
        }

        public long Sum(long l, long r)
        {
            return 0;
        }
    }

    public static class SetRangeSum
    {
        public static string[] Solve(string[] commands)
        {
            return new string[0];
        }

        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            var commands = new string[n];
            for (int i = 0; i < n; i++)
            {
                commands[i] = Console.ReadLine();
            }
            foreach (var line in Solve(commands))
            {
                Console.WriteLine(line);
            }
        }
    }
}
