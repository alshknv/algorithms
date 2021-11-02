using System.Collections.Generic;
using System.Linq;
using System;

namespace _1_evacuation
{
    public class Edge
    {
        public int Flow;
        public int Source;
        public int Destination;
    }

    public class TwoWayEdge
    {
        public Edge EdgeF;
        public Edge EdgeR;
    }

    public class Node
    {
        public bool Visited;
        public TwoWayEdge PathBack;
        public List<TwoWayEdge> Edges = new List<TwoWayEdge>();
    }

    public class Path
    {
        public TwoWayEdge[] Edges;
        public int MinCapacity;
    }

    public static class Evacuation
    {
        private static Path FindPath(Node[] residual)
        {
            //breadth-first search
            var queue = new Queue<int>();
            queue.Enqueue(1);
            Path result = null;
            var touched = new List<int>();
            while (queue.Count > 0)
            {
                var i = queue.Dequeue();
                if (i == residual.Length - 1)
                {
                    // reconstruct path if sink was found
                    var pathEdges = new Stack<TwoWayEdge>();
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
                    // add children with positive flows that weren't visited yet to queue
                    if (residual[i].Edges[j].EdgeF.Flow <= 0) continue;
                    var next = residual[i].Edges[j].EdgeF.Destination;
                    if (residual[next].Visited) continue;
                    touched.Add(next);
                    residual[next].Visited = true;
                    residual[next].PathBack = residual[i].Edges[j];
                    queue.Enqueue(next);
                }
            }

            // restore after search
            foreach (var t in touched)
            {
                residual[t].Visited = false;
                residual[t].PathBack = null;
            }
            return result;
        }

        public static string Solve(string[] input)
        {
            // initialization
            var nmCount = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var network = new Node[nmCount[0] + 1];
            for (int i = 1; i <= nmCount[0]; i++)
            {
                network[i] = new Node();
            }

            for (int j = 1; j <= nmCount[1]; j++)
            {
                var edge = input[j].Split(' ').Select(x => int.Parse(x)).ToArray();
                var edgeF = new Edge() { Destination = edge[1], Source = edge[0], Flow = edge[2] };
                var edgeR = new Edge() { Destination = edge[0], Source = edge[1], Flow = 0 };
                network[edge[0]].Edges.Add(new TwoWayEdge() { EdgeF = edgeF, EdgeR = edgeR });
                network[edge[1]].Edges.Add(new TwoWayEdge() { EdgeF = edgeR, EdgeR = edgeF });
            }

            // Edmonds-Karp algorithm
            var flow = 0;
            while (true)
            {
                var path = FindPath(network);
                if (path == null) return flow.ToString();
                for (int i = 0; i < path.Edges.Length; i++)
                {
                    path.Edges[i].EdgeF.Flow -= path.MinCapacity;
                    path.Edges[i].EdgeR.Flow += path.MinCapacity;
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
