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
        public int InLb;
        public int OutLb;
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
            queue.Enqueue(0);
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
                    while (i > 0)
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
            var network = new Node[n + 2]; // indexes from 1 + 2 nodes s & t
            for (int i = 0; i < n + 2; i++)
            {
                network[i] = new Node();
            }
            //sum of all lowerbounds
            var lowerSum = 0;

            // process input
            for (int j = 1; j < input.Length; j++)
            {
                var edge = input[j].Split(' ').Select(x => int.Parse(x)).ToArray();
                lowerSum += edge[2];
                var edgeF = new Edge() { Destination = edge[1], Source = edge[0], Flow = edge[3] - edge[2], Lowerbound = edge[2], Index = j };
                var edgeR = new Edge() { Destination = edge[0], Source = edge[1], Flow = 0, Lowerbound = edge[2], Index = j };
                network[edge[0]].Edges.Add(new TwoWayEdge() { EdgeF = edgeF, EdgeR = edgeR });
                network[edge[0]].OutLb += edge[2];
                network[edge[1]].InLb += edge[2];
            }

            // add s & t 
            for (int k = 1; k < network.Length - 1; k++)
            {
                network[0].Edges.Add(new TwoWayEdge()
                {
                    EdgeF = new Edge() { Destination = k, Source = 0, Flow = network[k].InLb, Lowerbound = 0, Index = 0 },
                    EdgeR = new Edge() { Destination = 0, Source = k, Flow = 0, Lowerbound = 0, Index = 0 }
                });
                network[k].Edges.Add(new TwoWayEdge()
                {
                    EdgeF = new Edge() { Destination = network.Length - 1, Source = k, Flow = network[k].OutLb, Lowerbound = 0, Index = 0 },
                    EdgeR = new Edge() { Destination = k, Source = network.Length - 1, Flow = 0, Lowerbound = 0, Index = 0 }
                });
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

            // if maxflow is not equal to sum of lowerbounds, there's no solution
            if (flow != lowerSum) return new string[] { "NO" };

            // output result using edges' indices
            var result = new string[input.Length];
            result[0] = "YES";
            for (int i = 1; i < network.Length - 1; i++)
            {
                for (int j = 0; j < network[i].Edges.Count; j++)
                {
                    if (network[i].Edges[j].EdgeR.Index == 0) continue;
                    result[network[i].Edges[j].EdgeR.Index] = (network[i].Edges[j].EdgeR.Flow + network[i].Edges[j].EdgeR.Lowerbound).ToString();
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
