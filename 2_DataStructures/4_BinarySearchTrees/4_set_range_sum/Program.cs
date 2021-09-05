using System;
using System.Linq;

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
                    if (N.Right != null) N.Right.Parent = root;
                    N.Right = root;
                    root.Parent = N;
                    N.Parent = null;
                    root = N;
                }
                else if (N.Parent == root && root.Right == N)
                {
                    // right zig
                    root.Right = N.Left;
                    if (N.Left != null) N.Left.Parent = root;
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
                        root = N;
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
                newNode.Parent = node;
            }
            else if (node.Key < key)
            {
                node.Right = newNode;
                newNode.Parent = node;
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
            return node.Key > key ? node : null;
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
            if (node?.Key == key)
            {
                if (node == root && root.Right == null)
                {
                    var newRoot = root.Left;
                    root.Left = null;
                    if (newRoot != null) newRoot.Parent = null;
                    root = newRoot;
                }
                else
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
                        if (next.Left != null) next.Left.Parent = next;
                        root = next;
                        next.Parent = null;
                    }
                }
            }
        }

        public long Sum(long l, long r)
        {
            long result = 0;
            while (l <= r)
            {
                var node = Find(l);
                if (node == null) break;
                if (node.Key >= l && node.Key <= r) result += node.Key;
                var next = Next(node.Key);
                if (next == null) break;
                l = next.Key;
            }
            return result;
        }
    }

    public static class SetRangeSum
    {
        private const long M = 1000000001;

        public static string[] Solve(string[] commands)
        {
            var tree = new SplayTree();
            long lastSum = 0;
            var count = 0;
            var result = new string[commands.Length];
            for (int i = 0; i < commands.Length; i++)
            {
                var command = commands[i].Split(' ');
                long arg = (long.Parse(command[1]) + lastSum) % M;
                switch (command[0])
                {
                    case "+":
                        tree.Add(arg);
                        break;
                    case "-":
                        tree.Remove(arg);
                        break;
                    case "?":
                        var node = tree.Find(arg);
                        result[count++] = (node?.Key == arg) ? "Found" : "Not found";
                        break;
                    case "s":
                        long arg2 = (long.Parse(command[2]) + lastSum) % M;
                        lastSum = tree.Sum(arg, arg2);
                        result[count++] = lastSum.ToString();
                        break;
                }
            }
            return result.Take(count).ToArray();
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
