using System.Collections.Generic;
using System.Linq;
using System;

namespace _1_evacuation
{
    public class Edge
    {
        public int Capacity;
        public int Flow;
        public int Source;
        public int Destination;
    }

    public class ResidualEdge
    {
        public Edge EdgeF;
        public Edge EdgeR;
    }

    public class Node
    {
        public List<Edge> Edges = new List<Edge>();
    }

    public class ResidualNode
    {
        public bool Visited;
        public ResidualEdge PathBack;
        public List<ResidualEdge> Edges = new List<ResidualEdge>();
    }

    /*public class PathSegment
    {
        public int Source;
        public int Destination;
        public int Edge1;
        public int Edge2;
        public int Capacity;
    }*/

    public class Path
    {
        public ResidualEdge[] Edges;
        public int MinCapacity;
    }

    public static class Evacuation
    {
        private static Path FindPath(ResidualNode[] residual)
        {
            var queue = new Queue<int>();
            queue.Enqueue(1);
            Path result = null;
            var touched = new List<int>();
            while (queue.Count > 0)
            {
                var i = queue.Dequeue();
                if (i == residual.Length - 1)
                {
                    var pathEdges = new Stack<ResidualEdge>();
                    var minCapacity = int.MaxValue;
                    while (i > 1)
                    {
                        pathEdges.Push(residual[i].PathBack);
                        if (residual[i].PathBack.EdgeF.Flow < minCapacity) minCapacity = residual[i].PathBack.EdgeF.Flow;
                        i = residual[i].PathBack.EdgeF.Source;
                    }
                    result = new Path()
                    {
                        Edges = pathEdges.ToArray(),
                        MinCapacity = minCapacity
                    };
                    break;
                }
                for (int j = 0; j < residual[i].Edges.Count; j++)
                {
                    if (residual[i].Edges[j].EdgeF.Flow <= 0) continue;
                    var next = residual[i].Edges[j].EdgeF.Destination;
                    if (!residual[next].Visited)
                    {
                        touched.Add(next);
                        residual[next].Visited = true;
                        residual[next].PathBack = residual[i].Edges[j];
                        queue.Enqueue(next);
                    }
                }
            }
            foreach (var t in touched)
            {
                residual[t].Visited = false;
                residual[t].PathBack = null;
            }
            return result;
        }

        public static string Solve(string[] input)
        {
            var nmCount = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var network = new Node[nmCount[0] + 1];
            var residual = new ResidualNode[nmCount[0] + 1];
            for (int i = 1; i <= nmCount[0]; i++)
            {
                network[i] = new Node();
                residual[i] = new ResidualNode();
            }

            for (int j = 1; j <= nmCount[1]; j++)
            {
                var edge = input[j].Split(' ').Select(x => int.Parse(x)).ToArray();
                network[edge[0]].Edges.Add(new Edge() { Destination = edge[1], Capacity = edge[2] });
                var edgeF = new Edge() { Destination = edge[1], Source = edge[0], Flow = edge[2] };
                var edgeR = new Edge() { Destination = edge[0], Source = edge[1], Flow = 0 };
                residual[edge[0]].Edges.Add(new ResidualEdge() { EdgeF = edgeF, EdgeR = edgeR });
                residual[edge[1]].Edges.Add(new ResidualEdge() { EdgeF = edgeR, EdgeR = edgeF });
            }

            var flow = 0;
            while (true)
            {
                var path = FindPath(residual);
                if (path == null) return flow.ToString();
                for (int i = 0; i < path.Edges.Length; i++)
                {
                    var e = path.Edges[i];
                    e.EdgeF.Flow -= path.MinCapacity;
                    e.EdgeR.Flow += path.MinCapacity;
                }
                flow += path.MinCapacity;
            }
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var nmCount = nmline.Split(' ').Select(x => int.Parse(x)).ToArray();
            var input = new string[nmCount[1] + 1];
            input[0] = nmline;
            for (int i = 1; i <= nmCount[1]; i++)
                input[i] = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
