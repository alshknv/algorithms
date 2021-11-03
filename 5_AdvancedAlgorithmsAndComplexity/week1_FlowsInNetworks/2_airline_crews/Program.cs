using System.Collections.Generic;
using System.Linq;
using System;

namespace _2_airline_crews
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

    public static class AirlineCrew
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

        public static string Solve(string[] input)
        {
            // initialization
            var nmCount = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var flightCount = nmCount[0];
            var crewCount = nmCount[1];

            // network: source, 1-based flights and crews, sink
            var network = new Node[crewCount + flightCount + 2];

            //source
            network[0] = new Node()
            {
                Edges = new List<TwoWayEdge>(flightCount)
            };
            //sink
            network[crewCount + flightCount + 1] = new Node()
            {
                Edges = new List<TwoWayEdge>(crewCount)
            };

            for (int i = 1; i <= flightCount; i++)
            {
                // source to flight
                var edgeF = new Edge() { Destination = i, Source = 0, Flow = 1 };
                var edgeR = new Edge() { Destination = 0, Source = i, Flow = 0 };
                network[i] = new Node();
                network[0].Edges.Add(new TwoWayEdge() { EdgeF = edgeF, EdgeR = edgeR });
                network[i].Edges.Add(new TwoWayEdge() { EdgeF = edgeR, EdgeR = edgeF });
            }
            for (int i = 1; i <= crewCount; i++)
            {
                // crew to sink
                var edgeF = new Edge() { Destination = flightCount + crewCount + 1, Source = flightCount + i, Flow = 1 };
                var edgeR = new Edge() { Destination = flightCount + i, Source = flightCount + crewCount + 1, Flow = 0 };
                network[flightCount + i] = new Node();
                network[flightCount + i].Edges.Add(new TwoWayEdge() { EdgeF = edgeF, EdgeR = edgeR });
                network[flightCount + crewCount + 1].Edges.Add(new TwoWayEdge() { EdgeF = edgeR, EdgeR = edgeF });
            }

            //
            for (int i = 1; i < input.Length; i++)
            {
                //flights to crews
                var connections = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                for (int j = 1; j <= connections.Length; j++)
                {
                    if (connections[j - 1] == 0) continue;
                    var edgeF = new Edge() { Destination = flightCount + j, Source = i, Flow = 1 };
                    var edgeR = new Edge() { Destination = i, Source = flightCount + j, Flow = 0 };
                    network[i].Edges.Add(new TwoWayEdge() { EdgeF = edgeF, EdgeR = edgeR });
                    network[flightCount + j].Edges.Add(new TwoWayEdge() { EdgeF = edgeR, EdgeR = edgeF });
                }
            }

            // Edmonds-Karp algorithm
            while (true)
            {
                var path = FindPath(network);
                if (path == null) break;
                for (int i = 0; i < path.Edges.Length; i++)
                {
                    path.Edges[i].EdgeF.Flow -= path.MinCapacity;
                    path.Edges[i].EdgeR.Flow += path.MinCapacity;
                }
            }

            //reconstruct pairings
            var result = new int[flightCount];
            for (int i = 1; i <= flightCount; i++)
            {
                result[i - 1] = -1;
                for (int j = 1; j < network[i].Edges.Count; j++)
                {
                    if (network[i].Edges[j].EdgeF.Flow == 0)
                    {
                        result[i - 1] = network[i].Edges[j].EdgeF.Destination - flightCount;
                    }
                }
            }
            return string.Join(" ", result);
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var nmCount = nmline.Split(' ').Select(x => int.Parse(x)).ToArray();
            var input = new string[nmCount[0] + 1];
            input[0] = nmline;
            for (int i = 1; i <= nmCount[0]; i++)
                input[i] = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
