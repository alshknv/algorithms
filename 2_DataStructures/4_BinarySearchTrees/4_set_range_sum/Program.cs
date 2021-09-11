using System.IO;
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
        public long Sum;
        public Node(long key)
        {
            Key = key;
        }
    }

    public class SplayTree
    {
        private void RecomputeSum(Node node)
        {
            if (node == null) return;
            node.Sum = node.Key + (node.Left?.Sum ?? 0) + (node.Right?.Sum ?? 0);
        }

        private Node Splay(Node N, Node r)
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
                    RecomputeSum(N.Right);
                    RecomputeSum(N);
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
                    RecomputeSum(N.Left);
                    RecomputeSum(N);
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
                    RecomputeSum(Q);
                    RecomputeSum(P);
                    RecomputeSum(N);
                }
            }
            return r;
        }

        private Node AddNs(long key, Node r)
        {
            var newNode = new Node(key);
            var node = FindNs(key, r);
            if (node?.Key == key) return null;
            if (node == null)
            {
                return newNode;
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
            var cNode = newNode;
            while (cNode != null)
            {
                RecomputeSum(cNode);
                cNode = cNode.Parent;
            }
            return newNode;
        }

        private Node FindNs(long key, Node r)
        {
            var node = r;
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

        private Node Next(long key, Node r)
        {
            var node = FindNs(key, r);
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

        public Node Add(long key, Node r)
        {
            var node = AddNs(key, r);
            if (r == null) r = node;
            return Splay(node, r);
        }

        public Node Find(long key, Node r)
        {
            var node = FindNs(key, r);
            Splay(node, r);
            return node;
        }

        public Node Remove(long key, Node r)
        {
            var node = Find(key, r);
            r = node;
            if (node?.Key == key)
            {
                if (node == r && r.Right == null)
                {
                    var newRoot = r.Left;
                    r.Left = null;
                    if (newRoot != null) newRoot.Parent = null;
                    r = newRoot;
                }
                else
                {
                    var next = Next(node.Key, r);
                    if (next == null)
                    {
                        node.Parent.Right = null;
                        node.Parent = null;
                    }
                    else
                    {
                        r = Splay(next, r);
                        Splay(node, r);
                        next.Left = node.Left;
                        if (next.Left != null) next.Left.Parent = next;
                        r = next;
                        next.Parent = null;
                    }
                }
            }
            return r;
        }

        private Node[] SplitL(Node N)
        {
            var result = new Node[2];
            result[0] = N.Left;
            result[1] = N;
            if (result[1] != null) result[1].Left = null;
            if (result[0] != null) result[0].Parent = null;
            RecomputeSum(result[0]);
            RecomputeSum(result[1]);
            return result;
        }

        private Node[] SplitR(Node N)
        {
            var result = new Node[2];
            result[0] = N;
            result[1] = N.Right;
            if (result[0] != null) result[0].Right = null;
            if (result[1] != null) result[1].Parent = null;
            RecomputeSum(result[0]);
            RecomputeSum(result[1]);
            return result;
        }

        private Node[] SplitLeft(Node r, long x)
        {
            if (r == null) return new Node[] { null, null };
            var N = Find(x, r);
            return N.Key < x ? SplitR(N) : SplitL(N);
        }

        private Node[] SplitRight(Node r, long x)
        {
            if (r == null) return new Node[] { null, null };
            var N = Find(x, r);
            return N.Key > x ? SplitL(N) : SplitR(N);
        }

        public Node Merge(Node t1, Node t2)
        {
            if (t1 == null) return t2;
            if (t2 == null) return t1;
            var maxT1 = Find(long.MaxValue, t1);
            maxT1.Right = t2;
            t2.Parent = maxT1;
            RecomputeSum(maxT1?.Right);
            RecomputeSum(maxT1);
            return maxT1;
        }

        public Node Sum(long l, long h, Node r, out long sum)
        {
            sum = 0;
            if (r != null)
            {
                var t1 = SplitLeft(r, l);
                var t2 = SplitRight(t1[1], h);
                sum = t2[0]?.Sum ?? 0;
                r = Merge(Merge(t1[0], t2[0]), t2[1]);
            }
            return r;
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
            Node root = null;
            for (int i = 0; i < commands.Length; i++)
            {
                var command = commands[i].Split(' ');
                long arg = (long.Parse(command[1]) + lastSum) % M;
                switch (command[0])
                {
                    case "+":
                        root = tree.Add(arg, root);
                        break;
                    case "-":
                        root = tree.Remove(arg, root);
                        break;
                    case "?":
                        var node = tree.Find(arg, root);
                        root = node;
                        result[count++] = (node?.Key == arg) ? "Found" : "Not found";
                        break;
                    case "s":
                        long arg2 = (long.Parse(command[2]) + lastSum) % M;
                        root = tree.Sum(arg, arg2, root, out lastSum);
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
