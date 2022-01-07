using System.Linq;
using System.Collections.Generic;
using System;


namespace _4_tip_removal
{
    public class Vertex
    {
        public int Index;
        public bool Deleted;
        public List<Edge> OutgoingEdges = new List<Edge>();
        public List<Edge> IncomingEdges = new List<Edge>();

        public Vertex(int index)
        {
            Index = index;
        }

        public void AddOutgoingEdge(int destination)
        {
            OutgoingEdges.Add(new Edge(destination));
        }

        public void AddIncomingEdge(int destination)
        {
            IncomingEdges.Add(new Edge(destination));
        }
    }

    public class Edge
    {
        public int Connection;
        public bool Visited;
        public Edge(int connection)
        {
            Connection = connection;
        }
    }

    public class Graph
    {
        private readonly List<string> mers = new List<string>();
        public readonly Dictionary<string, Vertex> Vertices = new Dictionary<string, Vertex>();
        public int VertexCount
        {
            get { return Vertices.Keys.Count; }
        }

        public Vertex this[int index]
        {
            get
            {
                return Vertices[mers[index]];
            }
            set
            {
                Vertices[mers[index]] = value;
            }
        }

        public int Vertex(string mer)
        {
            Vertex vertex;
            if (!mers.Contains(mer))
            {
                vertex = new Vertex(mers.Count);
                mers.Add(mer);
                Vertices.Add(mer, vertex);
            }
            else
            {
                vertex = Vertices[mer];
            }
            return vertex.Index;
        }
    }

    public static class TipRemoval
    {
        private static Graph graph;
        private static HashSet<string> GetKMers(string[] reads, int k)
        {
            var kmers = new HashSet<string>();
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = 0; j <= reads[i].Length - k; j++)
                {
                    kmers.Add(reads[i].Substring(j, k));
                }
            }
            return kmers;
        }

        private static void ConstructGraph(string[] kmers)
        {
            graph = new Graph();

            // construct De Bruijn graph
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddOutgoingEdge(to);
                graph[to].AddIncomingEdge(from);
            }
        }

        private static int RemoveTips()
        {
            var tipCount = 0;
            for (int i = 0; i < graph.VertexCount; i++)
            {
                if (graph[i].Deleted) continue;
                if (graph[i].IncomingEdges.Count == 0)
                {
                    // source tip
                    tipCount += graph[i].OutgoingEdges.Count;
                    for (int j = 0; j < graph[i].OutgoingEdges.Count; j++)
                    {
                        graph[graph[i].OutgoingEdges[j].Connection].IncomingEdges.RemoveAll(x => x.Connection == i);
                    }
                    graph[i].Deleted = true;
                }
                if (graph[i].OutgoingEdges.Count == 0)
                {
                    // target tip
                    tipCount += graph[i].IncomingEdges.Count;
                    for (int j = 0; j < graph[i].IncomingEdges.Count; j++)
                    {
                        graph[graph[i].IncomingEdges[j].Connection].OutgoingEdges.RemoveAll(x => x.Connection == i);
                    }
                    graph[i].Deleted = true;
                }
            }

            return tipCount;
        }

        public static string Solve(string[] input)
        {
            var kmers = GetKMers(input.Skip(1).ToArray(), input.Length >= 15 ? 15 : 3).ToArray();
            ConstructGraph(kmers);
            var removedTips = 0;
            int tips;
            do
            {
                tips = RemoveTips();
                removedTips += tips;
            } while (tips > 0);
            return removedTips.ToString();
        }

        static void Main(string[] args)
        {
            var input = new List<string>();
            string inLine;
            while ((inLine = Console.ReadLine()) != null)
            {
                input.Add(inLine);
            }
            Console.WriteLine(Solve(input.ToArray()));
        }
    }
}
