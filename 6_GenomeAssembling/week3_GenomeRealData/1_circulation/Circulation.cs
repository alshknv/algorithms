using System.Collections.Generic;
using System;
using System.Linq;

namespace _1_circulation
{
    public class Edge
    {
        public int Flow;
        public int Lowerbound;
        public int Index;
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
        public int Balance;
        public TwoWayEdge PathBack;
        public List<TwoWayEdge> Edges = new List<TwoWayEdge>();
    }

    public class Path
    {
        public TwoWayEdge[] Edges;
        public int MinCapacity;
    }

    public static class Circulation
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

        public static string[] Solve(string[] input)
        {
            var n = int.Parse(input[0].Split(' ')[0]);
            // initialization
            var network = new Node[n + 2];
            for (int i = 1; i <= n + 1; i++)
            {
                network[i] = new Node();
            }
            // we split 1st node to source (s) and sink (t), so we need extra node in the end of network array

            for (int j = 1; j < input.Length; j++)
            {
                var edge = input[j].Split(' ').Select(x => int.Parse(x)).ToArray();
                if (edge[1] == 1) edge[1] = n + 1; // if edge is going to s, redirect it to t
                var edgeF = new Edge() { Destination = edge[1], Source = edge[0], Flow = edge[3], Lowerbound = edge[2], Index = j };
                var edgeR = new Edge() { Destination = edge[0], Source = edge[1], Flow = 0, Lowerbound = edge[2], Index = j };
                network[edge[0]].Edges.Add(new TwoWayEdge() { EdgeF = edgeF, EdgeR = edgeR });
            }

            // Edmonds-Karp algorithm
            var flow = 0;
            while (true)
            {
                var path = FindPath(network);
                if (path == null) break;
                for (int i = 0; i < path.Edges.Length; i++)
                {
                    path.Edges[i].EdgeF.Flow -= path.MinCapacity;
                    network[path.Edges[i].EdgeF.Source].Balance -= path.MinCapacity;
                    path.Edges[i].EdgeR.Flow += path.MinCapacity;
                    network[path.Edges[i].EdgeF.Destination].Balance += path.MinCapacity;
                }
                flow += path.MinCapacity;
            }

            // check if incoming flow equals outgoing flow for splitted first node
            if (network[1].Balance + network[network.Length - 1].Balance != 0)
                return new string[] { "NO" };

            // check that resulting flow satisfies given lowerbounds and every node conserves flow
            var result = new string[input.Length];
            result[0] = "YES";
            for (int i = 1; i < network.Length - 1; i++)
            {
                if (i > 1 && network[i].Balance != 0) return new string[] { "NO" };
                for (int j = 0; j < network[i].Edges.Count; j++)
                {
                    if (network[i].Edges[j].EdgeR.Flow < network[i].Edges[j].EdgeR.Lowerbound)
                        return new string[] { "NO" };
                    result[network[i].Edges[j].EdgeR.Index] = network[i].Edges[j].EdgeR.Flow.ToString();
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var m = int.Parse(nmline.Split(' ')[1]);
            var input = new string[m + 1];
            input[0] = nmline;
            for (int i = 1; i <= m; i++)
            {
                input[i] = Console.ReadLine();
            }
            foreach (var line in Solve(input))
            {
                Console.WriteLine(line);
            }
        }
    }
}
