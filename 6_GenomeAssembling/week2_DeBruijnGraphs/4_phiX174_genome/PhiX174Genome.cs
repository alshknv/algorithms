using System.Collections.Generic;
using System;
using System.Linq;

namespace _4_phiX174_genome
{
    public class Vertex
    {
        public string Mer;
        public int Index;
        public List<Edge> Edges = new List<Edge>();
        public int VisitedCount;
        public bool AllVisited
        {
            get
            {
                return VisitedCount == Edges.Count;
            }
        }
        public Vertex(string mer, int index)
        {
            Mer = mer;
            Index = index;
        }

        public void AddEdge(int destination)
        {
            Edges.Add(new Edge(destination));
        }
    }

    public class Edge
    {
        public int Destination;
        public bool Visited;
        public Edge(int destination)
        {
            Destination = destination;
        }
    }

    public class Graph
    {
        private readonly List<string> mers = new List<string>();
        private readonly Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();
        public Vertex this[int index]
        {
            get
            {
                return vertices[mers[index]];
            }
            set
            {
                vertices[mers[index]] = value;
            }
        }

        public int Vertex(string mer)
        {
            Vertex vertex;
            if (!mers.Contains(mer))
            {
                vertex = new Vertex(mer, mers.Count);
                mers.Add(mer);
                vertices.Add(mer, vertex);
            }
            else
            {
                vertex = vertices[mer];
            }
            return vertex.Index;
        }
    }

    public static class PhiX174Genome
    {
        private static Graph graph;

        private static List<int> FindCycle(int start)
        {
            var v = start;
            var cycle = new List<int>() { start };
            do
            {
                // continue to go through unvisited edges until we come back to start
                var edgeFound = false;
                for (int e = 0; e < graph[v].Edges.Count; e++)
                {
                    if (!graph[v].Edges[e].Visited)
                    {
                        graph[v].Edges[e].Visited = true;
                        graph[v].VisitedCount++;
                        v = graph[v].Edges[e].Destination;
                        if (v != start)
                            cycle.Add(v);
                        edgeFound = true;
                        break;
                    }
                }
                if (!edgeFound) return null;
            } while (v != start);
            return cycle;
        }

        private static List<int> GetEulerianCycle(int cycleLength)
        {
            // find first cycle
            var cycle = FindCycle(0);

            // continue while cycle length is less than total number of edges
            while (cycle.Count < cycleLength)
            {
                for (int i = 0; i < cycle.Count; i++)
                {
                    if (graph[cycle[i]].AllVisited) continue;
                    for (int j = 0; j < graph[cycle[i]].Edges.Count; j++)
                    {
                        if (!graph[cycle[i]].Edges[j].Visited)
                        {
                            // unvisited edge found
                            var newCycle = cycle.Take(i).ToList();
                            // insert new cycle in the middle of the old one and check for unvisited edges again
                            var nextCycle = FindCycle(cycle[i]);
                            newCycle.AddRange(nextCycle);
                            newCycle.AddRange(cycle.Skip(i).Take(cycle.Count - i));
                            cycle = newCycle;
                            break;
                        }
                    }
                }
            }
            return cycle;
        }

        public static string Assemble(string[] kmers)
        {
            graph = new Graph();

            var edgeCount = 0;
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddEdge(to);
                edgeCount++;
            }

            // find Eulerian cycle
            var cycle = GetEulerianCycle(edgeCount);

            // compose genome
            var genomeArray = new char[cycle.Count];
            for (int c = 0; c < cycle.Count; c++)
            {
                var vertexMer = graph[cycle[c]].Mer;
                genomeArray[c] = vertexMer[vertexMer.Length - 1]; //take last nucleotide from only, others are overlapped
            }
            return string.Concat(genomeArray);
        }

        static void Main(string[] args)
        {
            var kmers = new string[5396];
            for (int i = 0; i < 5396; i++)
                kmers[i] = Console.ReadLine();
            Console.WriteLine(Assemble(kmers));
        }
    }
}
