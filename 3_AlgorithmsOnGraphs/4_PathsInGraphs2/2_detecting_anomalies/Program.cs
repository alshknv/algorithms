using System;
using System.Collections.Generic;
using System.Linq;

namespace _2_detecting_anomalies
{
    public static class NegativeCycle
    {
        public class Edge
        {
            public int Source;
            public int Destination;
            public int Weight;
        }

        public class Vertex
        {
            public int Distance;
            public bool Relaxed;
        }

        public static bool BellmanFordCycle(Vertex[] vertices, Edge[] edges, int start)
        {
            for (int i = 1; i < vertices.Length; i++)
            {
                vertices[i].Distance = int.MaxValue;
            }
            vertices[start].Distance = 0;
            bool relaxed = false;
            for (int i = 0; i <= vertices.Length; i++)
            {
                for (int k = 0; k < edges.Length; k++)
                {
                    if (vertices[edges[k].Source].Distance == int.MaxValue) continue;
                    if (vertices[edges[k].Destination].Distance > vertices[edges[k].Source].Distance + edges[k].Weight)
                    {
                        vertices[edges[k].Destination].Relaxed = true;
                        vertices[edges[k].Destination].Distance = vertices[edges[k].Source].Distance + edges[k].Weight;
                        if (i == vertices.Length)
                        {
                            //relaxation at (V+1)th iteration
                            relaxed = true;
                            break;
                        }
                    }
                }
            }
            return relaxed;
        }

        public static string Solve(string[] input)
        {
            // graph init
            var graphInfo = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var vertices = new Vertex[graphInfo[0] + 1];
            var edges = new Edge[graphInfo[1]];
            for (int i = 0; i < graphInfo[1]; i++)
            {
                var edgeInfo = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                edges[i] = new Edge()
                {
                    Source = edgeInfo[0],
                    Destination = edgeInfo[1],
                    Weight = edgeInfo[2]
                };
            }

            for (int i = 1; i <= graphInfo[0]; i++)
            {
                vertices[i] = new Vertex();
            }

            // Check for negative cycle
            for (int i = 1; i <= graphInfo[0]; i++)
            {
                if (vertices[i].Relaxed) continue;
                if (BellmanFordCycle(vertices, edges, i)) return "1";
            }
            return "0";
        }

        static void Main(string[] args)
        {
            var nmLine = Console.ReadLine();
            var edgeCount = int.Parse(nmLine.Split(' ').Last());
            var data = new string[edgeCount + 1];
            data[0] = nmLine;
            for (int i = 0; i < edgeCount; i++)
            {
                data[i + 1] = Console.ReadLine();
            }
            Console.WriteLine(Solve(data));
        }
    }
}
