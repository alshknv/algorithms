using System;
using System.Collections.Generic;
using System.Linq;

namespace _3_exchanging_money
{
    public class Edge
    {
        public int Source;
        public int Destination;
        public long Weight;
    }

    public class Vertex
    {
        public LinkedList<int> Destinations;
        public bool Visited;
        public long Distance = long.MaxValue;
        public Vertex()
        {
            Destinations = new LinkedList<int>();
        }
    }

    public static class MoneyExchange
    {
        private static void ExploreNegCycle(Vertex[] vertices, int index)
        {
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var v = vertexStack.Peek();
                vertices[v].Visited = true;
                vertices[v].Distance = long.MinValue;
                while (vertices[v].Destinations.First != null && vertices[vertices[v].Destinations.First.Value].Visited)
                {
                    vertices[v].Destinations.RemoveFirst();
                }
                if (vertices[v].Destinations.First != null)
                {
                    vertexStack.Push(vertices[v].Destinations.First.Value);
                }
                else
                {
                    vertexStack.Pop();
                }
            }
        }

        private static void BellmanFord(Vertex[] vertices, Edge[] edges, int start)
        {
            vertices[start].Distance = 0;
            var cyclic = new List<int>();
            for (int i = 0; i <= vertices.Length; i++)
            {
                for (int k = 0; k < edges.Length; k++)
                {
                    if (vertices[edges[k].Source].Distance == long.MaxValue) continue;
                    if (vertices[edges[k].Destination].Distance > vertices[edges[k].Source].Distance + edges[k].Weight)
                    {
                        vertices[edges[k].Destination].Distance = vertices[edges[k].Source].Distance + edges[k].Weight;
                        if (i == vertices.Length)
                        {
                            //relaxation at (V+1)th iteration
                            cyclic.Add(edges[k].Destination);
                        }
                    }
                }
            }

            for (int i = 0; i < cyclic.Count; i++)
            {
                // explore from cycle
                ExploreNegCycle(vertices, cyclic[i]);
            }
        }

        public static string[] Solve(string[] input)
        {
            // graph init
            var graphInfo = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var vertices = new Vertex[graphInfo[0] + 1];
            for (int i = 1; i <= graphInfo[0]; i++)
            {
                vertices[i] = new Vertex();
            }
            var edges = new Edge[graphInfo[1]];
            for (int i = 0; i < graphInfo[1]; i++)
            {
                var edgeInfo = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                vertices[edgeInfo[0]].Destinations.AddLast(edgeInfo[1]);
                edges[i] = new Edge()
                {
                    Source = edgeInfo[0],
                    Destination = edgeInfo[1],
                    Weight = edgeInfo[2]
                };
            }
            var start = int.Parse(input[input.Length - 1]);

            // Shortest paths
            BellmanFord(vertices, edges, start);
            return vertices.Skip(1).Select(x =>
            {
                if (x.Distance == long.MaxValue) return "*";
                if (x.Distance == long.MinValue) return "-";
                return x.Distance.ToString();
            }).ToArray();
        }

        static void Main(string[] args)
        {
            var nmLine = Console.ReadLine();
            var edgeCount = int.Parse(nmLine.Split(' ').Last());
            var data = new string[edgeCount + 2];
            data[0] = nmLine;
            for (int i = 0; i < edgeCount; i++)
            {
                data[i + 1] = Console.ReadLine();
            }
            data[edgeCount + 1] = Console.ReadLine();
            var result = Solve(data);
            for (int i = 0; i < result.Length; i++)
            {
                Console.WriteLine(result[i]);
            }
        }
    }
}
